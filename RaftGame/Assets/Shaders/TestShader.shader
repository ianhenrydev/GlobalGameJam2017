
Shader "RaftGameShaders/WaterShader" {
	Properties{
		_FoamGradient("FoamGradient", 2D) = "white" {}
		_Depth("Depth", Float) = 0
		_FoamStrength("FoamStrength", Range(0, 10)) = 8.785265
		_Append("Append", Range(0, 300)) = 0
		_Foam("Foam", 2D) = "white" {}
		_Bump("Bump", 2D) = "bump" {}
		_FoamSpeed("FoamSpeed", Range(-5, 5)) = 0
		_BumpPower("BumpPower", Float) = 0
		_FoamSaturationPower("FoamSaturationPower", Float) = 0
		_FoamDepthPower("FoamDepthPower", Float) = 0
		_FoamColor("FoamColor", Color) = (0.5,0.5,0.5,1)
		_WaterColor("WaterColor", Color) = (0.5,0.5,0.5,1)
		_DepthColorPow("DepthColorPow", Float) = 0
		_OpacityIn("OpacityIn", Range(0, 1)) = 0
		_OpacityOut("OpacityOut", Range(0, 1)) = 0
		_ShorePower("ShorePower", Float) = 0
		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader
		{
		Tags
		{
		"IgnoreProjector" = "True"
		"Queue" = "Transparent"
		"RenderType" = "Transparent"
	}
		Pass{
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
		#pragma target 3.0

		uniform half _OpacityOut;

		uniform float _Depth;
		uniform float _FoamStrength;
		uniform float _FoamSaturationPower;
		uniform float _FoamDepthPower;
		uniform float _FoamSpeed;
		uniform float _DepthColorPow;
		uniform float _ShorePower;

		uniform float4 _TimeEditor; 
		uniform float4 _WaterColor;
		uniform float4 _FoamColor;

		uniform sampler2D _CameraDepthTexture;
		uniform sampler2D _Foam; uniform float4 _Foam_ST;
		uniform sampler2D _Bump; uniform float4 _Bump_ST;
		uniform sampler2D _FoamGradient; uniform float4 _FoamGradient_ST;
	
		uniform fixed _BumpPower;
		uniform fixed _OpacityIn;
		uniform fixed _Append;

	struct VertexInput
	{
		float4 vertex : POSITION;
		float2 texcoord0 : TEXCOORD0;
	};

	struct VertexOutput
	{
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float4 projPos : TEXCOORD1;
	};

	VertexOutput vert(VertexInput v)
	{
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;

		float4 objPos = mul(unity_ObjectToWorld, float4(0,0,0,1));

		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		o.projPos = ComputeScreenPos(o.pos);

		COMPUTE_EYEDEPTH(o.projPos.z);

		return o;
	}

	float4 frag(VertexOutput i) : COLOR
	{
		//Calculate depth by screen-space
		float4 objPos = mul(unity_ObjectToWorld, float4(0,0,0,1));
		float sceneZ = max(0,LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
		float partZ = max(0,i.projPos.z - _ProjectionParams.g);

		//Transform depth by arbitrary far/near clip
		fixed depthVal = saturate((sceneZ - partZ) / _Depth);
		float foamToCamDot = (1.0 - dot((pow((depthVal - objPos.b),_FoamSaturationPower) / _FoamStrength),float3(0.3,0.59,0.11)));

		//Handle time
		float4 waterTime = _Time + _TimeEditor;
		float waterDimensionalTime = (waterTime.r*0.5);

		//Unpack nomal uv
		float3 _Bump_var = UnpackNormal(tex2D(_Bump,TRANSFORM_TEX(i.uv0, _Bump)));

		//Map bump offset
		fixed2 waterBump = ((_Bump_var.rgb.rg*depthVal)*_BumpPower);
		fixed2 foamTransformedUV = (float2((foamToCamDot - waterDimensionalTime),_Append) + waterBump);

		//Map gradient
		half4 _FoamGradient_var = tex2D(_FoamGradient,TRANSFORM_TEX(foamTransformedUV, _FoamGradient));

		//Offset by time
		float waveTime = sin((waterDimensionalTime*_FoamSpeed));
		float2 wavePosTimeOffset = ((((0.1*_WorldSpaceCameraPos.r) + 0.5)*float2(waveTime,waveTime)) + waterBump);
		float4 _Foam_var = 1 - tex2D(_Foam,TRANSFORM_TEX(wavePosTimeOffset, _Foam));

		//Calculate final pixel color
		float3 emissive = (lerp((_WaterColor.rgb*(depthVal*_DepthColorPow)),_FoamColor.rgb,saturate((dot(_FoamGradient_var.rgb,float3(0.3,0.59,0.11))*foamToCamDot*(1.0 - (_Foam_var.rgb*(depthVal / _FoamDepthPower))))))*((1.1 - clamp(saturate((sceneZ - partZ) / _Depth),0.9,1))*(_ShorePower)));
		float3 finalColor = emissive;
		return fixed4(finalColor,(0.0 + ((depthVal - _OpacityIn) * (1.0 - 0.0)) / (_OpacityOut - _OpacityIn)));
	}
		ENDCG
	}
	}
}
