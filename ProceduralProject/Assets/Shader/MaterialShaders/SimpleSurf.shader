Shader "EriksShaders/SimpleSurf"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Albedo 1 (RGB)", 2D) = "white" {}
        _SecondaryTex ("Albedo 2 (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SecondaryTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SecondaryTex;
            float4 color : COLOR;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            IN.uv_SecondaryTex.x += _Time.y;

            fixed4 c1 = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 c2 = tex2D (_SecondaryTex, IN.uv_SecondaryTex);
            o.Albedo = lerp(c1, c2, IN.color.r);
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
