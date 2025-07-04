Shader "Custom/PortalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TimeSpeed ("Rotation Speed", Float) = 1
        _GlowColor ("Glow Color", Color) = (1, 0, 1, 1)
        _GlowWidth ("Glow Width", Float) = 0.3
        _DarkCenterSize ("Dark Center Size", Float) = 0.3
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

            sampler2D _MainTex;
            float _TimeSpeed;
            float4 _GlowColor;
            float _GlowWidth;
            float _DarkCenterSize;

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
                o.uv = v.uv * 2.0 - 1.0; // para que vaya de -1 a 1
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float angle = atan2(uv.y, uv.x);
                float radius = length(uv);

                // Rotación
                float time = _Time.y * _TimeSpeed;
                angle += time;

                // Círculo oscuro en el centro
                float center = smoothstep(_DarkCenterSize, 0.0, radius);

                // Bordes púrpuras brillantes (tipo vórtice)
                float glow = sin(angle * 10.0) * 0.5 + 0.5;
                glow *= smoothstep(_DarkCenterSize, _DarkCenterSize + _GlowWidth, radius);
                
                float4 color = lerp(float4(0, 0, 0, 1), _GlowColor, glow);
                color.rgb *= center;

                return color;
            }
            ENDCG
        }
    }
}
