Shader "Hidden/Depth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amp ("Texture", 2D) = "amp" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed _Amp;
            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed depth = tex2D(_CameraDepthTexture, i.uv).r;
                //depth = Linear01Depth(depth);
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= (1- depth);

                return col;
            }
            ENDCG
        }
    }
}
