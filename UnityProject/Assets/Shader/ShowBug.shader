// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShowBug"
{
	Properties {
		_BugType("BugType", Color) = (1,1,1,1)
		_CameraClipColor("CameraClipColor", Color) = (1,1,1,1)
		_CameraNearClip("NearCameraClip", Float) = 1
	}

	SubShader {
		Tags {
			"RenderType"="Opaque"
		}

		ZWrite On

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

		Pass { // second pass is checking for camera clipping...
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


	}

	SubShader {
		Tags {
			"RenderType"="Bug"
		}

		ZWrite On

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

		Pass { // second pass is checking for camera clipping...
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
					discard;
				}
				return _CameraClipColor;
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

		Pass {

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
