// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hair05" {

	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[HideInInspector]_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[HideInInspector]_Metallic ("Metallic", Range(0,1)) = 0.0

    	_Density ("Fur Density", Range(0,12)) = 1
    	_Volume ("Fur Volume", Range(-10,10)) = 0.0

    	_Fuziness ("Fur Fuziness", Range(0,1)) = 0.5
    	_FurDepth("Fur Base", Range(0,1)) = 0.5
    	[HideInInspector]_FurLight("Fur Light", Range(0,1)) = 0.5

    	[HideInInspector]_Threshold("Threshold", Range(0,1)) = 0.25
    	_Cutoff ("Shadow", Range(0,1)) = 0.5
	}
	
	SubShader{

		Tags {
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}
		
		Pass {
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite OFF
            Cull OFF
				
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"
 

            uniform sampler2D _MainTex;
            uniform sampler2D _CameraDepthTexture; //Depth Texture

            uniform float _Metallic;

            //HAIR
            uniform int _Density;
            uniform float _Volume;

            uniform float _Fuziness;


            uniform float _FurLight;
            uniform float _Threshold;
            uniform float _FurDepth;

            #define TAM 36

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal: NORMAL;
               
            };
               
			struct v2g
			{
				float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal: NORMAL;

                float4 projPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
			};

			struct g2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal: NORMAL;

                float4 projPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            v2g vert(VertexInput v) {
                v2g o;

                o.vertex = v.vertex;

                o.uv = v.uv;
                o.normal = v.normal;

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

						tri.worldPos = p[k].worldPos;
						tri.projPos = p[k].projPos;

						triStream.Append(tri);
					}
					triStream.RestartStrip();
				}
            }


            half4 frag(g2f input) : SV_Target {

				float4 mainTex = tex2D(_MainTex, input.uv);
                float4 output = float4(1,1,1,1);

                //NORMAL
                float3 N = normalize(input.normal);
                float3 V = normalize(_WorldSpaceCameraPos.xyz - input.worldPos.xyz);
                float NdotL = max(dot(-N, V), dot(N, V));


                //DEPTH
                //Get the distance to the camera from the depth buffer for this point
                float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(input.projPos)).r);
                float partZ = input.projPos.z;
                float depth = clamp( pow((abs(sceneZ - partZ)) / _Threshold, 1), 0 , 1);

                output.rgb = mainTex.rgb;


                //float depth = clamp( pow((abs(sceneZ - partZ)) / _Threshold, 1), 0 , 1);

                //float alpha = mainTex.a - (1 - _Fuziness);
                //alpha = lerp(alpha, mainTex.a,  pow(1 - depth, 1.3 - NdotL));

                //float fur_inside = pow(clamp( pow((abs(sceneZ - partZ)) / _Threshold, 1), 0 , 1), 1.3 - NdotL);

                //output.rgb = lerp(float3(1,0,0), float3(0,1,0), alpha);
                  
                //output.a =  lerp(0, alpha, pow(depth, 1.3 - NdotL));

                float silhouette = pow(depth, 1.3 - NdotL);

                float furFuziness = clamp(mainTex.a + (_Fuziness - 0.5),  0,1);
                float furRoot = pow(clamp( pow((abs(sceneZ - partZ)) / (1-_FurDepth), 1), 0 , 1), 1.3 - NdotL);

                float furLight = clamp(mainTex.a - (_FurLight + 0.5),  0, 1);
                furLight = lerp(mainTex.a, furLight, silhouette);

                float test = 1;
                output.rgb = mainTex.rgb;
                //output.rgb = lerp( float3(0,1,0), float3(1,0,0), furLight);// 1-furLight);
                //output.rgb = mainTex.rgb + (1-furLight)*float3(1,1,1);


                //output.rgb = lerp(float3(1,0,0), float3(0,1,0), furLight);
                output.a = mainTex.a;
                output.a = lerp(0, output.a, furRoot);
                output.a = lerp(0, output.a, furFuziness);

                output.a = lerp(0, output.a, test);

                return output;
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