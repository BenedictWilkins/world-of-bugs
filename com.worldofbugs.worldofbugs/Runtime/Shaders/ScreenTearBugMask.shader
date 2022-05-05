Shader "Bug/ScreenTearBugMask"
{

    Properties {

        _BugType("BugType", Color) = (1,1,1,1)

        _MainTex("Texture", 2D) = "white" {}
        _TearTex("TearTexture", 2D) = "white" {}
        _CameraTex("CameraTexture", 2D) = "red" {}

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
            sampler2D _CameraTex;

            float _TearMin;
            float _TearMax;
            float4 _BugType;

			float4 frag (v2f i) : SV_Target {
                float4 col1 = tex2D(_TearTex, i.uv);
                float4 col2 = tex2D(_CameraTex, i.uv);
                float4 col3 = tex2D(_MainTex, i.uv);
                
                float tmax = step(i.uv.y, _TearMax); // <= 
                float tmin = step(_TearMin, i.uv.y); // <= 
                
                
                // compare the current and previous frames from the main camera
                float4 c = abs(col1 - col2);
                float s = c[0] + c[1] + c[2];

                float v = tmax * tmin * (1 - step(s, 0.2));

                //return lerp(col3, col1, 0.5);

                return lerp(col3, _BugType, v);

                //return lerp(col1, col3, v);
            }
			ENDCG
		}

	}
   
}

