// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Powermult("Power mult", Vector) = (3.6,3.1,0,0)
		_Frequency("Frequency", Float) = 1
		_Ampl("Ampl", Float) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_ColorBotton("ColorBotton", Color) = (1,1,1,0)
		_ColorUp("ColorUp", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPosition37;
		};

		uniform float _Frequency;
		uniform float _Ampl;
		uniform float4 _ColorUp;
		uniform float4 _ColorBotton;
		uniform float2 _Powermult;
		uniform sampler2D _TextureSample0;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 break24 = ase_vertex3Pos;
			float4 appendResult25 = (float4(break24.x , ( ( sin( ( ( break24.x * _Frequency ) + _Time.y ) ) * _Ampl ) + break24.y ) , break24.z , 0.0));
			float4 lerpResult35 = lerp( float4( ase_vertex3Pos , 0.0 ) , appendResult25 , v.texcoord.xy.y);
			v.vertex.xyz += lerpResult35.xyz;
			float3 vertexPos37 = ase_vertex3Pos;
			float4 ase_screenPos37 = ComputeScreenPos( UnityObjectToClipPos( vertexPos37 ) );
			o.screenPosition37 = ase_screenPos37;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 lerpResult20 = lerp( _ColorUp , _ColorBotton , ( 1.0 - saturate( ( pow( i.uv_texcoord.y , _Powermult.x ) * _Powermult.y ) ) ));
			float4 tex2DNode3 = tex2D( _TextureSample0, i.uv_texcoord );
			o.Albedo = ( lerpResult20 * tex2DNode3 ).rgb;
			float temp_output_14_0 = 0.0;
			o.Metallic = temp_output_14_0;
			o.Smoothness = temp_output_14_0;
			o.Alpha = 1;
			float4 ase_screenPos37 = i.screenPosition37;
			float4 ase_screenPosNorm37 = ase_screenPos37 / ase_screenPos37.w;
			ase_screenPosNorm37.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm37.z : ase_screenPosNorm37.z * 0.5 + 0.5;
			float screenDepth37 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm37.xy ));
			float distanceDepth37 = saturate( abs( ( screenDepth37 - LinearEyeDepth( ase_screenPosNorm37.z ) ) / ( 1.0 ) ) );
			clip( ( tex2DNode3.a * distanceDepth37 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
0;642;1366;359;2019.004;-230.437;1.94;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;23;-1334.397,636.3085;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1297.264,1009.285;Inherit;False;Property;_Frequency;Frequency;3;0;Create;True;0;0;False;0;1;3.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;24;-1066.636,823.3712;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.Vector2Node;17;-1678.154,-104.759;Inherit;False;Property;_Powermult;Power mult;1;0;Create;True;0;0;False;0;3.6,3.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;27;-1257.536,1154.954;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1070.693,954.7228;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1705.575,-219.7548;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;15;-1481.121,-112.671;Inherit;False;2;0;FLOAT;0;False;1;FLOAT2;1,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1040.382,1049.628;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-696.6072,1261.331;Inherit;False;Property;_Ampl;Ampl;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;29;-923.2367,1108.555;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1466.416,66.78269;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-648.0049,1138.306;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;19;-1284.144,61.32169;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-1127.295,-105.1337;Inherit;False;Property;_ColorUp;ColorUp;7;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-633.4888,1032.977;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;-1095.452,215.0474;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-1189.445,-281.1314;Inherit;False;Property;_ColorBotton;ColorBotton;6;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-802.578,44.33445;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;-1;None;88e7d77e1982d264091bd1d14ba2fe9e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;25;-316.5546,950.993;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;20;-847.0378,-199.4489;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-195.9114,1081.033;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;37;-928.1122,512.0406;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-736.8965,302.9248;Inherit;False;Property;_Threshold;Threshold;2;0;Create;True;0;0;False;0;1000;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-243.5711,230.8126;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-245.8634,30.15046;Inherit;False;Constant;_zero;zero;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;85.95918,940.0775;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-496.2204,-53.26075;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-394.0856,474.9918;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;11;499.3338,292.8675;Float;False;True;2;ASEMaterialInspector;0;0;Standard;Grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;1;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;23;0
WireConnection;30;0;24;0
WireConnection;30;1;26;0
WireConnection;15;0;2;2
WireConnection;15;1;17;0
WireConnection;31;0;30;0
WireConnection;31;1;27;0
WireConnection;29;0;31;0
WireConnection;18;0;15;0
WireConnection;18;1;17;2
WireConnection;32;0;29;0
WireConnection;32;1;33;0
WireConnection;19;0;18;0
WireConnection;34;0;32;0
WireConnection;34;1;24;1
WireConnection;21;0;19;0
WireConnection;3;1;2;0
WireConnection;25;0;24;0
WireConnection;25;1;34;0
WireConnection;25;2;24;2
WireConnection;20;0;22;0
WireConnection;20;1;1;0
WireConnection;20;2;21;0
WireConnection;37;1;23;0
WireConnection;13;0;3;4
WireConnection;35;0;23;0
WireConnection;35;1;25;0
WireConnection;35;2;36;2
WireConnection;4;0;20;0
WireConnection;4;1;3;0
WireConnection;38;0;3;4
WireConnection;38;1;37;0
WireConnection;11;0;4;0
WireConnection;11;3;14;0
WireConnection;11;4;14;0
WireConnection;11;10;38;0
WireConnection;11;11;35;0
ASEEND*/
//CHKSM=07297CB45BCDCEB059FBD55945D055DF543C15C3