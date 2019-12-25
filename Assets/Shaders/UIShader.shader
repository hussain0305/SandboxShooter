Shader "Eurus/UIShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
		_EdgeWidth("Edge Width", Range(0,0.5)) = 0.1
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_GapWidth("Gap Width", Range(0,0.5)) = 0.1
		_GapColor("Gap Color", Color) = (1, 1, 1, 1)
		_CenterColor("Center Color", Color) = (1, 1, 1, 1)
		_EVerticalCo("Edge Vertical Correction", Range(0, 2)) = 1
		_EHorizontalCo("Edge Horizontal Correction", Range(0, 2)) = 1
		_GVerticalCo("Gap Vertical Correction", Range(0, 2)) = 1
		_GHorizontalCo("Gap Horizontal Correction", Range(0, 2)) = 1
	}
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _EdgeWidth;
			float4 _EdgeColor;
			float _GapWidth;
			float4 _GapColor;
			float4 _CenterColor;
			float _EVerticalCo;
			float _EHorizontalCo;
			float _GVerticalCo;
			float _GHorizontalCo;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				float2 uv = i.uv;
				if (uv.x < _EdgeWidth * _EHorizontalCo || uv.y < _EdgeWidth * _EVerticalCo || uv.x > (1 - _EdgeWidth * _EHorizontalCo) || uv.y > (1 - _EdgeWidth * _EVerticalCo)) {
					float4 col2 = _EdgeColor;
					col2.a = _EdgeColor.a;

					return col2;
				}
				else if (uv.x < _GapWidth * _GHorizontalCo || uv.y < _GapWidth * _GVerticalCo || uv.x >(1 - _GapWidth * _GHorizontalCo) || uv.y >(1 - _GapWidth * _GVerticalCo)) {
					float4 col2 = _GapColor;
					col2.a = _GapColor.a;

					return col2;
				}
				else {
					float4 col2 = _CenterColor;
					col2.a = _CenterColor.a;
					return col2;
				}
            }
            ENDCG
        }
    
	}
	FallBack "Diffuse"
}


//float2 uv = i.uv;
//
//fixed4 col = tex2D(_MainTex, i.uv);
//if (uv.x < _EdgeWidth || uv.y < _EdgeWidth || uv.x >(1 - _EdgeWidth) || uv.y >(1 - _EdgeWidth)) {
//	float4 col2 = _EdgeColor;
//	col2.a = _EdgeColor.a;
//
//	return col2;
//}
//else if (uv.x < _GapWidth || uv.y < _GapWidth || uv.x >(1 - _GapWidth) && uv.x || uv.y >(1 - _GapWidth)) {
//	float4 col2 = _GapColor;
//	col2.a = _GapColor.a;
//
//	return col2;
//}
//else {
//	float4 col2 = _CenterColor;
//	col2.a = _CenterColor.a;
//	return col2;
//}


//float2 uv = i.uv;
////fixed4 col = tex2D(_MainTex, i.uv);
//if (uv.x < (_EdgeWidth * (_ScreenParams.x / _ScreenParams.y)) || uv.y < _EdgeWidth || uv.x >(1 - (_EdgeWidth * (_ScreenParams.x / _ScreenParams.y))) || uv.y >(1 - _EdgeWidth)) {
//	float4 col2 = _EdgeColor;
//	col2.a = _EdgeColor.a;
//
//	return col2;
//}
//else if (uv.x < (_GapWidth * (_ScreenParams.x / _ScreenParams.y)) || uv.y < _GapWidth || uv.x >(1 - (_GapWidth * (_ScreenParams.x / _ScreenParams.y))) || uv.y >(1 - _GapWidth)) {
//	float4 col2 = _GapColor;
//	col2.a = _GapColor.a;
//
//	return col2;
//}
//else {
//	float4 col2 = _CenterColor;
//	col2.a = _CenterColor.a;
//	return col2;
//}
