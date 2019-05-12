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

    private void Update()
    {
        Vector3 relativePos = inPortal.InverseTransformPoint(transform.position);
        Vector3 inCamPos = outPortal.TransformPoint(relativePos);

        inPortalCamera.transform.position = inCamPos;
        inPortalCamera.transform.rotation = transform.rotation * Quaternion.Inverse(inPortal.rotation) * outPortal.rotation;

        inPortalCamera.nearClipPlane = Vector3.Distance(transform.position, inPortal.position);

        relativePos = outPortal.InverseTransformPoint(transform.position);
        Vector3 outCamPos = inPortal.TransformPoint(relativePos);

        outPortalCamera.transform.position = outCamPos;
        outPortalCamera.transform.rotation = transform.rotation * Quaternion.Inverse(outPortal.rotation) * inPortal.rotation;

        outPortalCamera.nearClipPlane = Vector3.Distance(transform.position, outPortal.position);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        portalMaterial.SetTexture("_PortalTex", inPortalTex);
        portalMaterial.SetInt("_MaskID", 1);
        Graphics.Blit(src, src, portalMaterial, 1);

        portalMaterial.SetTexture("_PortalTex", outPortalTex);
        portalMaterial.SetInt("_MaskID", 2);
        Graphics.Blit(src, src, portalMaterial, 1);

        Graphics.Blit(src, dst);
    }
}
