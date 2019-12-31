Shader "Eurus/GlowLoading" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Glow("Intensity", Range(0, 3)) = 1
		_Percentage("Loading Percentage", Range(0, 1)) = 0.1
	}
		SubShader{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			LOD 100
			Cull Off
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha

			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					fixed4 _Color;
					half _Glow;
					float _Percentage;

					struct appdata
					{
						float4 vertex : POSITION;
						float2 uv : TEXCOORD0;
					};

					struct v2f
					{
						float2 uv : TEXCOORD0;
						float4 vertex : SV_POSITION;
					};


					v2f vert(appdata v) {
						v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						return o;
					}

					fixed4 frag(v2f i) : SV_Target {
						float2 uv = i.uv;
						float4 col = (1, 1, 1, 0);
						col *= _Color;
						col *= _Glow;
						if (uv.x > _Percentage) {
							col.a = 1;
						}
						return col;
					}
				ENDCG
			}
		}
}