using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPortalCamera : PortalCamera
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private Camera[] portalCameras = new Camera[2];

    [SerializeField]
    private Material portalMaterial;

    private RenderTexture tempTexture;

    private const int maskID1 = 1;
    private const int maskID2 = 2;

    private new void Awake()
    {
        base.Awake();
        tempTexture = new RenderTexture(Screen.width, Screen.height, 24);

        portalCameras[0].targetTexture = tempTexture;
        portalCameras[1].targetTexture = tempTexture;
    }

    private void Start()
    {
        portals[0].SetMaskID(maskID1);
        portals[1].SetMaskID(maskID2);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        // Render the first portal output onto the image.
        RenderCamera(portals[0], portals[1], portalCameras[0]);
        portalMaterial.SetInt("_MaskID", maskID1);
        Graphics.Blit(tempTexture, src, portalMaterial);

        // Render the second portal output onto the image.
        RenderCamera(portals[1], portals[0], portalCameras[1]);
        portalMaterial.SetInt("_MaskID", maskID2);
        Graphics.Blit(tempTexture, src, portalMaterial);

        // Output the combined texture.
        Graphics.Blit(src, dst);
    }
}
