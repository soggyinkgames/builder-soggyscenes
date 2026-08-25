Shader "MatrixAI/MatrixRain"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 1, 0, 1)
        _Speed ("Speed", Float) = 1.0
        _Density ("Density", Float) = 10.0
        _Glow ("Glow", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend One One
        ZWrite Off
        Cull Off

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
            float4 _Color;
            float _Speed;
            float _Density;
            float _Glow;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xyz *= 1.0; // Assume inverted mesh or handle inside
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * _Density;
                float col_id = floor(uv.x);
                float row_speed = hash(float2(col_id, 0.0)) * 0.5 + 0.5;
                float time = _Time.y * _Speed * row_speed;
                
                float y = frac(uv.y + time);
                float char_id = floor(uv.y + time);
                
                // Vertical gradient for rain tail
                float rain = pow(y, 3.0);
                
                // Pseudo-character flicker
                float char_val = hash(float2(col_id, char_id));
                
                fixed4 col = _Color * rain * char_val * _Glow;
                return col;
            }
            ENDCG
        }
    }
}