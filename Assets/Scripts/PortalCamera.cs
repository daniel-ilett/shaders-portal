using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private Camera[] portalCameras = new Camera[2];

    [SerializeField]
    private RenderTexture[] portalTextures = new RenderTexture[2];

    [SerializeField]
    private Material portalMaterial;

    private Camera mainCamera;

    private const int iterations = 2;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        RenderCamera(portals[0], portals[1], portalCameras[0]);
        RenderCamera(portals[1], portals[0], portalCameras[1]);

        // Render the first portal output onto the image.
        portalMaterial.SetTexture("_PortalTex", portalTextures[0]);
        portalMaterial.SetInt("_MaskID", 1);
        Graphics.Blit(src, src, portalMaterial, 1);

        // Render the second portal output onto the image.
        portalMaterial.SetTexture("_PortalTex", portalTextures[1]);
        portalMaterial.SetInt("_MaskID", 2);
        Graphics.Blit(src, src, portalMaterial, 1);

        Graphics.Blit(src, dst);
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, Camera renderCamera)
    {
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        // Position the camera behind the other portal.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos.x *= -1;
        relativePos.z *= -1;
        renderCamera.transform.position = outTransform.TransformPoint(relativePos);

        // Rotate the camera to look through the other portal.
        Vector3 relativeUpDir = inTransform.InverseTransformDirection(transform.up);
        Vector3 relativeForwardDir = inTransform.InverseTransformDirection(transform.forward);

        Vector3 newUpDir = outTransform.TransformDirection(relativeUpDir);
        Vector3 newForwardDir = outTransform.TransformDirection(relativeForwardDir);

        Quaternion newLookRotation = Quaternion.LookRotation(newForwardDir, newUpDir);

        renderCamera.transform.rotation = Quaternion.Euler(0, 180, 0) * newLookRotation;

        // Set the camera's oblique view frustum.
        Plane p = new Plane(-outTransform.forward, outTransform.position);
        Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(renderCamera.worldToCameraMatrix)) * clipPlane;

        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        renderCamera.projectionMatrix = newMatrix;

        // Render the camera to its render target.
        renderCamera.Render();
    }
}
