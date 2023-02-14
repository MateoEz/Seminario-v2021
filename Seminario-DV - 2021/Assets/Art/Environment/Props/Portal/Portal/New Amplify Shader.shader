// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New Amplify Shader"
{
	Properties
	{
		_Opacity("Opacity", Float) = 10.94
		_Color0("Color 0", Color) = (0,0.9898968,1,0)
		_pow("pow", Float) = 1
		_Float2("Float 0", Float) = 1
		_rangeT("rangeT", Range( -100 , 100)) = -5.327737
		_Range1("Range", Range( -100 , 100)) = 2.834983
		_offsetForce1("offsetForce", Float) = 0
		_Reduce("Reduce", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _rangeT;
		uniform float _Range1;
		uniform float _offsetForce1;
		uniform float _Float2;
		uniform float _Reduce;
		uniform float4 _Color0;
		uniform float _pow;
		uniform float _Opacity;


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


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 transform32 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float gradient37 = saturate( ( ( transform32.y + _rangeT ) / _Range1 ) );
			float mulTime22 = _Time.y * _Float2;
			float2 panner23 = ( mulTime22 * float2( 0,-5 ) + float2( 0,0 ));
			float2 uv_TexCoord25 = v.texcoord.xy * float2( 55,55 ) + panner23;
			float simplePerlin2D26 = snoise( uv_TexCoord25 );
			simplePerlin2D26 = simplePerlin2D26*0.5 + 0.5;
			float Noise29 = ( simplePerlin2D26 + 0.4 );
			float3 offset40 = ( ( ( ase_vertex3Pos * gradient37 ) * _offsetForce1 ) * Noise29 );
			v.vertex.xyz += ( offset40 * _Reduce );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV12 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode12 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV12, _pow ) );
			float4 temp_output_14_0 = ( _Color0 * fresnelNode12 );
			o.Albedo = temp_output_14_0.rgb;
			o.Emission = temp_output_14_0.rgb;
			o.Alpha = ( fresnelNode12 * _Opacity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
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
				o.worldNormal = worldNormal;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
141;253;959;381;2458.028;424.8062;3.430897;True;False
Node;AmplifyShaderEditor.CommentaryNode;20;-2833.695,1076.137;Inherit;False;1480.678;536.5005;Comment;10;37;36;35;34;33;32;31;30;1;17;Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;19;-3106.863,131.5962;Inherit;False;1579.735;613.6957;Comment;9;29;28;27;26;25;24;23;22;21;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;30;-2783.695,1126.137;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;32;-2585.916,1193.106;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-3056.863,609.2161;Inherit;False;Property;_Float2;Float 0;3;0;Create;True;0;0;False;0;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2587.953,1368.609;Inherit;True;Property;_rangeT;rangeT;5;0;Create;True;0;0;False;0;-5.327737;100;-100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-2228.57,1424.358;Inherit;False;Property;_Range1;Range;6;0;Create;True;0;0;False;0;2.834983;41;-100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-2351.844,1173.566;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-2884.821,610.4623;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;35;-2017.169,1259.934;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;23;-2669.171,535.7485;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;24;-2891.491,275.1619;Inherit;False;Constant;_Tilling1;Tilling;1;0;Create;True;0;0;False;0;55,55;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SaturateNode;36;-1758.787,1450.196;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-2616.611,181.5961;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;26;-2359.41,216.2633;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2444.358,487.2916;Inherit;True;Constant;_Float3;Float 1;2;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1596.018,1216.738;Inherit;True;gradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-2124.544,416.7516;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;41;-1112.983,661.4647;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;44;-1061.254,920.1086;Inherit;False;37;gradient;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-1770.128,308.4591;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-824.3142,721.4867;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-504.5471,1038.732;Inherit;False;Property;_offsetForce1;offsetForce;7;0;Create;True;0;0;False;0;0;1.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-437.5095,678.0077;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-402.1894,864.0146;Inherit;False;29;Noise;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-143.7348,741.1847;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1110.993,88.17035;Inherit;False;Property;_pow;pow;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-320.8136,420.4309;Inherit;False;Property;_Reduce;Reduce;8;0;Create;True;0;0;False;0;0;0.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-312.196,565.1962;Inherit;False;offset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-738.1041,190.695;Inherit;False;Property;_Opacity;Opacity;0;0;Create;True;0;0;False;0;10.94;1.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;12;-746.9271,-21.57231;Inherit;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-625.1712,-257.9496;Inherit;False;Property;_Color0;Color 0;1;0;Create;True;0;0;False;0;0,0.9898968,1,0;0,0.9898968,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-406.4482,-103.2408;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-497.0547,127.2563;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;17;-1572.321,1165.302;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-102.218,371.1977;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-2070.413,1132.948;Inherit;False;Property;_OffsetTime;OffsetTime;4;0;Create;True;0;0;False;0;5.63;5.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;188.2282,40.35771;Float;False;True;2;ASEMaterialInspector;0;0;Standard;New Amplify Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;30;0
WireConnection;33;0;32;2
WireConnection;33;1;31;0
WireConnection;22;0;21;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;23;1;22;0
WireConnection;36;0;35;0
WireConnection;25;0;24;0
WireConnection;25;1;23;0
WireConnection;26;0;25;0
WireConnection;37;0;36;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;29;0;28;0
WireConnection;42;0;41;0
WireConnection;42;1;44;0
WireConnection;43;0;42;0
WireConnection;43;1;38;0
WireConnection;39;0;43;0
WireConnection;39;1;45;0
WireConnection;40;0;39;0
WireConnection;12;3;5;0
WireConnection;14;0;8;0
WireConnection;14;1;12;0
WireConnection;15;0;12;0
WireConnection;15;1;9;0
WireConnection;17;0;1;0
WireConnection;46;0;40;0
WireConnection;46;1;47;0
WireConnection;0;0;14;0
WireConnection;0;2;14;0
WireConnection;0;9;15;0
WireConnection;0;11;46;0
ASEEND*/
//CHKSM=0548C41D6A4C4D82A212C0A6C966B0C2844F6F3C