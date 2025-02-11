Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 texelSize = _BlurSize / _ScreenParams.xy;
                half4 color = tex2D(_MainTex, i.uv) * 0.36;
                color += tex2D(_MainTex, i.uv + texelSize * float2( 1,  0)) * 0.12;
                color += tex2D(_MainTex, i.uv + texelSize * float2(-1,  0)) * 0.12;
                color += tex2D(_MainTex, i.uv + texelSize * float2( 0,  1)) * 0.12;
                color += tex2D(_MainTex, i.uv + texelSize * float2( 0, -1)) * 0.12;
                return color;
            }
            ENDCG
        }
    }
}