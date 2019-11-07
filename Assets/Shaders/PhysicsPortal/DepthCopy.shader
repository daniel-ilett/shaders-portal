// Found at https://support.unity3d.com/hc/en-us/articles/115000229323-Graphics-Blit-does-not-copy-RenderTexture-depth
Shader "Hidden/DepthCopy"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MyDepthTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite On ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment CopyDepthBufferFragmentShader
			
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
			
			sampler2D _MainTex;
			sampler2D_float _MyDepthTex;

			// important part: outputs depth from _MyDepthTex to depth buffer
			half4 CopyDepthBufferFragmentShader(v2f i, out float outDepth : SV_Depth) : SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE(_MyDepthTex, i.uv);
				outDepth = depth;
				return 0;
			}

			ENDCG
		}
	}
}
