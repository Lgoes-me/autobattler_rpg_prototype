Shader "Custom/Stencil"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" "RenderPipeline" = "UniversalPipeline"}

        Pass 
        {
            Blend Zero One
            ZWrite Off

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
        }
    }
}