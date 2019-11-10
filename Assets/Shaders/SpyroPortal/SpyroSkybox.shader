Shader "Portals/SpyroSkybox"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_WorldCube ("World Cubemap", CUBE) = "" {}
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque" 
			"Queue" = "Geometry+1"
		}
        LOD 200

		Cull Off

        CGPROGRAM
        #pragma surface surf Unlit noforwardadd

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		uniform fixed4 _Color;
		uniform samplerCUBE _WorldCube;

        struct Input
        {
			float3 viewDir;
        };

		fixed4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 col = texCUBE(_WorldCube, IN.viewDir) * _Color;
			o.Albedo = col.xyz;
            o.Alpha = 1.0f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
