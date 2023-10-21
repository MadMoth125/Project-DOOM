Shader "Custom/BillboardShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" { }
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Sample the main texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Assign the sampled color to the Albedo property
            o.Albedo = c.rgb;

            // Set the alpha value based on the sampled texture's alpha
            o.Alpha = c.a;
        }
        ENDCG
    }

    SubShader {
        Tags { "RenderType"="Transparent" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutputStandard o) {
            // Sample the main texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // Assign the sampled color to the Albedo property
            o.Albedo = c.rgb;

            // Set the alpha value based on the sampled texture's alpha
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Diffuse"
}