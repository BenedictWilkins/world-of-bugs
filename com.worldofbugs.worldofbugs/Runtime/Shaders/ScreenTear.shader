Shader "Bug/ScreenTear"
{

    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _TearTex("TearTexture", 2D) = "white" {}
        _TearMin("TearMin", Float) = 0.1
        _TearMax("TearMax", Float) = 0.5
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

            sampler2D _TearTex;
			sampler2D _MainTex;
            float _TearMin;
            float _TearMax;


			float4 frag (v2f i) : SV_Target {
                float4 col1 = tex2D(_TearTex, i.uv);
                float4 col2 = tex2D(_MainTex, i.uv);
                // check if in region of a screen tear
                float tmax = step(i.uv.y, _TearMax); // <=
                float tmin = step(_TearMin, i.uv.y); // <=
                float v = tmax * tmin; // 1 its between the tear bounds, 0 otherwise
                return lerp(col2, col1, v);
                //return col2;
            }
			ENDCG
		}

	}

}
