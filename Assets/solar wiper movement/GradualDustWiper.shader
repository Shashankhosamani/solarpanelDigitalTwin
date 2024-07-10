Shader "Custom/GradualDustWiper"
{
    Properties
    {
        _CleanTex("Clean Texture", 2D) = "white" {}
        _DustyTex("Dusty Texture", 2D) = "white" {}
        _DustAmount("Dust Amount", Range(0, 1)) = 0
        _WipeProgress("Wipe Progress", Range(0, 1)) = 0
        _WipeWidth("Wipe Width", Range(0, 1)) = 0.1
    }
        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert alpha

            sampler2D _CleanTex;
            sampler2D _DustyTex;
            float _DustAmount;
            float _WipeProgress;
            float _WipeWidth;

            struct Input
            {
                float2 uv_CleanTex;
            };

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 cleanColor = tex2D(_CleanTex, IN.uv_CleanTex);
                fixed4 dustyColor = tex2D(_DustyTex, IN.uv_CleanTex);

                float wipePosition = _WipeProgress;
                float dustFactor = _DustAmount;

                // Calculate dust based on wipe progress
                if (IN.uv_CleanTex.y < wipePosition)
                {
                    dustFactor = 0;
                }
                else
                {
                    dustFactor = _DustAmount;
                }

                // Add a visible line for the wiper
                

                fixed4 finalColor = lerp(cleanColor, dustyColor, dustFactor);

                o.Albedo = finalColor.rgb;
                o.Alpha = finalColor.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}