Shader "Custom/LavaShader"
{
    Properties
    {
        _Color ("Lava Tint", Color) = (1, 0.5, 0, 1)
        _MainTex ("Lava Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FlowSpeed ("Flow Speed", Float) = 1.0
        _DistortionStrength ("Distortion Strength", Float) = 0.1
        _EmissionIntensity ("Emission Intensity", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NoiseTex;

        float _FlowSpeed;
        float _DistortionStrength;
        float _EmissionIntensity;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float time = _Time.y * _FlowSpeed;

            // Animate UVs using noise texture
            float2 noiseUV = IN.uv_MainTex + float2(time, 0);
            float2 noise = tex2D(_NoiseTex, noiseUV).rg;

            // Distort main UVs
            float2 distortedUV = IN.uv_MainTex + (noise - 0.5) * _DistortionStrength;

            fixed4 c = tex2D(_MainTex, distortedUV) * _Color;

            o.Albedo = c.rgb;
            o.Metallic = 0;
            o.Smoothness = 0.5;

            // Add emission for glow
            o.Emission = c.rgb * _EmissionIntensity;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
