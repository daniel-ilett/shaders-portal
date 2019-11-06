Shader "Portals/SpyroPortal"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_WorldCube ("World Cubemap", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		uniform fixed4 _Color;
		uniform samplerCUBE _WorldCube;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 col = texCUBE(_WorldCube, IN.viewDir) * _Color;
			o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
