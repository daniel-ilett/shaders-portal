Shader "Portals/PortalMask"
{
    Properties
    {
		_MaskID("Mask ID", Int) = 1
    }
    SubShader
    {
        Tags
		{ 
			"RenderType" = "Opaque" 
			"Queue" = "Geometry+1" 
		}

        Pass
        {
			Stencil
			{
				Ref [_MaskID]
				Comp Always
				Pass replace
			}

			ColorMask 0
			ZWrite Off
			Cull Off
        }
    }
}
