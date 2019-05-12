using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private Transform inPortal;

    [SerializeField]
    private Transform outPortal;

    [SerializeField]
    private Camera inPortalCamera;

    [SerializeField]
    private Camera outPortalCamera;

    [SerializeField]
    private Material portalMaterial;

    [SerializeField]
    private RenderTexture inPortalTex;

    [SerializeField]
    private RenderTexture outPortalTex;
    
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        RenderCamera(inPortal, outPortal, inPortalCamera);
        RenderCamera(outPortal, inPortal, outPortalCamera);

        portalMaterial.SetTexture("_PortalTex", inPortalTex);
        portalMaterial.SetInt("_MaskID", 1);
        Graphics.Blit(src, src, portalMaterial, 1);

        portalMaterial.SetTexture("_PortalTex", outPortalTex);
        portalMaterial.SetInt("_MaskID", 2);
        Graphics.Blit(src, src, portalMaterial, 1);

        Graphics.Blit(src, dst);
    }

    private void RenderCamera(Transform inPortal, Transform outPortal, Camera renderCamera)
    {
        Vector3 relativePos = inPortal.InverseTransformPoint(transform.position);
        Vector3 inCamPos = outPortal.TransformPoint(relativePos);

        renderCamera.transform.position = inCamPos;
        renderCamera.transform.rotation = transform.rotation * Quaternion.Inverse(inPortal.rotation) * outPortal.rotation;

        renderCamera.nearClipPlane = Vector3.Distance(transform.position, inPortal.position);
        renderCamera.Render();
    }
}
