Shader "Eurus/SurfaceGrid"
{
    Properties
    {
		_LineColor("Line Color", Color) = (1,1,1,1)
		_CellColor("Cell Color", Color) = (0,0,0,0)
		[IntRange] _GridSize("Grid Size", Range(1,100)) = 10
		_LineSize("Line Size", Range(0,1)) = 0.15

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Emission("Emission", Range(0,1)) = 0.15
	}
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha


        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
		
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex : TEXCOORD0;
        };

        half _Glossiness;
        half _Metallic;
		half _Emission;
        fixed4 _LineColor;
		fixed4 _CellColor;
		int _GridSize;
		half _LineSize;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float gsize = floor(_GridSize);

			float pixelBrightness;

			gsize += _LineSize;

			float2 id;

			id.x = floor(IN.uv_MainTex.x / (1.0 / gsize));
			id.y = floor(IN.uv_MainTex.y / (1.0 / gsize));

			float4 color = _CellColor;

			if (frac(IN.uv_MainTex.x * gsize) <= _LineSize || frac(IN.uv_MainTex.y * gsize) <= _LineSize)
			{
				color = _LineColor;
			}
			
			o.Albedo = color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = o.Albedo * _Emission;
			o.Alpha = color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
