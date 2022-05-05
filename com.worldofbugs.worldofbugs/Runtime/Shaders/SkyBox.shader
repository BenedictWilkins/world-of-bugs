// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Bug/SkyBox"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Clip("Clip", Float) = 0.01
	}

	SubShader {
		Tags
		{
			"RenderType"="Opaque"
		}

		ZWrite On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float depth : DEPTH;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			half4 _Color;
			float _Clip;

			fixed4 frag (v2f i) : SV_Target {
				float c = max(i.vertex.y * _Clip, 0.1);
				
				return fixed4(c, c, c, 1);
			}
			ENDCG
		}
	}
}
