// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GrassTomi"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 15
		_TessMin( "Tess Min Distance", Float ) = 40
		_TessMax( "Tess Max Distance", Float ) = 40
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0
		_Tilling("Tilling", Float) = 0
		_ColorChange("ColorChange", Float) = 0
		_GrassHeight("GrassHeight", Float) = 0
		_UpColor("UpColor", Color) = (0,0,0,0)
		_EdgeIntensity("EdgeIntensity", Float) = 0
		_NoiseScale("NoiseScale", Range( 0 , 5)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_WindIntensity("WindIntensity", Float) = -0.01
		_WindMaskHeight("WindMaskHeight", Float) = 0.04
		_WindSpeed("WindSpeed", Vector) = (0,0,0,0)
		_DownColor("DownColor", Color) = (0,0,0,0)
		_Mask("Mask", Range( 0 , 1)) = 0
		_NoiseIntensity("NoiseIntensity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _WindMaskHeight;
		uniform sampler2D _TextureSample0;
		uniform float _Tilling;
		uniform float _GrassHeight;
		uniform float _NoiseScale;
		uniform float _NoiseIntensity;
		uniform float2 _WindSpeed;
		uniform float _WindIntensity;
		uniform float4 _DownColor;
		uniform float _ColorChange;
		uniform float4 _UpColor;
		uniform float _EdgeIntensity;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _Mask;
		uniform float _Cutoff = 0;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertex3Pos = v.vertex.xyz;
			float WindMask87 = ( saturate( ( ase_vertex3Pos.y + _WindMaskHeight ) ) * 1.0 );
			float smoothstepResult72 = smoothstep( 0.0 , 1.0 , WindMask87);
			float temp_output_74_0 = saturate( ( smoothstepResult72 * 3.0 ) );
			float2 temp_cast_0 = (_Tilling).xx;
			float2 uv_TexCoord3 = v.texcoord.xy * temp_cast_0;
			float4 temp_cast_1 = (0.89).xxxx;
			float4 temp_cast_2 = (1.33).xxxx;
			float4 temp_cast_3 = (_GrassHeight).xxxx;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult13 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float simplePerlin2D14 = snoise( appendResult13.xy*_NoiseScale );
			simplePerlin2D14 = simplePerlin2D14*0.5 + 0.5;
			float4 temp_cast_5 = (( 1.0 - saturate( ( simplePerlin2D14 * _NoiseIntensity ) ) )).xxxx;
			float3 ase_vertexNormal = v.normal.xyz;
			float4 Offset42 = ( ( pow( (float4( 0,0,0,0 ) + (tex2Dlod( _TextureSample0, float4( uv_TexCoord3, 0, 0.0) ) - float4( 0,0,0,0 )) * (temp_cast_2 - float4( 0,0,0,0 )) / (temp_cast_1 - float4( 0,0,0,0 ))) , temp_cast_3 ) - temp_cast_5 ) * float4( ase_vertexNormal , 0.0 ) );
			float4 OffsetUp79 = ( ( 1.0 - temp_output_74_0 ) * Offset42 );
			float4 appendResult66 = (float4(ase_vertex3Pos.x , 0.0 , ase_vertex3Pos.z , 0.0));
			float4 appendResult57 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float2 panner58 = ( 1.0 * _Time.y * _WindSpeed + appendResult57.xy);
			float simplePerlin2D59 = snoise( panner58*3.11 );
			simplePerlin2D59 = simplePerlin2D59*0.5 + 0.5;
			float4 WindOffset70 = ( ( appendResult66 * saturate( ( ( simplePerlin2D59 - 0.5 ) + _WindIntensity ) ) ) * WindMask87 );
			float4 OffsetDown80 = ( temp_output_74_0 * Offset42 );
			v.vertex.xyz += ( ( OffsetUp79 + WindOffset70 ) + OffsetDown80 ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float temp_output_29_0 = saturate( ( ase_vertex3Pos.y - _ColorChange ) );
			float4 Albedo38 = ( saturate( ( _DownColor * ( 1.0 - temp_output_29_0 ) ) ) + saturate( ( temp_output_29_0 * _UpColor ) ) );
			o.Albedo = Albedo38.rgb;
			o.Alpha = 1;
			float3 objToWorld41 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float3 ase_worldPos = i.worldPos;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float OpacityMask53 = ( ( 1.0 - ( distance( objToWorld41 , ase_worldPos ) * _EdgeIntensity * tex2D( _TextureSample1, uv_TextureSample1 ).r ) ) * _Mask );
			clip( OpacityMask53 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17200
7;703;1906;275;8004.645;-5433.882;1.454434;True;False
Node;AmplifyShaderEditor.CommentaryNode;23;-1545.556,693.6807;Inherit;False;1714.779;750.3966;Comment;20;1;3;4;6;8;7;9;10;11;20;21;19;18;16;14;13;15;17;12;42;GrassOffset;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;12;-1472.674,1109.913;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;83;18.1514,2385.07;Inherit;False;Property;_WindMaskHeight;WindMaskHeight;15;0;Create;True;0;0;False;0;0.04;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;81;-12.00724,2202.256;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-1343.45,1267.62;Inherit;False;Property;_NoiseScale;NoiseScale;12;0;Create;True;0;0;False;0;0;1.04;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-1249.813,1106.189;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1495.556,805.3604;Inherit;False;Property;_Tilling;Tilling;7;0;Create;True;0;0;False;0;0;3.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;244.3854,2326.303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;86;383.4286,2326.063;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;56;-1116.817,1777.75;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1309.522,775.4261;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1087.365,1329.077;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;19;0;Create;True;0;0;False;0;0;0.517;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;14;-1076.683,1177.324;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;359.8756,2450.805;Inherit;False;Constant;_WindMaskIntensity;WindMaskIntensity;15;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;57;-901.222,1786.108;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;579.6352,2303.093;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-987.4355,951.9417;Inherit;False;Constant;_MaxOld;MaxOld;4;0;Create;True;0;0;False;0;0.89;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;93;-973.6036,2030.38;Inherit;False;Property;_WindSpeed;WindSpeed;16;0;Create;True;0;0;False;0;0,0;0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;7;-974.6376,1049.47;Inherit;False;Constant;_MaxNew;MaxNew;4;0;Create;True;0;0;False;0;1.33;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-812.1026,1154.966;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1066.872,743.6806;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;-1;None;c21f723a38161ae4e9575b82841ce12b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;6;-761.4213,785.2184;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;58;-688.4221,1789.308;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;18;-661.3163,1116.237;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;87;743.1615,2377.245;Inherit;False;WindMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-722.4647,958.6296;Inherit;False;Property;_GrassHeight;GrassHeight;9;0;Create;True;0;0;False;0;0;1.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;-497.5966,1031.855;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;71;-572.3596,2891.537;Inherit;False;87;WindMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;9;-546.5632,801.4371;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;40;-1492.82,-884.0063;Inherit;False;1717.672;656.9491;Comment;13;28;26;27;29;31;36;34;35;30;32;37;33;38;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;59;-454.6613,1768.006;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3.11;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;21;-236.8878,1012.937;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-360.0022,2079.944;Inherit;False;Property;_WindIntensity;WindIntensity;14;0;Create;True;0;0;False;0;-0.01;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1328.223,-451.9336;Inherit;False;Property;_ColorChange;ColorChange;8;0;Create;True;0;0;False;0;0;-0.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;-374.4285,855.6904;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;72;-314.7231,2871.181;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;60;-199.5307,1790.724;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;26;-1339.041,-605.3202;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;65;-65.83427,1593.214;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-51.31826,2867.424;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-217.8932,852.5734;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-63.20652,1897.705;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;55;-1414.697,-116.3159;Inherit;False;1420.601;524.1589;Comment;10;46;41;48;49;47;45;50;52;51;53;OpacityMask;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-1124.689,-556.6752;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;41;-1364.697,-66.31587;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;29;-966.5628,-554.8562;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;64;180.0377,1794.714;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;74;110.169,2868.77;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;66;168.6889,1599.612;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldPosInputsNode;46;-1323.766,98.15509;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-36.82982,778.692;Inherit;False;Offset;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;373.3129,2866.582;Inherit;False;42;Offset;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;34;-972.0441,-627.3068;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-808.8591,-467.6329;Inherit;False;Property;_UpColor;UpColor;10;0;Create;True;0;0;False;0;0,0,0,0;0.9215686,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;49;-1132.884,177.843;Inherit;True;Property;_TextureSample1;Texture Sample 1;13;0;Create;True;0;0;False;0;-1;None;5670620004339484c8a7a6003c6f117d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;69;565.0532,1707.851;Inherit;False;87;WindMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;357.4958,1665.421;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;36;-833.2604,-786.7982;Inherit;False;Property;_DownColor;DownColor;17;0;Create;True;0;0;False;0;0,0,0,0;1,0.8230088,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;75;391.4346,2773.542;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1092.127,98.8453;Inherit;False;Property;_EdgeIntensity;EdgeIntensity;11;0;Create;True;0;0;False;0;0;-8.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;45;-1078.606,4.297806;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-821.431,63.66008;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-598.69,-536.5898;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-554.4196,-652.6317;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;771.5849,1598.007;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;592.3187,2771.179;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-694.155,178.6862;Inherit;False;Property;_Mask;Mask;18;0;Create;True;0;0;False;0;0;0.762;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;50;-662.7706,75.37648;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;1036.138,1605.431;Inherit;False;WindOffset;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;785.3157,2804.503;Inherit;False;OffsetUp;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;587.5918,2957.882;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;37;-393.4872,-634.2318;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;32;-413.8182,-541.387;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;90;95.48991,85.11398;Inherit;False;79;OffsetUp;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;92.77702,164.1563;Inherit;False;70;WindOffset;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-416.408,82.00084;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;771.797,2955.143;Inherit;False;OffsetDown;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-221.9448,-558.5576;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;335.9318,122.2732;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;38;-18.14819,-516.6605;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;185.4204,275.6598;Inherit;False;80;OffsetDown;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;-237.096,75.52701;Inherit;False;OpacityMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;395.9084,3.208306;Inherit;False;53;OpacityMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;92;448.7367,209.1224;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;426.9574,-199.9653;Inherit;False;38;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;623.5377,-201.4486;Float;False;True;6;ASEMaterialInspector;0;0;Standard;GrassTomi;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0;True;True;0;False;TransparentCutout;;AlphaTest;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;15;40;40;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;6;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;12;1
WireConnection;13;1;12;3
WireConnection;82;0;81;2
WireConnection;82;1;83;0
WireConnection;86;0;82;0
WireConnection;3;0;4;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;57;0;56;1
WireConnection;57;1;56;3
WireConnection;84;0;86;0
WireConnection;84;1;85;0
WireConnection;16;0;14;0
WireConnection;16;1;17;0
WireConnection;1;1;3;0
WireConnection;6;0;1;0
WireConnection;6;2;8;0
WireConnection;6;4;7;0
WireConnection;58;0;57;0
WireConnection;58;2;93;0
WireConnection;18;0;16;0
WireConnection;87;0;84;0
WireConnection;19;0;18;0
WireConnection;9;0;6;0
WireConnection;9;1;10;0
WireConnection;59;0;58;0
WireConnection;11;0;9;0
WireConnection;11;1;19;0
WireConnection;72;0;71;0
WireConnection;60;0;59;0
WireConnection;73;0;72;0
WireConnection;20;0;11;0
WireConnection;20;1;21;0
WireConnection;63;0;60;0
WireConnection;63;1;61;0
WireConnection;27;0;26;2
WireConnection;27;1;28;0
WireConnection;29;0;27;0
WireConnection;64;0;63;0
WireConnection;74;0;73;0
WireConnection;66;0;65;1
WireConnection;66;2;65;3
WireConnection;42;0;20;0
WireConnection;34;0;29;0
WireConnection;67;0;66;0
WireConnection;67;1;64;0
WireConnection;75;0;74;0
WireConnection;45;0;41;0
WireConnection;45;1;46;0
WireConnection;47;0;45;0
WireConnection;47;1;48;0
WireConnection;47;2;49;1
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;35;0;36;0
WireConnection;35;1;34;0
WireConnection;68;0;67;0
WireConnection;68;1;69;0
WireConnection;76;0;75;0
WireConnection;76;1;78;0
WireConnection;50;0;47;0
WireConnection;70;0;68;0
WireConnection;79;0;76;0
WireConnection;77;0;74;0
WireConnection;77;1;78;0
WireConnection;37;0;35;0
WireConnection;32;0;30;0
WireConnection;51;0;50;0
WireConnection;51;1;52;0
WireConnection;80;0;77;0
WireConnection;33;0;37;0
WireConnection;33;1;32;0
WireConnection;91;0;90;0
WireConnection;91;1;89;0
WireConnection;38;0;33;0
WireConnection;53;0;51;0
WireConnection;92;0;91;0
WireConnection;92;1;88;0
WireConnection;0;0;39;0
WireConnection;0;10;54;0
WireConnection;0;11;92;0
ASEEND*/
//CHKSM=87918B72DD82B9AD5389D594F6C3EB611FA4695D