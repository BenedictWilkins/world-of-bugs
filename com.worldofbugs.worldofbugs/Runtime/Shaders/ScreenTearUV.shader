Shader "Bug/ScreenTearUV"
{

    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _TearTex("TearTexture", 2D) = "black" {}
    }

    SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {

                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				return o;
			}

            sampler2D _MainTex;
            sampler2D _TearTex;

			float4 frag (v2f i) : SV_Target {
                float4 offset = tex2D(_TearTex, i.uv);
                float u = fmod(offset[0] + i.uv.x, 1);
                float v = fmod(offset[1] + i.uv.y, 1);
                float2 uv = float2(u,v);
                return tex2D(_MainTex, uv);
            }
			ENDCG
		}
	}
}
