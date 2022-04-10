Shader "Hidden/Shader/GrayScale"

{

    HLSLINCLUDE

    #pragma target 4.5

    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

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

    float4 CustomPostProcess(Varyings input) : SV_Target

    {

        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        uint2 positionSS = floor(input.texcoord*512.0)/512.0 * _ScreenSize.xy;

        float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;
        //float levels = 32;
        float greyscale = max(outColor.r, max(outColor.g, outColor.b));
        //float lower = floor(greyscale * levels) / levels;
        //float lowerDiff = abs(greyscale - lower);
        //float upper = ceil(greyscale * levels) / levels;
        //float upperDiff = abs(upper - greyscale);
        //float level = lowerDiff <= upperDiff ? lower : upper;
        //float adjustment = level / greyscale;
        //float quantize = floor(greyscale*2.0)/2.0;
        return float4(floor(outColor*32.0)/32, 1);

    }

    ENDHLSL

    SubShader

    {

        Pass

        {

            Name "GrayScale"

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