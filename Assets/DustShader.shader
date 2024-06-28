Shader "Custom/DustShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DustTex("Dust (RGB)", 2D) = "white" {}
        _DustAmount("Dust Amount", Range(0, 1)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            sampler2D _MainTex;
            sampler2D _DustTex;
            float _DustAmount;

            struct Input
            {
                float2 uv_MainTex;
            };

            half4 LightingStandard(SurfaceOutputStandard s, half3 lightDir, half atten)
            {
                half4 c;
                c.rgb = s.Albedo * _LightColor0.rgb * (atten * 2);
                c.a = s.Alpha;
                return c;
            }

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                half4 d = tex2D(_DustTex, IN.uv_MainTex);
                o.Albedo = lerp(c.rgb, d.rgb, _DustAmount);
                o.Alpha = 1.0;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
