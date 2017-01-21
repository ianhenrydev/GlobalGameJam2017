/*Shader "Unlit/WorldWaterShader"
{
	Properties
	{
		_WaterColor ("Water Color", COLOR) = (1,1,1,1)
		_FoamColor ("Foam Color", COLOR) = (1,1,1,1)

		_Foam("Foam", 2D) = "white" {}
		_FoamGradient("Foam Gradient", 2D) = "white" {}
		_WaterNormal("WaterNormal", 2D) = "bump" {}

		_Depth("Depth", float) = 1.0
		_DepthDeepPower("Deep Depth", float) = 1.0
		_FoamDepthPower("FoamDepthPower", Float) = 0

		_FoamSaturationPower("FoamSaturationPower", Float) = 0

		_OpacityIn("OpacityIn", Range(0, 1)) = 0
		_OpacityOut("OpacityOut", Range(0, 1)) = 0

		_ShorePower("ShorePower", float) = 0.0
		_BumpPower("Water Normal Power", float) = 0.0
		_FoamSpeed("Foam Speed", float) = 1.0
		_FoamStrength("Foam Strength", float ) = 1.0

		_Append("Append", Range(0, 300)) = 0
	}
	SubShader
	{
			Tags{
			"IgnoreProjector" = "True"
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		LOD 100
		
			Pass
			{
				Name "FORWARD"
				Tags{
				"LightMode" = "ForwardBase"
			}

			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			#pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 projPos : TEXCOORD1;
			};

			uniform sampler2D _CameraDepthTexture;
			uniform sampler2D _Foam; uniform float4 _Foam_ST;
			uniform sampler2D _FoamGradient; uniform float4 _FoamGradient_ST;
			uniform sampler2D _Bump; uniform float4 _Bump_ST;

			uniform float _OpacityIn;
			uniform float _OpacityOut;
			uniform float _FoamDepthPower;
			uniform float _FoamStrength;
			uniform float _FoamSaturationPower;

			uniform fixed _Append;

			uniform float _ShorePower;
			uniform float4 _TimeEditor;

			float4 _WaterColor;
			float4 _FoamColor;
			float _Depth;
			float  _DepthDeepPower;
			float _FoamSpeed;
			float _BumpPower;
			

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float4 objPos = mul(unity_ObjectToWorld, float4(0,0,0,1));
				float sceneZ = max(0,LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
				float partZ = max(0,i.projPos.z - _ProjectionParams.g);
				fixed sampleDepth = saturate((sceneZ - partZ) / _Depth);

				//Textures / normals
				float3 _Bump_var = UnpackNormal(tex2D(_Bump, TRANSFORM_TEX(i.uv0, _Bump)));

				//Time
				float4 editorOffsetTime = _Time + _TimeEditor;
				float modTime = (editorOffsetTime.r*1.5);
				float foamTime = sin((modTime*_FoamSpeed));

				//Camera
				fixed2 bumpSample = ((_Bump_var.rgb.rg*sampleDepth)*_BumpPower);
				float2 worldCameraTime = ((((0.1*_WorldSpaceCameraPos.r) + 0.5)*float2(foamTime, foamTime)) + bumpSample);

				float depthClamp = 1.0 - clamp(saturate((sceneZ - partZ) / _Depth), 0.9, 1);
				float3 waterCol = (_WaterColor.rgb * ((sampleDepth) * _DepthDeepPower));
				float3 foamCol = _FoamColor.rgb;

				float4 _Foam_var = tex2D(_Foam, TRANSFORM_TEX(worldCameraTime, _Foam));
				float foamDot = (1.0 - dot((pow((sampleDepth - objPos.b), _FoamSaturationPower) / _FoamStrength), float3(0.3, 0.59, 0.11)));

				float2 node_7609 = ((_Bump_var.rgb.rg*sampleDepth)*_BumpPower);
				float2 node_4561 = (float2((foamDot - modTime), _Append) + node_7609);
				half4 _FoamGradient_var = tex2D(_FoamGradient, TRANSFORM_TEX(node_4561, _FoamGradient));

				float depthSaturation = saturate((dot(
					_FoamGradient_var.rgb,
					float3(0.3, 0.59, 0.11))*foamDot*(1.0 - (_Foam_var.rgb*(sampleDepth / _FoamDepthPower))))*((1.0 - clamp(saturate((sceneZ - partZ) / _Depth), 0.9, 1))*_ShorePower));

				float3 emissive = (lerp(
					(_WaterColor.rgb*(sampleDepth*_DepthDeepPower)),
					_FoamColor.rgb,
					depthSaturation));

				return fixed4(emissive, (0.0 + ((sampleDepth - _OpacityIn) * (1.0 - 0.0)) / (_OpacityOut - _OpacityIn)));
			}

			ENDCG
		}
	}
}
*/


