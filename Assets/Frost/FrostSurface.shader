Shader "Custom/URPImageBlendWithDistortion"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Transparency ("Transparency", Range(0, 1)) = 0.5
        _Distortion ("Distortion", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        // Turn off culling so both sides of the mesh are rendered
        Cull Off

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            float _Transparency;
            float _Distortion;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the normal map for distortion
                half3 normal = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv)).rgb;

                // Distort the UVs based on the normal map and distortion parameter
                float2 distortedUV = IN.uv + normal.xy * _Distortion;

                // Sample the base texture
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortedUV);

                // Apply transparency control
                baseColor.a *= _Transparency;

                // Return the color with adjusted transparency
                return baseColor;
            }

            ENDHLSL
        }
    }

    FallBack Off
}
