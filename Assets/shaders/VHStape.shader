Shader "Hidden/Shader/VHStape"
{
    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    float _Intensity;
    TEXTURE2D_X(_InputTexture);
    float rand(in float2 st) {
        return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
    }
    float2 screenDistort(float2 uv)
    {
        float mask = sin(uv.y*6.0+_Time.y);
        if (mask < 0.7) {
            mask = 0.15;
        }
        float diff = (floor((uv.y + _Time.y * 0.0001)*8.0)- (uv.y + _Time.y * 0.0001) *8.0)*mask;
        return uv+ float2(rand(float2(0.0,uv.y+_Time.y*0.0001))*0.01*diff,0.0) + float2(sin(uv.y*10.0)*mask,0.0)*rand(_Time.y)*0.04;
    }

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        uint2 positionSS = screenDistort(input.texcoord) * _ScreenSize.xy;
        float3 outColor = float3(0.0, 0.0, 0.0);
        for (int i = -4; i < 4; i++) {
            outColor+= LOAD_TEXTURE2D_X(_InputTexture, positionSS+float2(i,0.0)).xyz*float3(max(-i,0.0) * 0.25, max(-i, 0.0) * 0.25, max(i, 0.0) * 0.25),float3(0.5,0.5,0.5);
        }
        outColor *= 0.125;

        return float4(outColor, 1);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "VHStape"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
