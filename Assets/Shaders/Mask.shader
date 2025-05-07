Shader "Custom/Mask"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" "RenderPipeline" = "UniversalPipeline"}

        Pass 
        {
            Stencil
            {
                Ref 1
                Comp NotEqual
            }
        }
    }
}