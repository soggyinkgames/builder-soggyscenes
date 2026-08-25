// Matrix digital-rain terrain shader – URP unlit
// Shader name kept exactly so M_MatrixGrid_Terrain, M_MatrixGrid_Water,
// and M_MatrixPlane materials continue to reference it without reassignment.
Shader "MatrixAI/MatrixGrid"
{
    Properties
    {
        // ── Active properties ──────────────────────────────────────────
        [Toggle(_SPHERICAL_MAPPING)] _SphericalMapping ("Spherical Mapping (Dome)", Float) = 0
        _Color       ("Rain Color",              Color)       = (0, 1, 0, 1)
        _Glow        ("Glow Intensity",          Float)       = 1.5
        _CellSize    ("Cell Size (world units)", Float)       = 4.0
        _Speed       ("Fall Speed",              Float)       = 2.0
        _TrailLength ("Trail Length (cells)",    Float)       = 10.0
        _Density     ("Column Density",          Range(0,1))  = 0.70

        // ── Legacy – kept so existing material serialisation stays intact ─
        _GridSpacing ("Grid Spacing (Legacy)",   Float)       = 50.0
        _LineWidth   ("Line Width (Legacy)",     Float)       = 0.05
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue"          = "Geometry"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }
            Cull Off

            HLSLPROGRAM
            #pragma vertex   Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma shader_feature_local _SPHERICAL_MAPPING

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ── Material uniform block (SRP-Batcher compatible) ────────
            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float  _Glow;
                float  _CellSize;
                float  _Speed;
                float  _TrailLength;
                float  _Density;
                float  _GridSpacing;
                float  _LineWidth;
            CBUFFER_END

            // ── Vertex / fragment structs ──────────────────────────────
            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // ── Hash helpers ───────────────────────────────────────────
            float Hash1(float  n) { return frac(sin(n)                           * 43758.5453); }
            float Hash2(float2 p) { return frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453); }

            // ── Procedural 5×5 dot-matrix glyph ───────────────────────
            // uv : 0..1 within the inner (margin-trimmed) area of the cell.
            // Returns 1 (pixel lit) or 0 (pixel off).
            float GlyphPixel(float2 uv, float charSeed)
            {
                // Clamp to avoid edge-pixel index overflow
                float2 px = clamp(floor(uv * 5.0), 0.0, 4.0);
                float  s  = Hash2(float2(charSeed * 73.3 + px.x,
                                         charSeed * 51.7 + px.y));
                return step(0.38, s);   // ~62 % fill – letter-density feel
            }

            // ── Vertex shader ──────────────────────────────────────────
            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionOS = IN.positionOS.xyz;
                return OUT;
            }

            // ── Fragment shader ────────────────────────────────────────
            half4 Frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                float  cs        = max(_CellSize, 0.001);
                float2 cellCoord;

                #if defined(_SPHERICAL_MAPPING)
                    // Spherical mapping for dome (flows from world top +Y down)
                    float3 centerWS = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w);
                    float3 relPos = IN.positionWS - centerWS;
                    float radius = length(relPos);
                    float3 dir = relPos / max(radius, 0.001);

                    // Horizontal column based on angle around Y
                    float theta = atan2(dir.x, dir.z); 
                    // Vertical row based on angle from world top (+Y)
                    float phi = acos(clamp(dir.y, -1.0, 1.0));

                    cellCoord.x = theta * radius / cs;
                    cellCoord.y = phi * radius / cs;
                #else
                    // World XZ → cell grid
                    cellCoord = IN.positionWS.xz / cs;
                #endif

                float2 cellId    = floor(cellCoord);
                float2 cellUV    = frac(cellCoord);

                // ── Per-column random properties ───────────────────────
                float colHash = Hash1(cellId.x * 127.3 + 17.9);

                // Density gate – skip inactive columns (return black)
                if (colHash > _Density)
                    return half4(0, 0, 0, 1);

                float colSpeed = _Speed * (0.5 + 0.8 * colHash);
                // Large phase spread so rain is distributed across the terrain at t=0
                float colPhase = Hash1(cellId.x * 53.9 + 0.13) * 100.0;

                // ── Tiled rain: period keeps drops cycling continuously ─
                // Period long enough to have gaps between successive drops.
                float period = max(_TrailLength * 8.0, _TrailLength + 1.0);

                // Large positive offset makes fmod safe for any world-Z sign
                float cellRow = fmod(cellId.y + 100000.0, period);
                float headPos = fmod(_Time.y * colSpeed + colPhase, period);

                // trailDist = 0 → head glyph (brightest)
                // trailDist = TrailLength-1 → tail end (dimmest)
                float trailDist = fmod(headPos - cellRow + period, period);

                if (trailDist >= _TrailLength)
                    return half4(0, 0, 0, 1);

                // ── Brightness gradient ────────────────────────────────
                float trailT     = trailDist / max(_TrailLength, 0.001);
                float brightness = pow(saturate(1.0 - trailT), 2.2);

                // ── Cell margin (gap between glyphs) ───────────────────
                float  margin   = 0.07;
                float2 innerUV  = (cellUV - margin) / (1.0 - 2.0 * margin);
                if (any(innerUV < 0.0) || any(innerUV > 1.0))
                    return half4(0, 0, 0, 1);

                // ── Character selection ────────────────────────────────
                // Head flickers every ~1/12 s; tail glyphs are fixed per cell.
                float flicker  = (trailDist < 1.0) ? floor(_Time.y * 12.0) * 0.01 : 0.0;
                float charSeed = Hash2(cellId + float2(0.0, flicker));

                float glyphPx = GlyphPixel(innerUV, charSeed);
                if (glyphPx < 0.5)
                    return half4(0, 0, 0, 1);

                // ── Final colour ───────────────────────────────────────
                // Head: bright white-green. Trail: _Color fading to black.
                float3 col = (trailDist < 1.0)
                    ? float3(0.75, 1.0, 0.75)          // leading glyph
                    : _Color.rgb * brightness;           // trailing glyphs

                col *= _Glow;
                return half4(col, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
