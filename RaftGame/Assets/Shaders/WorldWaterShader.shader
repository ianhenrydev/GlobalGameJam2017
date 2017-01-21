Shader "Unlit/WorldWaterShader"
{
	Properties
	{
		_WaterColor ("Water Color", COLOR) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 depth : TEXCOORD0;
			};

			//Declare shader properties as variables
			float4 _WaterColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_DEPTH(o.depth);

				//Transfer data
				UNITY_TRANSFER_FOG(o,o.vertex);
				UNITY_TRANSFER_DEPTH(o.depth);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				//Get depth
				UNITY_OUTPUT_DEPTH(i.depth);

				return _WaterColor;
			}
			ENDCG
		}
	}
}
