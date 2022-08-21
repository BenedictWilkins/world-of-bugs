// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShowBug"
{
	Properties {
		_BugType("BugType", Color) = (1,1,1,1)
		_CameraClipColor("CameraClipColor", Color) = (1,1,1,1)
		_CameraNearClip("NearCameraClip", Float) = 0.01
		_BackFaceColor("BackFaceColor", Color) = (1,1,1,1)
	}

	SubShader {
		Tags {
			"RenderType"="Opaque"
		}
		ZTest LEqual
		Cull Off
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
				if (i.depth > _CameraNearClip) {
					return fixed4(0, 0, 0, 1) ;
				}
				return _CameraClipColor;
			}
			ENDCG
		}

		// Render the inside of any object (if we can see inside, we have clipped through the geometry)
		Pass {
			Cull Front

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

			half4 _BackFaceColor;

			fixed4 frag (v2f i) : SV_Target {
				return _BackFaceColor;
			}
			ENDCG
		}
	}

	SubShader {
		Tags {
			"RenderType"="Bug"
		}
		//ZTest LEqual
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

			half4 _BugType;

			fixed4 frag (v2f i) : SV_Target {
				return _BugType;
			}
			ENDCG
		}
	}


	SubShader {
		Tags {
			"RenderType"="ZBug"
			"Queue"="Geometry+1"
		}

		ZWrite Off // TODO remove?
		ColorMask RGB


		Pass {

			Offset -1, -1
			ZTest Greater

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

			half4 _BugType;

			fixed4 frag (v2f i) : SV_Target {
				return _BugType;
			}
			ENDCG
		}

		Pass {

			ZTest Less

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

			half4 _BugType;

			fixed4 frag (v2f i) : SV_Target {
				return fixed4(0, 0, 0, 1);
			}
			ENDCG
		}
	}
}
