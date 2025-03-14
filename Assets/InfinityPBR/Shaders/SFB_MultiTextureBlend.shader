// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SFBayStudios/SFB_MultiTextureBlender"
{
	Properties
	{
		_HueShift("Hue Shift", Range( -1 , 1)) = 0
		_Saturation("Saturation", Range( 0 , 1)) = 0
		_TextureBlend("Texture Blend", Range( 0 , 1)) = 0
		_TextureBlend2("Texture Blend 2", Range( 0 , 1)) = 0
		_TextureBlend3("Texture Blend 3", Range( 0 , 1)) = 0
		_HeightMultiplier("Height Multiplier", Range( 0 , 1)) = 1
		_HeightMultiplier2("Height Multiplier 2", Range( 0 , 1)) = 1
		_HieghtMultiplier3("Hieght Multiplier 3", Range( 0 , 1)) = 1
		[Toggle(_USEEMISSION_ON)] _UseEmission("Use Emission?", Float) = 0
		_EmissionMultplier("Emission Multplier", Range( 0 , 10)) = 1
		[Toggle]_PulseEmission("Pulse Emission", Float) = 0
		_AlbedoOpacity("AlbedoOpacity", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_MetalAOHeightRough("MetalAOHeightRough", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_AlbedoOpacity2("AlbedoOpacity2", 2D) = "white" {}
		_Normal2("Normal2", 2D) = "bump" {}
		_MetalAOHeightRough2("MetalAOHeightRough2", 2D) = "white" {}
		_Emission2("Emission2", 2D) = "white" {}
		_AlbedoOpacity3("AlbedoOpacity3", 2D) = "white" {}
		_Normal3("Normal3", 2D) = "bump" {}
		_MetalAOHeightRough3("MetalAOHeightRough3", 2D) = "white" {}
		_Emission3("Emission3", 2D) = "white" {}
		_AlbedoOpacity4("AlbedoOpacity4", 2D) = "white" {}
		_Normal4("Normal4", 2D) = "bump" {}
		_MetalAOHeightRough4("MetalAOHeightRough4", 2D) = "white" {}
		_Emission4("Emission4", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEEMISSION_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Normal2;
		uniform float4 _Normal2_ST;
		uniform float _TextureBlend;
		uniform sampler2D _MetalAOHeightRough2;
		uniform float4 _MetalAOHeightRough2_ST;
		uniform float _HeightMultiplier;
		uniform sampler2D _Normal3;
		uniform float4 _Normal3_ST;
		uniform float _TextureBlend2;
		uniform sampler2D _MetalAOHeightRough3;
		uniform float4 _MetalAOHeightRough3_ST;
		uniform float _HeightMultiplier2;
		uniform sampler2D _Normal4;
		uniform float4 _Normal4_ST;
		uniform float _TextureBlend3;
		uniform sampler2D _MetalAOHeightRough4;
		uniform float4 _MetalAOHeightRough4_ST;
		uniform float _HieghtMultiplier3;
		uniform float _HueShift;
		uniform sampler2D _AlbedoOpacity;
		uniform float4 _AlbedoOpacity_ST;
		uniform sampler2D _AlbedoOpacity2;
		uniform float4 _AlbedoOpacity2_ST;
		uniform sampler2D _AlbedoOpacity3;
		uniform float4 _AlbedoOpacity3_ST;
		uniform sampler2D _AlbedoOpacity4;
		uniform float4 _AlbedoOpacity4_ST;
		uniform float _Saturation;
		uniform float _PulseEmission;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform sampler2D _Emission2;
		uniform float4 _Emission2_ST;
		uniform sampler2D _Emission3;
		uniform float4 _Emission3_ST;
		uniform sampler2D _Emission4;
		uniform float4 _Emission4_ST;
		uniform float _EmissionMultplier;
		uniform sampler2D _MetalAOHeightRough;
		uniform float4 _MetalAOHeightRough_ST;


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		float3 RGBToHSV(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
			float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
			float d = q.x - min( q.w, q.y );
			float e = 1.0e-10;
			return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float2 uv_Normal2 = i.uv_texcoord * _Normal2_ST.xy + _Normal2_ST.zw;
			float2 uv_MetalAOHeightRough2 = i.uv_texcoord * _MetalAOHeightRough2_ST.xy + _MetalAOHeightRough2_ST.zw;
			float4 tex2DNode5 = tex2D( _MetalAOHeightRough2, uv_MetalAOHeightRough2 );
			float clampResult27 = clamp( ( _TextureBlend + ( _TextureBlend * ( tex2DNode5.b * _HeightMultiplier ) ) ) , 0.0 , 1.0 );
			float3 lerpResult12 = lerp( UnpackNormal( tex2D( _Normal, uv_Normal ) ) , UnpackNormal( tex2D( _Normal2, uv_Normal2 ) ) , clampResult27);
			float2 uv_Normal3 = i.uv_texcoord * _Normal3_ST.xy + _Normal3_ST.zw;
			float2 uv_MetalAOHeightRough3 = i.uv_texcoord * _MetalAOHeightRough3_ST.xy + _MetalAOHeightRough3_ST.zw;
			float4 tex2DNode52 = tex2D( _MetalAOHeightRough3, uv_MetalAOHeightRough3 );
			float clampResult66 = clamp( ( _TextureBlend2 + ( _TextureBlend2 * ( tex2DNode52.b * _HeightMultiplier2 ) ) ) , 0.0 , 1.0 );
			float3 lerpResult56 = lerp( lerpResult12 , UnpackNormal( tex2D( _Normal3, uv_Normal3 ) ) , clampResult66);
			float2 uv_Normal4 = i.uv_texcoord * _Normal4_ST.xy + _Normal4_ST.zw;
			float2 uv_MetalAOHeightRough4 = i.uv_texcoord * _MetalAOHeightRough4_ST.xy + _MetalAOHeightRough4_ST.zw;
			float4 tex2DNode53 = tex2D( _MetalAOHeightRough4, uv_MetalAOHeightRough4 );
			float clampResult69 = clamp( ( _TextureBlend3 + ( _TextureBlend3 * ( tex2DNode53.b * _HieghtMultiplier3 ) ) ) , 0.0 , 1.0 );
			float3 lerpResult57 = lerp( lerpResult56 , UnpackNormal( tex2D( _Normal4, uv_Normal4 ) ) , clampResult69);
			float3 normalizeResult22 = normalize( lerpResult57 );
			o.Normal = normalizeResult22;
			float2 uv_AlbedoOpacity = i.uv_texcoord * _AlbedoOpacity_ST.xy + _AlbedoOpacity_ST.zw;
			float4 tex2DNode1 = tex2D( _AlbedoOpacity, uv_AlbedoOpacity );
			float2 uv_AlbedoOpacity2 = i.uv_texcoord * _AlbedoOpacity2_ST.xy + _AlbedoOpacity2_ST.zw;
			float4 tex2DNode4 = tex2D( _AlbedoOpacity2, uv_AlbedoOpacity2 );
			float4 blendOpSrc83 = tex2DNode1;
			float4 blendOpDest83 = tex2DNode4;
			float4 lerpResult84 = lerp( ( saturate( ( 1.0 - ( ( 1.0 - blendOpDest83) / blendOpSrc83) ) )) , tex2DNode4 , clampResult27);
			float4 lerpResult10 = lerp( tex2DNode1 , lerpResult84 , clampResult27);
			float2 uv_AlbedoOpacity3 = i.uv_texcoord * _AlbedoOpacity3_ST.xy + _AlbedoOpacity3_ST.zw;
			float4 tex2DNode48 = tex2D( _AlbedoOpacity3, uv_AlbedoOpacity3 );
			float4 blendOpSrc85 = lerpResult10;
			float4 blendOpDest85 = tex2DNode48;
			float4 lerpResult86 = lerp( ( saturate( ( 1.0 - ( ( 1.0 - blendOpDest85) / blendOpSrc85) ) )) , tex2DNode48 , clampResult66);
			float4 lerpResult70 = lerp( lerpResult10 , lerpResult86 , clampResult66);
			float2 uv_AlbedoOpacity4 = i.uv_texcoord * _AlbedoOpacity4_ST.xy + _AlbedoOpacity4_ST.zw;
			float4 tex2DNode49 = tex2D( _AlbedoOpacity4, uv_AlbedoOpacity4 );
			float4 blendOpSrc87 = lerpResult70;
			float4 blendOpDest87 = tex2DNode49;
			float4 lerpResult88 = lerp( ( saturate( ( 1.0 - ( ( 1.0 - blendOpDest87) / blendOpSrc87) ) )) , tex2DNode49 , clampResult69);
			float4 lerpResult71 = lerp( lerpResult70 , lerpResult88 , clampResult69);
			float3 hsvTorgb92 = RGBToHSV( lerpResult71.rgb );
			float3 hsvTorgb89 = HSVToRGB( float3(( _HueShift + hsvTorgb92.x ),( _Saturation * hsvTorgb92.y ),hsvTorgb92.z) );
			o.Albedo = hsvTorgb89;
			float4 temp_cast_1 = 0;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float2 uv_Emission2 = i.uv_texcoord * _Emission2_ST.xy + _Emission2_ST.zw;
			float4 lerpResult19 = lerp( tex2D( _Emission, uv_Emission ) , tex2D( _Emission2, uv_Emission2 ) , clampResult27);
			float2 uv_Emission3 = i.uv_texcoord * _Emission3_ST.xy + _Emission3_ST.zw;
			float4 lerpResult80 = lerp( lerpResult19 , tex2D( _Emission3, uv_Emission3 ) , clampResult66);
			float2 uv_Emission4 = i.uv_texcoord * _Emission4_ST.xy + _Emission4_ST.zw;
			float4 lerpResult81 = lerp( lerpResult80 , tex2D( _Emission4, uv_Emission4 ) , clampResult69);
			float temp_output_40_0 = ( ( 1 + _SinTime.w ) / 2 );
			#ifdef _USEEMISSION_ON
				float4 staticSwitch46 = lerp(lerpResult81,( ( _EmissionMultplier * temp_output_40_0 ) * lerpResult81 ),_PulseEmission);
			#else
				float4 staticSwitch46 = temp_cast_1;
			#endif
			o.Emission = staticSwitch46.rgb;
			float2 uv_MetalAOHeightRough = i.uv_texcoord * _MetalAOHeightRough_ST.xy + _MetalAOHeightRough_ST.zw;
			float4 tex2DNode3 = tex2D( _MetalAOHeightRough, uv_MetalAOHeightRough );
			float lerpResult13 = lerp( tex2DNode3.r , tex2DNode5.r , clampResult27);
			float lerpResult75 = lerp( lerpResult13 , tex2DNode52.r , clampResult66);
			float lerpResult79 = lerp( lerpResult75 , tex2DNode53.r , clampResult69);
			o.Metallic = lerpResult79;
			float lerpResult16 = lerp( tex2DNode3.a , tex2DNode5.a , clampResult27);
			float lerpResult74 = lerp( lerpResult16 , tex2DNode52.a , clampResult66);
			float lerpResult78 = lerp( lerpResult74 , tex2DNode53.a , clampResult69);
			o.Smoothness = lerpResult78;
			float lerpResult14 = lerp( tex2DNode3.g , tex2DNode5.g , clampResult27);
			float lerpResult72 = lerp( lerpResult14 , tex2DNode52.g , clampResult66);
			float lerpResult76 = lerp( lerpResult72 , tex2DNode53.g , clampResult69);
			o.Occlusion = lerpResult76;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
5;94;1226;874;1469.827;2457.489;2.405286;True;True
Node;AmplifyShaderEditor.SamplerNode;5;-1155.274,135.3491;Float;True;Property;_MetalAOHeightRough2;MetalAOHeightRough2;18;0;Create;True;0;0;False;0;None;1afd2de9ce59a4ed0be0dde5afe9060d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-2250.276,178.4709;Float;False;Property;_HeightMultiplier;Height Multiplier;5;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2255.623,-393.1088;Float;False;Property;_TextureBlend;Texture Blend;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1798.426,89.18868;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1893.612,-410.6986;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-2244.611,282.8177;Float;False;Property;_HeightMultiplier2;Height Multiplier 2;6;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1188.561,-1657.229;Float;True;Property;_AlbedoOpacity2;AlbedoOpacity2;16;0;Create;True;0;0;False;0;None;c3d579a806b334a2fbe71cf4bb1961d4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1198.005,-1862.021;Float;True;Property;_AlbedoOpacity;AlbedoOpacity;12;0;Create;True;0;0;False;0;None;b3c7df93b2fbe4355ab5d4e7816274b1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1770.545,-517.2279;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;-1149.853,359.3111;Float;True;Property;_MetalAOHeightRough3;MetalAOHeightRough3;22;0;Create;True;0;0;False;0;None;120b1c2a7ebee4d4bb38187a4b5b8151;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-2254.931,-297.7001;Float;False;Property;_TextureBlend2;Texture Blend 2;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;27;-1547.148,-460.1686;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1782.078,254.7719;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;83;-784.3953,-2024.536;Float;False;ColorBurn;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1888.975,-211.7308;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;-613.6203,-1918.703;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;-496.4946,-1788.72;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;-1146.826,585.4793;Float;True;Property;_MetalAOHeightRough4;MetalAOHeightRough4;26;0;Create;True;0;0;False;0;None;b65dc4ca25c484bb68132e8a56ef753d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;59;-2239.451,383.4408;Float;False;Property;_HieghtMultiplier3;Hieght Multiplier 3;7;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-1184.912,-1455.893;Float;True;Property;_AlbedoOpacity3;AlbedoOpacity3;20;0;Create;True;0;0;False;0;None;50dc184de4c3c4078a6bca8f7b7f34bc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-1765.908,-318.2601;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;66;-1542.511,-261.2008;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;85;-732.6825,-1644.502;Float;False;ColorBurn;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-2245.683,-199.657;Float;False;Property;_TextureBlend3;Texture Blend 3;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1783.388,390.332;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;86;-578.7444,-1543.479;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-1878.756,-52.30305;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;-1177.294,-1265.466;Float;True;Property;_AlbedoOpacity4;AlbedoOpacity4;24;0;Create;True;0;0;False;0;None;76a0f17a4315a4260a58be6e3a713ddd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;70;-380.3079,-1467.56;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-1755.689,-158.8324;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;37;-279.5576,1884.228;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;38;-295.2196,1792.485;Float;False;Constant;_Int0;Int 0;12;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;18;-1150.612,1094.23;Float;True;Property;_Emission2;Emission2;19;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1145.825,873.3182;Float;True;Property;_Emission;Emission;15;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;41;40.42147,1940.167;Float;False;Constant;_Int1;Int 1;12;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.ClampOpNode;69;-1532.292,-101.7731;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;19;-666.0433,937.051;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;54;-1146.825,1320.525;Float;True;Property;_Emission3;Emission3;23;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-46.84402,1864.088;Float;False;2;2;0;INT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;87;-655.7129,-1322.194;Float;False;ColorBurn;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;88;-484.9379,-1192.308;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-1180.839,-926.0229;Float;True;Property;_Normal;Normal;13;0;Create;True;0;0;False;0;None;c1024ec2548d947d0b6234f5c3602df4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;80;-566.036,1100.65;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;40;206.0073,1826.049;Float;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1177.441,-717.4224;Float;True;Property;_Normal2;Normal2;17;0;Create;True;0;0;False;0;None;ebf15dff6ccfe4bd9a4f5dfabd0e1311;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;55;-1143.017,1545.229;Float;True;Property;_Emission4;Emission4;27;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-150.8552,1427.358;Float;False;Property;_EmissionMultplier;Emission Multplier;9;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;71;-295.2131,-1143.29;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1154.179,-64.36864;Float;True;Property;_MetalAOHeightRough;MetalAOHeightRough;14;0;Create;True;0;0;False;0;None;a40f847fb32f34b60a1c8283d8876375;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;12;-751.3425,-880.0248;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;275.2147,1476.981;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-1175.723,-511.3773;Float;True;Property;_Normal3;Normal3;21;0;Create;True;0;0;False;0;None;40ccabb71fec247cbb41b6b25a2fda2b;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;81;-502.4619,1294.399;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-38.68189,-1834.519;Float;False;Property;_HueShift;Hue Shift;0;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RGBToHSVNode;92;-38.68203,-1322.191;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;56;-596.2189,-658.5931;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-175.7832,-1593.991;Float;False;Property;_Saturation;Saturation;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;16;-621.2936,301.3376;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-1165.869,-309.5251;Float;True;Property;_Normal4;Normal4;25;0;Create;True;0;0;False;0;None;61a3ca32ba7a64a1c907bde513acdb2d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;-618.7888,33.91597;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;539.1274,1255.426;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;13;-623.7231,-112.3561;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;103.23,-1464.106;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;72;-424.8897,150.6721;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;47;797.5086,1230.172;Float;False;Constant;_Int2;Int 2;14;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;44;738.4549,1080.271;Float;False;Property;_PulseEmission;Pulse Emission;10;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;74;-418.3125,418.0938;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;75;-420.742,4.400039;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;96;276.4106,-1723.876;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;57;-418.6764,-416.4894;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;78;-211.179,534.7886;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.HSVToRGBNode;89;269.1948,-1319.787;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StaticSwitch;46;1063.665,1207.625;Float;False;Property;_UseEmission;Use Emission?;8;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;77;-215.0414,400.942;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;43;-317.5956,1647.04;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;79;-213.6085,121.095;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;22;-141.006,-398.0033;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;4.620588,1593.337;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;76;-217.7562,267.3671;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;-625.156,167.4909;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;73;-422.1749,284.247;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-335.4975,1564.248;Float;False;Property;_TimeSpeed;Time Speed;11;0;Create;True;0;0;False;0;1;1;-5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1405.509,198.2501;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SFBayStudios/SFB_MultiTextureBlender;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;5;3
WireConnection;28;1;29;0
WireConnection;24;0;7;0
WireConnection;24;1;28;0
WireConnection;23;0;7;0
WireConnection;23;1;24;0
WireConnection;27;0;23;0
WireConnection;62;0;52;3
WireConnection;62;1;58;0
WireConnection;83;0;1;0
WireConnection;83;1;4;0
WireConnection;65;0;60;0
WireConnection;65;1;62;0
WireConnection;84;0;83;0
WireConnection;84;1;4;0
WireConnection;84;2;27;0
WireConnection;10;0;1;0
WireConnection;10;1;84;0
WireConnection;10;2;27;0
WireConnection;64;0;60;0
WireConnection;64;1;65;0
WireConnection;66;0;64;0
WireConnection;85;0;10;0
WireConnection;85;1;48;0
WireConnection;63;0;53;3
WireConnection;63;1;59;0
WireConnection;86;0;85;0
WireConnection;86;1;48;0
WireConnection;86;2;66;0
WireConnection;68;0;61;0
WireConnection;68;1;63;0
WireConnection;70;0;10;0
WireConnection;70;1;86;0
WireConnection;70;2;66;0
WireConnection;67;0;61;0
WireConnection;67;1;68;0
WireConnection;69;0;67;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;19;2;27;0
WireConnection;39;0;38;0
WireConnection;39;1;37;4
WireConnection;87;0;70;0
WireConnection;87;1;49;0
WireConnection;88;0;87;0
WireConnection;88;1;49;0
WireConnection;88;2;69;0
WireConnection;80;0;19;0
WireConnection;80;1;54;0
WireConnection;80;2;66;0
WireConnection;40;0;39;0
WireConnection;40;1;41;0
WireConnection;71;0;70;0
WireConnection;71;1;88;0
WireConnection;71;2;69;0
WireConnection;12;0;2;0
WireConnection;12;1;6;0
WireConnection;12;2;27;0
WireConnection;34;0;31;0
WireConnection;34;1;40;0
WireConnection;81;0;80;0
WireConnection;81;1;55;0
WireConnection;81;2;69;0
WireConnection;92;0;71;0
WireConnection;56;0;12;0
WireConnection;56;1;50;0
WireConnection;56;2;66;0
WireConnection;16;0;3;4
WireConnection;16;1;5;4
WireConnection;16;2;27;0
WireConnection;14;0;3;2
WireConnection;14;1;5;2
WireConnection;14;2;27;0
WireConnection;32;0;34;0
WireConnection;32;1;81;0
WireConnection;13;0;3;1
WireConnection;13;1;5;1
WireConnection;13;2;27;0
WireConnection;95;0;94;0
WireConnection;95;1;92;2
WireConnection;72;0;14;0
WireConnection;72;1;52;2
WireConnection;72;2;66;0
WireConnection;44;0;81;0
WireConnection;44;1;32;0
WireConnection;74;0;16;0
WireConnection;74;1;52;4
WireConnection;74;2;66;0
WireConnection;75;0;13;0
WireConnection;75;1;52;1
WireConnection;75;2;66;0
WireConnection;96;0;93;0
WireConnection;96;1;92;1
WireConnection;57;0;56;0
WireConnection;57;1;51;0
WireConnection;57;2;69;0
WireConnection;78;0;74;0
WireConnection;78;1;53;4
WireConnection;78;2;69;0
WireConnection;89;0;96;0
WireConnection;89;1;95;0
WireConnection;89;2;92;3
WireConnection;46;1;47;0
WireConnection;46;0;44;0
WireConnection;77;0;73;0
WireConnection;77;1;53;3
WireConnection;77;2;69;0
WireConnection;79;0;75;0
WireConnection;79;1;53;1
WireConnection;79;2;69;0
WireConnection;22;0;57;0
WireConnection;42;0;36;0
WireConnection;42;1;40;0
WireConnection;76;0;72;0
WireConnection;76;1;53;2
WireConnection;76;2;69;0
WireConnection;15;0;3;3
WireConnection;15;1;5;3
WireConnection;15;2;27;0
WireConnection;73;0;15;0
WireConnection;73;1;52;3
WireConnection;73;2;66;0
WireConnection;0;0;89;0
WireConnection;0;1;22;0
WireConnection;0;2;46;0
WireConnection;0;3;79;0
WireConnection;0;4;78;0
WireConnection;0;5;76;0
ASEEND*/
//CHKSM=C944E9C4C2C214C6A877CE4640F90612B3FEBA07