Shader "Portals/PortalMask"
{
    Properties
    {
		_Colour("Base Colour", Color) = (1, 1, 1, 1)
		_MaskID("Mask ID", Int) = 1
    }
    SubShader
    {
        Tags
		{ 
			"RenderType" = "Opaque" 
			"Queue" = "Geometry" 
		}

        Pass
        {
			Stencil
			{
				Ref [_MaskID]
				Comp Always
				Pass replace
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			uniform float4 _Colour;

			fixed4 frag(v2f i) : SV_Target
			{
				return _Colour;
			}
		ENDCG
        }
    }
}
