﻿Shader "Hidden/CarsRender"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma target 5.0

            struct data
            {
                float2 pos;
                float2 dir;
            };

            struct v2g
            {
                uint2 id : ANY;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float3 uv : TEXCOORD0;
            };

            #include "./Geometries/Quad.cginc"
            
            uniform float _Height, _Size;
            uniform float4 _ForwardColor, _BackColor;
            uniform StructuredBuffer<data> _GeomData;

            float3 getCameraForward()
            {
                return -UNITY_MATRIX_V[2].xyz;
            }

            v2g vert(uint id : SV_VertexID, uint inst : SV_InstanceID)
            {
                v2g o;
                o.id = uint2(id, inst);

                return o;
            }   

            [maxvertexcount(128)]
            void geom(point v2g input[1], inout TriangleStream<g2f> outStream)
            {
                v2g v = input[0];
                uint id = v.id.x;
                uint inst = v.id.y;

                data d = _GeomData[inst];

                float view = dot(getCameraForward(), float3(d.dir.x, 0.0, d.dir.y));
                AppendQuad(d.pos, _Size, _Height, view > 0 ? 1.0 : -1.0, outStream);
            }

            float4 frag(g2f i) : COLOR
            {
                float dis = distance(i.uv.xy, float2(0.5, 0.5));
                float vdis = saturate(1.0 - dis);
                
                float4 c = i.uv.z >= 0.0 ? _BackColor : _ForwardColor;
                return float4((c * vdis).rgb, saturate(0.5 - dis));
            }

            ENDCG
        }
    }
}