Shader "Eurus/HealthBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ColorRed("Color Red", Color) = (1,1,1,1)
		_ColorGreen("Color Green", Color) = (1,1,1,1)
		_HealthPercentage("Health Percentage", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
			fixed4 _ColorRed;
			fixed4 _ColorGreen;
			float _HealthPercentage;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				//fixed4 col = tex2D(_MainTex, i.uv);
				//if (col.a == 0) {
				//	return col;
				//}
				if (uv.x < _HealthPercentage)
				{
					return _ColorGreen;
				}
				else
				{
					return _ColorRed;
				}
                
            }
            ENDCG
        }
    }
}
