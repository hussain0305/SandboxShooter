Shader "Unlit/EnvObjectShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
		_EdgeWidth("Edge Width", Range(0,1)) = 0.1
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_CenterColor("Center Color", Color) = (1, 1, 1, 1)
		_EdgeTransparency("Edge Transparency", Range(0, 0.75)) = 0.1
		_CenterTransparency("Center Transparency", Range(0, 0.75)) = 0.1

		_ColorTint("Color Tint", Color) = (1, 1, 1, 1)
		_BumpMap("Normal Map", 2D) = "bump" {}
		_RimColor("Rim Color", Color) = (1, 1, 1, 1)
		_RimPower("Rim Power", Range(1.0, 6.0)) = 3.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
        LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		/*CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float4 color : Color;
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};

		float4 _ColorTint;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _RimColor;
		float _RimPower;

		void surf(Input IN, inout SurfaceOutput o)
		{
			IN.color = _ColorTint;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
			o.Normal = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));

			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG*/

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
			float4 _CenterColor;
			float _EdgeTransparency;
			float _CenterTransparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				if (uv.x < _EdgeWidth || uv.y < _EdgeWidth || uv.x > (1 - _EdgeWidth) || uv.y >(1 - _EdgeWidth)) {
					float4 col2 = _EdgeColor;
					col2.a = _EdgeTransparency;
					
					return col2;
				}
				else {
					float4 col2 = _CenterColor;
					col2.a = _CenterTransparency;
					return col2;
				}
                //return col;
            }
            ENDCG
        }
    
	}
	FallBack "Diffuse"
}


/*

CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
*/