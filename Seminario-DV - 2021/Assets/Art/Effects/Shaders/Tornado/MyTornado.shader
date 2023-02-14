// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Tornado"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = -0.33
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Texture("Texture", 2D) = "white" {}
		_DisplacementNOise("Displacement NOise", 2D) = "white" {}
		_Speed("Speed", Vector) = (-4,-4,0,0)
		_Colores1("Colores1", Color) = (0.02858604,0.8156863,0,1)
		_DisplacementValue("DisplacementValue", Float) = 0
		_Colores2("Colores2", Color) = (1,0.240346,0,1)
		_ChangeMASK("ChangeMASK", Range( 0 , 1)) = 0
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Float1("Float 1", Range( 0 , 1)) = 0
		_opacity("opacity", Float) = 0
		_off("off", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture;
		uniform float _off;
		uniform sampler2D _DisplacementNOise;
		uniform float _DisplacementValue;
		uniform sampler2D _TextureSample0;
		uniform float2 _Speed;
		uniform float _Float0;
		uniform float4 _Colores1;
		uniform float _Float1;
		uniform float4 _Colores2;
		uniform float _opacity;
		uniform float _ChangeMASK;
		uniform float _Cutoff = -0.33;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 panner50 = ( 1.0 * _Time.y * float2( -1,-1 ) + v.texcoord.xy);
			float4 disp56 = ( tex2Dlod( _Texture, float4( panner50, 0, 0.0) ) * _off * tex2Dlod( _DisplacementNOise, float4( panner50, 0, 0.0) ).r * _DisplacementValue );
			v.vertex.xyz += disp56.rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner2 = ( 1.0 * _Time.y * _Speed + i.uv_texcoord);
			float4 tex2DNode4 = tex2D( _TextureSample0, panner2 );
			float temp_output_20_0 = step( tex2DNode4.r , _Float0 );
			float temp_output_24_0 = step( tex2DNode4.r , _Float1 );
			float4 Albedo27 = ( ( temp_output_20_0 * _Colores1 ) + ( temp_output_24_0 * _Colores2 ) );
			float4 temp_output_34_0 = Albedo27;
			o.Albedo = temp_output_34_0.rgb;
			o.Emission = temp_output_34_0.rgb;
			o.Alpha = _opacity;
			float temp_output_85_0 = ( temp_output_20_0 + temp_output_24_0 );
			float lerpResult96 = lerp( temp_output_85_0 , ( 1.0 - temp_output_85_0 ) , _ChangeMASK);
			float myMask87 = lerpResult96;
			clip( myMask87 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;408.6667;1003;273;5405.468;1692.147;5.299564;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-3493.686,-1206.747;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;3;-3473.896,-986.2022;Float;False;Property;_Speed;Speed;5;0;Create;True;0;0;False;0;-4,-4;-0.2,-0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;2;-3129.112,-1095.625;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-2826.532,-1118.987;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;None;1d26076aa317e6546ae689297ca69165;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;98;-2694.823,-1353.729;Float;False;Property;_Float0;Float 0;11;0;Create;True;0;0;False;0;0;0.29;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-2794.745,-837.2439;Float;False;Property;_Float1;Float 1;12;0;Create;True;0;0;False;0;0;0.48;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;20;-2326.733,-1295.162;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.39;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;24;-2395.268,-891.6356;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.52;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-1098.765,206.2009;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;49;-1037.914,419.609;Float;False;Constant;_SpeedMVerteznnORMAL;SpeedMVerteznnORMAL;7;0;Create;True;0;0;False;0;-1,-1;-1,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-1792.095,-1269.131;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-2129.877,-841.334;Float;False;Property;_Colores1;Colores1;6;0;Create;True;0;0;False;0;0.02858604,0.8156863,0,1;1,0.009313838,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;50;-717.5908,339.4569;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;26;-2279.461,-320.1334;Float;False;Property;_Colores2;Colores2;8;0;Create;True;0;0;False;0;1,0.240346,0,1;0.4811321,0.4811321,0.4811321,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;-412.9931,562.539;Inherit;True;Property;_DisplacementNOise;Displacement NOise;4;0;Create;True;0;0;False;0;-1;None;0a832fc5001a31247ba848f2ea3d25ee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;86;-1408.847,-1100.443;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-235.054,1043.168;Float;False;Property;_DisplacementValue;DisplacementValue;7;0;Create;True;0;0;False;0;0;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1460.228,-973.2275;Float;False;Property;_ChangeMASK;ChangeMASK;10;0;Create;True;0;0;False;0;0;0.87;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-420.5431,338.23;Inherit;True;Property;_Texture;Texture;3;0;Create;True;0;0;False;0;-1;None;1d26076aa317e6546ae689297ca69165;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;101;-209.3804,873.0737;Inherit;False;Property;_off;off;14;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1739.838,-840.2245;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-1773.699,-349.309;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;42.31096,678.2329;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;96;-1061.097,-1264.548;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-1388.63,-548.9814;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-858.2415,-661.4777;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;361.3716,596.9589;Float;False;disp;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;-752.5141,-1265.052;Float;False;myMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;989.4489,-10.52357;Inherit;False;27;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;29;-2255.804,2219.898;Float;False;Constant;_SpeedMask;SpeedMask;6;0;Create;True;0;0;False;0;-1,-1;-1,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;36;989.4857,225.0805;Inherit;False;87;myMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;67;-857.8804,2208.189;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;38;-1349.468,2099.485;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.39;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1105.862,2405.073;Float;False;Property;_SeleccionadorDeMaska;SeleccionadorDeMaska;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;30;-1935.481,2139.747;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;28;-2316.655,2006.491;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;59;-1087.148,2288.385;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-1638.433,2138.519;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;-1;None;1d26076aa317e6546ae689297ca69165;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1183.787,2150.398;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;1066.883,167.3023;Inherit;False;Property;_opacity;opacity;13;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;969.046,319.9343;Inherit;False;56;disp;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;39;-1391.983,2319.127;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.52;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;7;1294.494,0;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Tornado;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;-0.33;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;2;2;3;0
WireConnection;4;1;2;0
WireConnection;20;0;4;1
WireConnection;20;1;98;0
WireConnection;24;0;4;1
WireConnection;24;1;99;0
WireConnection;85;0;20;0
WireConnection;85;1;24;0
WireConnection;50;0;48;0
WireConnection;50;2;49;0
WireConnection;52;1;50;0
WireConnection;86;0;85;0
WireConnection;51;1;50;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;95;0;24;0
WireConnection;95;1;26;0
WireConnection;53;0;51;0
WireConnection;53;1;101;0
WireConnection;53;2;52;1
WireConnection;53;3;55;0
WireConnection;96;0;85;0
WireConnection;96;1;86;0
WireConnection;96;2;97;0
WireConnection;23;0;21;0
WireConnection;23;1;95;0
WireConnection;27;0;23;0
WireConnection;56;0;53;0
WireConnection;87;0;96;0
WireConnection;67;0;40;0
WireConnection;67;1;59;0
WireConnection;67;2;68;0
WireConnection;38;0;31;1
WireConnection;30;0;28;0
WireConnection;30;2;29;0
WireConnection;59;0;40;0
WireConnection;31;1;30;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;39;0;31;1
WireConnection;7;0;34;0
WireConnection;7;2;34;0
WireConnection;7;9;100;0
WireConnection;7;10;36;0
WireConnection;7;11;57;0
ASEEND*/
//CHKSM=32D233BA74B0D1B4D74210AEF660D5412CCD5EF0