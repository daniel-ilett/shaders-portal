Shader "Portals/RecursivePortalMask"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags 
		{ 
			"RenderType" = "Opaque"
		}

        Pass
        {
			Stencil
			{
				Ref 1
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
					float4 vertex : SV_POSITION;
					float4 screenPos : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.screenPos = ComputeScreenPos(o.vertex);
					return o;
				}

				uniform sampler2D _MainTex;

				fixed4 frag(v2f i) : SV_Target
				{
					float2 uv = i.screenPos.xy / i.screenPos.w;
					fixed4 col = tex2D(_MainTex, uv);
					return col;
				}
			ENDCG
        }
    }
}
