// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mask/CameraClip"
{
	Properties {
		_BugType("BugType", Color) = (1,1,1,1)
		_CameraClipColor("CameraClipColor", Color) = (1,1,1,1)
		_CameraNearClip("NearCameraClip", Float) = 0.01
	}

	SubShader {
		Tags {
			"RenderType"="Opaque"
		}
		ZTest On
		Cull Back
		ZWrite On
		ColorMask RGB

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				return fixed4(0, 0, 0, 1) ;
			}
			ENDCG
		}

		// second pass is checking for camera clipping...
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float depth : DEPTH;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.depth = -UnityObjectToViewPos(v.vertex).z;
				return o;
			}

			half4 _CameraClipColor;
			float _CameraNearClip;

			fixed4 frag (v2f i) : SV_Target {
                float ii = step(i.depth, _CameraNearClip);
				return fixed4(i.depth,i.depth,ii,1);
			}
			ENDCG
		}
    }
}
