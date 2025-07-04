Shader "Custom/VoxelFog"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.8, 0.8, 0.9, 1)
        _VoxelSize ("Voxel Size", Float) = 0.5
        _FogDensity ("Fog Density", Float) = 1.0
        _FogSpread ("Fog Spread", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float4 _FogColor;
            float _VoxelSize;
            float _FogDensity;
            float _FogSpread;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                float3 world = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.worldPos = world;
                OUT.positionHCS = TransformWorldToHClip(world);
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                // Voxelization: redondeamos la posición al tamaño del voxel
                float3 voxelPos = floor(IN.worldPos / _VoxelSize) * _VoxelSize;

                // Calculamos la distancia desde el centro del objeto (puedes ajustar esto)
                float distance = length(voxelPos);

                // Fog factor: entre más lejos, más suave y difusa la niebla
                float fogFactor = saturate(exp(-_FogDensity * pow(distance * _FogSpread, 1.2)));

                // Color + alpha según densidad
                float4 color = _FogColor;
                color.a *= 1.0 - fogFactor;

                return color;
            }
            ENDHLSL
        }
    }
}
