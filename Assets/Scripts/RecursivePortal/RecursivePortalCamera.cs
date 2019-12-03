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

    private RenderTexture tempTexture3;
    private RenderTexture tempTexture4;
    
    private const int iterations = 1;

    private new void Awake()
    {
        base.Awake();

        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24);
        tempTexture3 = new RenderTexture(Screen.width, Screen.height, 24);
        tempTexture4 = new RenderTexture(Screen.width, Screen.height, 24);

        portalCameras[0].targetTexture = tempTexture1;
        portalCameras[1].targetTexture = tempTexture2;
    }

    private void Start()
    {
        portals[0].SetTexture(tempTexture3);
        portals[1].SetTexture(tempTexture4);
    }

    private void OnPreRender()
    {
        for (int i = 0; i < iterations; ++i)
        {
            RenderCamera(portals[0], portals[1], portalCameras[0]);
            RenderCamera(portals[1], portals[0], portalCameras[1]);

            Graphics.Blit(tempTexture1, tempTexture3);
            Graphics.Blit(tempTexture2, tempTexture4);
        }
    }
}