Shader "Unlit/RawShootyWater"
{
	Properties
	{
		_SurfaceTexture("Water Tex", 2D) = "white" {}
	_FoamGradient("Foam Gradient", 2D) = "white" {}
	_FoamTexture("Foam Texture", 2D) = "white" {}
	_BumpTexture("Bump", 2D) = "bump" {}
	_Depth("Depth", Range(0.0, 50.0)) = 1.0
		_WaterColor("Water Color", Color) = (0, 0, 0)
		_FoamPower("Foam Power", Range(0.0, 20.0)) = 1.0
		_WaveCount("Wave Count", Range(1.0, 5.0)) = 1.0
		_SampleTime("Sample Time", Range(0, 20.0)) = 1.0
		_FoamSpeed("Foam Speed", Float) = 0
		_BumpPower("Bump Power", Float) = 0
	}
		SubShader
	{
		Tags
	{
		"RenderType" = "Opaque"
		"IgnoreProjector" = "True"
		"Queue" = "Transparent"
	}

		Pass
	{
		ZWrite Off
		ColorMask RGB

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
#pragma multi_compile_fwdbase
#define UNITY_PASS_FORWARDBASE
#include "UnityCG.cginc"

		//Texture Samplers
		uniform sampler2D _CameraDepthTexture;
	uniform sampler2D _FoamGradient;
	uniform sampler2D _FoamTexture;
	uniform sampler2D _SurfaceTexture;
	uniform sampler2D _BumpTexture;

	//Texture Buffers
	uniform float4 _SurfaceTexture_ST;
	uniform float4 _FoamGradient_ST;
	uniform float4 _FoamTexture_ST;
	uniform float4 _BumpTexture_ST;

	//Time stuff
	uniform float4 _TimeEditor;
	uniform half2 _SampleTime;

	//Colors
	uniform half4 _WaterColor;
	uniform half4 _SeaColor;

	//Values
	uniform half _Depth;
	uniform half _WaveCount;
	uniform half _FoamPower;
	uniform half _FoamSpeed;
	uniform half _FoamTextureSize;
	uniform half _BumpPower;

	struct appdata
	{
		fixed4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		fixed4 pos : SV_POSITION;
		float4 uvcoord	: TEXCOORD0;
		float4 projPos	: TEXCOORD1;
		float2 uv		: TEXCOORD2;
	};

	v2f vert(appdata v)
	{
		v2f o;

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.projPos = ComputeScreenPos(o.pos);
		COMPUTE_EYEDEPTH(o.projPos.z);
		o.uvcoord = UNITY_PROJ_COORD(o.projPos);
		o.uv = TRANSFORM_TEX(v.uv, _SurfaceTexture);

		return o;
	}

	fixed3 frag(v2f i) : SV_Target
	{
		//Get Camera depth
		half sceneZ = max(0, LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, i.uvcoord))) - _ProjectionParams.g);

	//Get view projection
	half partZ = max(0, i.projPos.z - _ProjectionParams.g);

	//Get depth
	half objDepth = (sceneZ - partZ) / _Depth;

	//Get current world time
	half4 waveTime = _Time + _TimeEditor;

	//BUMP
	half3 bumpOffset = UnpackNormal(tex2D(_BumpTexture, TRANSFORM_TEX(i.uv, _BumpTexture)));

	//Foam Gradient Texture
	half2 foamDivision = (float2((((0.0 - (objDepth*_WaveCount))*_WaveCount) - (waveTime.r.r * _FoamSpeed)), 0.0) + i.uv);
	half4 _FoamGradient_var = tex2D(_FoamGradient, TRANSFORM_TEX(foamDivision, _FoamGradient));

	//Foam texture
	half2 foamUV = (i.uv + (_FoamTextureSize*i.uvcoord));
	half4 _FoamTexture_var = tex2D(_FoamTexture, TRANSFORM_TEX(foamUV, _FoamTexture));

	//Get surface color
	half2 surfaceUV1 = (i.uv + (_Time.rg * 0.01));
	half4 surfaceCol = tex2D(_SurfaceTexture, TRANSFORM_TEX(surfaceUV1, _SurfaceTexture));

	half2 surfaceUV2 = (i.uv + (fixed2(_Time.r * 5, _Time.g * -0.01)));
	surfaceCol += tex2D(_SurfaceTexture, TRANSFORM_TEX(surfaceUV2 - (surfaceUV1 * 2), _SurfaceTexture));

	//Lerp surface colors
	half4 finalCol = saturate(clamp(lerp(0, (surfaceCol / 2), objDepth), 0, 1)) * 5;

	//Produce final emission
	half3 emissive = saturate((1.0 - (1.0 - _SeaColor.rgb)*(1.0 - ((_FoamGradient_var.rgb*(1.0 - objDepth)) *
		(_FoamTexture_var.rgb*_FoamPower)))));

	return half4(saturate(emissive + _WaterColor), 1) + clamp(finalCol * _BumpPower, 0, 1);
	}
		ENDCG
	}
	}
}