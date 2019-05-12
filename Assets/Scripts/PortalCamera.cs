using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private Transform[] portals = new Transform[2];

    [SerializeField]
    private Camera[] portalCameras = new Camera[2];

    [SerializeField]
    private RenderTexture[] portalTextures = new RenderTexture[2];

    [SerializeField]
    private Material portalMaterial;

    private const int iterations = 2;
    
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        RenderCamera(portals[0], portals[1], portalCameras[0]);
        RenderCamera(portals[1], portals[0], portalCameras[1]);

        portalMaterial.SetTexture("_PortalTex", portalTextures[0]);
        portalMaterial.SetInt("_MaskID", 1);
        Graphics.Blit(src, src, portalMaterial, 1);

        portalMaterial.SetTexture("_PortalTex", portalTextures[1]);
        portalMaterial.SetInt("_MaskID", 2);
        Graphics.Blit(src, src, portalMaterial, 1);

        Graphics.Blit(src, dst);
    }

    private void RenderCamera(Transform inPortal, Transform outPortal, Camera renderCamera)
    {
        Vector3 relativePos = inPortal.InverseTransformPoint(transform.position);
        relativePos.x *= -1;
        relativePos.z *= -1;
        renderCamera.transform.position = outPortal.TransformPoint(relativePos);

        Vector3 relativeUpDir = inPortal.InverseTransformDirection(transform.up);
        Vector3 relativeForwardDir = inPortal.InverseTransformDirection(transform.forward);

        Vector3 newUpDir = outPortal.TransformDirection(relativeUpDir);
        Vector3 newForwardDir = outPortal.TransformDirection(relativeForwardDir);

        Quaternion newLookRotation = Quaternion.LookRotation(newForwardDir, newUpDir);

        renderCamera.transform.localRotation = Quaternion.Euler(0, 180, 0) * newLookRotation;
        
        //renderCamera.transform.rotation = transform.rotation * Quaternion.Inverse(inPortal.rotation) * outPortal.rotation;

        renderCamera.nearClipPlane = Vector3.Distance(transform.position, inPortal.position);
        renderCamera.Render();
    }
}
