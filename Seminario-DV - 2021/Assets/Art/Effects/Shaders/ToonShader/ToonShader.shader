// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonShader"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0.002
		_ASEOutlineColor( "Outline Color", Color ) = (0.1792453,0.1792453,0.1792453,0)
		_ToonRamp("ToonRamp", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Albedo("Albedo", 2D) = "white" {}
		_Tint("Tint", Color) = (0.509434,0.509434,0.509434,0)
		_RimOffset("RimOffset", Float) = 0
		_Power("Power", Range( 0 , 1)) = 0
		_Rim("Rim", Color) = (0.261659,0.6583914,0.7924528,0)
		_SpecSomething("SpecSomething", Range( 0 , 1)) = 0
		_IntesitySpec("IntesitySpec", Range( 0 , 1)) = 0
		_Specular("Specular", 2D) = "white" {}
		_SpecularColor("SpecularColor", Color) = (0,0,0,0)
		_Movement("Movement", Vector) = (0,0,0,0)
		_Emission("Emission", Float) = 0
		[HDR]_Emmisive("Emmisive", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float3 _Movement;
		uniform float4 _Emmisive;
		uniform float _Emission;
		uniform sampler2D _ToonRamp;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _Tint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _RimOffset;
		uniform float _Power;
		uniform float4 _Rim;
		uniform float _SpecSomething;
		uniform sampler2D _Specular;
		uniform float4 _Specular_ST;
		uniform float4 _SpecularColor;
		uniform float _IntesitySpec;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( sin( _Time.y ) * _Movement );
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float4 Normal20 = tex2D( _Normal, uv_Normal );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult2 = dot( normalize( (WorldNormalVector( i , Normal20.rgb )) ) , ase_worldlightDir );
			float NormalLightDir7 = dotResult2;
			float2 temp_cast_2 = ((NormalLightDir7*0.5 + 0.5)).xx;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 Albedo27 = ( _Tint * tex2D( _Albedo, uv_Albedo ) );
			float4 Shadow13 = ( tex2D( _ToonRamp, temp_cast_2 ) * Albedo27 );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi35 = gi;
			float3 diffNorm35 = WorldNormalVector( i , Normal20.rgb );
			gi35 = UnityGI_Base( data, 1, diffNorm35 );
			float3 indirectDiffuse35 = gi35.indirect.diffuse + diffNorm35 * 0.0001;
			float4 Lightning34 = ( Shadow13 * ( ase_lightColor * float4( ( indirectDiffuse35 + ase_lightAtten ) , 0.0 ) ) );
			float3 ase_worldViewDir = Unity_SafeNormalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult5 = dot( normalize( (WorldNormalVector( i , Normal20.rgb )) ) , ase_worldViewDir );
			float NormalViewDir8 = dotResult5;
			float4 Rim47 = ( saturate( ( pow( ( 1.0 - saturate( ( NormalViewDir8 + _RimOffset ) ) ) , _Power ) * ( NormalLightDir7 * ase_lightAtten ) ) ) * ( ase_lightColor * _Rim ) );
			float dotResult67 = dot( ( ase_worldViewDir + _WorldSpaceLightPos0.xyz ) , normalize( (WorldNormalVector( i , Normal20.rgb )) ) );
			float smoothstepResult70 = smoothstep( 1.1 , 1.2 , pow( dotResult67 , _SpecSomething ));
			float2 uv_Specular = i.uv_texcoord * _Specular_ST.xy + _Specular_ST.zw;
			float4 lerpResult85 = lerp( _SpecularColor , ase_lightColor , 0.4);
			float4 Specular77 = ( ase_lightAtten * ( ( smoothstepResult70 * ( tex2D( _Specular, uv_Specular ) * lerpResult85 ) ) * _IntesitySpec ) );
			c.rgb = ( ( Lightning34 + Rim47 ) + Specular77 ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			o.Emission = ( _Emmisive * _Emission ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
0;368.6667;1003;273;7769.036;879.6647;8.625478;True;False
Node;AmplifyShaderEditor.SamplerNode;18;-5237.716,-149.4802;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;False;0;-1;None;8aa92ae3cc5e4b749b94cda82db3df36;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-4845.922,-120.935;Inherit;False;Normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;10;-3578.385,812.5037;Inherit;False;799.8665;435.346;Comment;4;4;6;5;8;ViewDir;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-3798.433,882.4442;Inherit;False;20;Normal;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;9;-3579.714,17.03507;Inherit;False;792.505;443.1299;Comment;4;1;3;2;7;NormalDotLight;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-3872.282,74.65821;Inherit;False;20;Normal;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;6;-3493.228,1063.85;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;4;-3528.385,862.5037;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-3526.296,281.165;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;5;-3221.298,948.3527;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-3529.714,67.03508;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;2;-3263.456,150.9458;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;59;-2430.45,613.1351;Inherit;False;1978.655;646.5793;Rim;17;40;42;41;43;46;44;45;47;53;58;54;56;57;51;49;52;61;RimLight;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;30;-3518.913,-1236.685;Inherit;False;886.915;482.0967;;4;25;23;26;27;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;-3027.85,939.0256;Inherit;False;NormalViewDir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;87;-2582.736,1843.004;Inherit;False;2735.472;1246.825;Comment;23;66;63;62;64;65;67;83;69;82;84;72;71;68;85;80;86;70;79;74;76;73;75;77;Spec;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;7;-3037.876,141.1413;Inherit;False;NormalLightDir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-3411.058,-1186.685;Inherit;False;Property;_Tint;Tint;3;0;Create;True;0;0;False;0;0.509434,0.509434,0.509434,0;0.7254534,0.6156842,0.3076531,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-2328.498,759.0465;Inherit;False;Property;_RimOffset;RimOffset;4;0;Create;True;0;0;False;0;0;3.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-3468.913,-984.5875;Inherit;True;Property;_Albedo;Albedo;2;0;Create;True;0;0;False;0;-1;None;52c389326e641ab4c9ae72a8f7c50c5d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;17;-3543.948,-612.1075;Inherit;False;1398.458;495.1684;Comment;7;13;12;15;16;11;28;29;sHADOW;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-2380.45,665.134;Inherit;False;8;NormalViewDir;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-2078.731,681.1191;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;63;-2532.736,2079.61;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;16;-3488.118,-413.8755;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0.5;0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-3493.948,-551.3129;Inherit;False;7;NormalLightDir;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-3100.089,-1069.955;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-2438.168,2222.43;Inherit;False;20;Normal;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;62;-2463.427,1893.004;Inherit;False;World;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-2880.665,-1053.9;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;65;-2190.894,2189.46;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-2174.409,2008.126;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;43;-1914.884,681.119;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;15;-3201.236,-515.1179;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1368.396,2974.828;Inherit;False;Constant;_SpecTransition;SpecTransition;11;0;Create;True;0;0;False;0;0.4;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;44;-1753.035,681.1188;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;83;-1381.631,2828.624;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;46;-1931.41,784.0695;Inherit;False;Property;_Power;Power;5;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;67;-1921.64,2129.016;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1940.872,2277.38;Inherit;False;Property;_SpecSomething;SpecSomething;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-2273.969,50.20609;Inherit;False;20;Normal;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-2699.604,-291.9583;Inherit;False;27;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;82;-1408.813,2607.763;Inherit;False;Property;_SpecularColor;SpecularColor;10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;12;-2896.886,-546.3703;Inherit;True;Property;_ToonRamp;ToonRamp;0;0;Create;True;0;0;False;0;-1;15c1734c45b753b41b1082218685bbf0;15c1734c45b753b41b1082218685bbf0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;54;-1894.768,929.6926;Inherit;False;7;NormalLightDir;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;56;-1905.409,1051.344;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1602.715,982.3757;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;68;-1690.851,2161.986;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;45;-1555.219,663.1351;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-1649.638,2271.885;Inherit;False;Constant;_Min;Min;9;0;Create;True;0;0;False;0;1.1;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;80;-1444.179,2398.61;Inherit;True;Property;_Specular;Specular;9;0;Create;True;0;0;False;0;-1;None;6d02fa5ee9c82804690d424b0c5de4b6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IndirectDiffuseLighting;35;-2011.98,59.82031;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;36;-2009.093,148.7148;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;85;-1119.662,2824.153;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1641.395,2373.542;Inherit;False;Constant;_Max;Max;10;0;Create;True;0;0;False;0;1.2;1.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2351.781,-440.886;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;70;-1399.616,2186.713;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-2164.056,-583.855;Inherit;False;Shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;52;-1413.765,1052.714;Inherit;False;Property;_Rim;Rim;6;0;Create;True;0;0;False;0;0.261659,0.6583914,0.7924528,0;0.261659,0.6583914,0.7924528,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LightColorNode;49;-1381.314,860.1387;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1356.241,676.1696;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1711.535,81.4523;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-1113.793,2500.83;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;32;-1875.353,-120.0445;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-1127.506,2205.495;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;61;-1140.067,697.98;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1628.284,-45.49884;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-846.314,2336.591;Inherit;False;Property;_IntesitySpec;IntesitySpec;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-1809.019,-242.7821;Inherit;False;13;Shadow;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1128.423,811.8915;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1377.288,-180.7923;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-892.7556,676.1505;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-562.0128,2176.336;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;76;-475.1214,2093.741;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-700.4617,684.8925;Inherit;False;Rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;34;-1188.68,-148.5515;Inherit;False;Lightning;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-279.1212,2196.389;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;90;-339.7437,553.0898;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;-95.93083,2204.442;Inherit;False;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-617.4568,250.7967;Inherit;False;47;Rim;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;-619.1003,96.77713;Inherit;False;34;Lightning;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;-430.3105,390.7839;Inherit;False;77;Specular;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-83.26042,44.28005;Inherit;False;Property;_Emission;Emission;12;0;Create;True;0;0;False;0;0;1.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-363.2086,160.6391;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;89;-143.1415,453.325;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;96;-201.759,-199.5194;Inherit;False;Property;_Emmisive;Emmisive;13;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,1.105582,1.515717,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;93;168.7395,582.749;Inherit;False;Property;_Movement;Movement;11;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;140.8325,-42.61133;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;310.121,385.8462;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-181.6625,235.9169;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;491.0865,63.30357;Float;False;True;2;ASEMaterialInspector;0;0;CustomLighting;ToonShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.002;0.1792453,0.1792453,0.1792453,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;18;0
WireConnection;4;0;22;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;1;0;21;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;8;0;5;0
WireConnection;7;0;2;0
WireConnection;41;0;40;0
WireConnection;41;1;42;0
WireConnection;26;0;25;0
WireConnection;26;1;23;0
WireConnection;27;0;26;0
WireConnection;65;0;66;0
WireConnection;64;0;62;0
WireConnection;64;1;63;1
WireConnection;43;0;41;0
WireConnection;15;0;11;0
WireConnection;15;1;16;0
WireConnection;15;2;16;0
WireConnection;44;0;43;0
WireConnection;67;0;64;0
WireConnection;67;1;65;0
WireConnection;12;1;15;0
WireConnection;57;0;54;0
WireConnection;57;1;56;0
WireConnection;68;0;67;0
WireConnection;68;1;69;0
WireConnection;45;0;44;0
WireConnection;45;1;46;0
WireConnection;35;0;38;0
WireConnection;85;0;82;0
WireConnection;85;1;83;0
WireConnection;85;2;84;0
WireConnection;28;0;12;0
WireConnection;28;1;29;0
WireConnection;70;0;68;0
WireConnection;70;1;71;0
WireConnection;70;2;72;0
WireConnection;13;0;28;0
WireConnection;58;0;45;0
WireConnection;58;1;57;0
WireConnection;37;0;35;0
WireConnection;37;1;36;0
WireConnection;86;0;80;0
WireConnection;86;1;85;0
WireConnection;79;0;70;0
WireConnection;79;1;86;0
WireConnection;61;0;58;0
WireConnection;39;0;32;0
WireConnection;39;1;37;0
WireConnection;51;0;49;0
WireConnection;51;1;52;0
WireConnection;33;0;31;0
WireConnection;33;1;39;0
WireConnection;53;0;61;0
WireConnection;53;1;51;0
WireConnection;73;0;79;0
WireConnection;73;1;74;0
WireConnection;47;0;53;0
WireConnection;34;0;33;0
WireConnection;75;0;76;0
WireConnection;75;1;73;0
WireConnection;77;0;75;0
WireConnection;60;0;14;0
WireConnection;60;1;48;0
WireConnection;89;0;90;0
WireConnection;95;0;96;0
WireConnection;95;1;94;0
WireConnection;91;0;89;0
WireConnection;91;1;93;0
WireConnection;81;0;60;0
WireConnection;81;1;78;0
WireConnection;0;2;95;0
WireConnection;0;13;81;0
WireConnection;0;11;91;0
ASEEND*/
//CHKSM=9775A716D0B0D4E3A0FEBEA097FC8713EE3FE8DB