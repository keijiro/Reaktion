Shader "Custom/Scaling Diffuse"
{
    Properties
    {
        _MainTex("Base", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Scale("Scale Facor (<=1.0)", Vector) = (1, 1, 1, 0)
        _Scale2("Scale Facor 2", Vector) = (1, 1, 1, 0)
        _Scale3("Scale Facor 3", Vector) = (1, 1, 1, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert addshadow

        sampler2D _MainTex;
        half4 _Color;
        half3 _Scale;
        half3 _Scale2;
        half3 _Scale3;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v)
        {
            half3 s = _Scale * _Scale2 * _Scale3;
            v.vertex.xyz *= s;
            v.normal = normalize(v.normal / s);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * _Color.rgb;
            o.Alpha = c.a * _Color.a;
        }
        ENDCG
    } 
    FallBack "Diffuse"
}
