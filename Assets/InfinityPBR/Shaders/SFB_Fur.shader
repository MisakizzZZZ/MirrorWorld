// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "SFBayStudios/Fur" {

	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[HideInInspector]_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[HideInInspector]_Metallic ("Metallic", Range(0,1)) = 0.0

    	_Density ("Fur Density", Range(0,12)) = 1
    	_Volume ("Fur Volume", Range(-10,10)) = 0.0

    	_Fuziness ("Fur Fuziness", Range(0,1)) = 0.5
    	_FurDepth("Fur Base", Range(0,1)) = 0.5
    	_FurLight("Fur Light", Range(0,1)) = 0.5

    	[HideInInspector]_Threshold("Threshold", Range(0,1)) = 0.25
    	_Cutoff ("Shadow", Range(0,1)) = 0.5
	}
	
	SubShader{

		Tags {
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}
		
		Pass {
			Name "FORWARD"
			Tags {
                "LightMode"="ForwardBase"
            }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite OFF
            Cull OFF
				
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"
 

            uniform sampler2D _MainTex;
            uniform sampler2D _Normal;
            uniform sampler2D _CameraDepthTexture; //Depth Texture

            uniform float _Metallic;

            //HAIR
            uniform int _Density;
            uniform float _Volume;

            uniform float _Fuziness;


            uniform float _FurLight;
            uniform float _Threshold;
            uniform float _FurDepth;

            uniform float4 _LightColor0;

            #define TAM 36

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal: NORMAL;
                float4 tangent: TANGENT;
            };
               
			struct v2g
			{
				float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal: NORMAL;
                float4 tangent: TANGENT;

                float4 projPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;

			};

			struct g2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal: NORMAL;
                float4 tangent: TANGENT;

                float4 projPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            v2g vert(VertexInput v) {
                v2g o;

                o.vertex = v.vertex;

                o.uv = v.uv;
                o.normal = v.normal;
                o.tangent = v.tangent;

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                float4 pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos(pos);

                return o;
            }


            [maxvertexcount(TAM)] //point || triangle
            void geom(triangle v2g p[3], inout TriangleStream<g2f> triStream){

            	g2f tri;
				for (int j = 0; j < _Density; j++){
					for (int k = 0; k < 3; k++){
						float4 adjust = float4( p[k].vertex.xyz + p[k].normal * _Volume * j, p[k].vertex.w);
						
						tri.pos = UnityObjectToClipPos(adjust);
						tri.uv = p[k].uv;
						tri.normal = p[k].normal;
						tri.tangent = p[k].tangent;

						tri.worldPos = p[k].worldPos;
						tri.projPos = p[k].projPos;

						triStream.Append(tri);
					}
					triStream.RestartStrip();
				}
            }

            //REFLECTION
            float3 R(float3 L, float3 N){
            	return L - 2.0 * dot(N, L) * N;
            }

            float3 unpack(float4 input){
            	#if defined(SHADER_API_GLES) && defined(SHADER_API_MOBILE)
            		return input.xyz * 2 - 1;
            	#else
            		float3 normal;
            		normal.xy = input.wy * 2 - 1;
            		normal.z = sqrt(1 - normal.x*normal.x - normal.y * normal.y);
				    return normal;
				#endif
            }

            float GetSpecularBlinn(float3 L, float3 N, float3 V, float gloss){
            	
                float shininess = exp2( gloss * 10.0 + 1.0);
            	float3 specular = float3(1,1,1);

            	float3 H = normalize(L + V);
            	specular *= pow( max(0, dot(H, N)), shininess);

            	return specular;
            }

             float GetSpecularPhong(float3 L, float3 N, float3 V, float gloss){
            	
                float shininess = exp2( gloss * 10.0 + 1.0);
            	float3 specular = float3(1,1,1);


            	specular *= pow( max(0, dot(R(-L, N), V)), shininess);
            	//if (dot(N, L) < 0.0){
            	//	specular = float3(0.0, 0.0, 0.0);
            	//}

            	return specular;
            }


            half4 frag(g2f input) : SV_Target {

				//MATRIX
				float4x4 modelMatrix = unity_ObjectToWorld;
            	float4x4 modelMatrixInverse = unity_WorldToObject;
 
             	float3 tangentWorld = normalize( mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);
            	float3 normalWorld = normalize( mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            	float3 binormalWorld = normalize( cross(normalWorld, tangentWorld) * input.tangent.w);

            	float3x3 tangentTransform = float3x3(tangentWorld, binormalWorld, normalWorld);
         		
            	
            	//TEXTURE
				float4 mainTex = tex2D(_MainTex, input.uv);
                float4 output = float4(1,1,1,1);

                float3 normalLocal = unpack(tex2D(_Normal, input.uv));


                //LIGHT
                float3 L = normalize(_WorldSpaceLightPos0.xyz);
				//float3 lightDir = normalize(_lightPos.xyz - input.worldPos.xyz);

				//NORMAL
				//float3 N =  normalize(mul(normalLocal, tangentTransform));
				float3 N = normalize(input.normal);	
				float NdotL = dot(N, L);

                //CAMERA NORMAL
                float3 V = normalize(_WorldSpaceCameraPos.xyz - input.worldPos.xyz);
                float NdotV = max(dot(-N, V), dot(N, V));


                //** DEPTH
                float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(input.projPos)).r);
                float partZ = input.projPos.z;
                float depth = clamp( pow((abs(sceneZ - partZ)) / _Threshold, 1), 0 , 1);




                float silhouette = pow(depth, 1.3 - NdotL);

                float furFuziness = clamp(mainTex.a + (_Fuziness - 0.5),  0,1);
                float furRoot = pow(clamp( pow((abs(sceneZ - partZ)) / (1-_FurDepth), 1), 0 , 1), 1.3 - NdotL);

                float furLight = clamp(mainTex.a - (_FurLight + 0.5),  0, 1);
                //furLight = lerp(mainTex.a, furLight, silhouette);


                //** COLOR
                float3 light = NdotL * _LightColor0;
                float3 diffuse = mainTex.rgb * light;
                float3 specular = GetSpecularPhong(L, N, V, 0.5) * light;

                output.rgb = lerp(mainTex.rgb, diffuse, _FurLight) + lerp(0, specular, _FurLight);

                //** ALPHA
                output.a = mainTex.a;
                //**** DEPTH IMPROVEMENT
                output.a = lerp(0, output.a, furRoot);
                output.a = lerp(0, output.a, furFuziness);

                //output.a = 0;

                return output;
            }

            ENDCG
        }

        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 diffuseColor = _MainTex_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * _MainTex_var.a,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }

        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0

            uniform sampler2D _MainTex; 
            uniform float4 _MainTex_ST;

            uniform float _Cutoff;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - (1-_Cutoff));
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
	}
	//FallBack "Diffuse"
}