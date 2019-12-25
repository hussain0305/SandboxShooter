Shader "Eurus/GridForming"
{
	Properties
	{
		_LineColor("Line Color", Color) = (1,1,1,1)
		_LineBrightness("Line Brightness", float) = 1
		_CellColor("Cell Color", Color) = (0,0,0,0)
		_CellBrightness("Cell Brightness", float) = 1
		_SelectedColor("Selected Color", Color) = (1,0,0,1)
		[PerRendererData] _MainTex("Albedo (RGB)", 2D) = "white" {}
		[IntRange] _GridSize("Grid Size", Range(1,100)) = 10
		_LineSize("Line Size", Range(0,1)) = 0.15
		[IntRange] _SelectCell("Select Cell Toggle ( 0 = False , 1 = True )", Range(0,1)) = 0.0
		[IntRange] _SelectedCellX("Selected Cell X", Range(0,100)) = 0.0
		[IntRange] _SelectedCellY("Selected Cell Y", Range(0,100)) = 0.0

		_FormLine("Formation Percentage", Range(0, 1)) = 0
	}

		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 500//100

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

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

				half _Glossiness = 0.0;
				half _Metallic = 0.0;
				float4 _LineColor;
				float4 _CellColor;
				float4 _SelectedColor;
				sampler2D _MainTex;

				float _GridSize;
				float _LineSize;

				float _SelectCell;
				float _SelectedCellX;
				float _SelectedCellY;

				float _LineBrightness;
				float _CellBrightness;

				float _FormLine;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float2 uv = i.uv;

					_SelectedCellX = floor(_SelectedCellX);
					_SelectedCellY = floor(_SelectedCellY);

					fixed4 c = float4(0.0,0.0,0.0,0.0);

					float gsize = floor(_GridSize);

					float pixelBrightness;

					gsize += _LineSize;

					float2 id;

					id.x = floor(uv.x / (1.0 / gsize));
					id.y = floor(uv.y / (1.0 / gsize));

					float4 color = _CellColor;
					pixelBrightness = _CellColor.w * _CellBrightness;

					if (round(_SelectCell) == 1.0 && id.x == _SelectedCellX && id.y == _SelectedCellY)
					{
						pixelBrightness = _SelectedColor.w * _CellBrightness;
						color = _SelectedColor;
					}

					if (frac(uv.x * gsize) <= _LineSize || frac(uv.y * gsize) <= _LineSize)
					{
						pixelBrightness = _LineColor.w * _LineBrightness;
						color = _LineColor;
					}

					if (pixelBrightness == 0.0) {
						clip(c.a - 1.0);
					}


					c = fixed4(color.x * _LineBrightness, color.y * _LineBrightness, color.z * _LineBrightness, pixelBrightness);
					if (uv.x < (0.5 - _FormLine) || uv.x > (0.5 + _FormLine)) {
						c.a = 0;
					}
					return c;
				}
				ENDCG
			}
		}
}