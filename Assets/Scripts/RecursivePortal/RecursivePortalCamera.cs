using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursivePortalCamera : PortalCamera
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private Camera[] portalCameras = new Camera[2];

    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;
    
    private const int iterations = 7;

    private new void Awake()
    {
        base.Awake();

        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);

        portalCameras[0].targetTexture = tempTexture1;
        portalCameras[1].targetTexture = tempTexture2;
    }

    private void Start()
    {
        portals[0].SetTexture(tempTexture1);
        portals[1].SetTexture(tempTexture2);
    }

    private void OnPreRender()
    {
        for (int i = 1; i <= iterations; ++i)
        {
            RenderCamera(portals[0], portals[1], portalCameras[0], iterations - i);
            RenderCamera(portals[1], portals[0], portalCameras[1], iterations - i);
        }
    }
}
