Shader "MatrixAI/MatrixGrid"
{
    Properties
    {
        _Color ("Grid Color", Color) = (0, 1, 0, 1)
        _GridSpacing ("Grid Spacing", Float) = 10.0
        _LineWidth ("Line Width", Float) = 0.1
        _Glow ("Glow Intensity", Float) = 1.0
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
            };

            struct v2f
            {
                float4 worldPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _GridSpacing;
            float _LineWidth;
            float _Glow;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 grid = frac(i.worldPos.xz / _GridSpacing);
                float2 line_grid = smoothstep(_LineWidth, 0.0, grid) + smoothstep(1.0 - _LineWidth, 1.0, grid);
                float gridVal = max(line_grid.x, line_grid.y);
                
                fixed4 col = _Color * gridVal * _Glow;
                return col;
            }
            ENDCG
        }
    }
}