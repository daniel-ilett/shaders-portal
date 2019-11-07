Shader "Portals/PortalRender"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MaskID("Mask ID", Int) = 1
    }
    SubShader
    {
		Tags
		{ 
			"RenderType" = "Opaque" 
			"Queue" = "Geometry+2" 
		}

        Pass
        {
			Stencil
			{
				Ref [_MaskID]
				Comp equal
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            uniform sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
