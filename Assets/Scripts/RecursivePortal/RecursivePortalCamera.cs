using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursivePortalCamera : MonoBehaviour
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private Camera[] portalCameras = new Camera[2];

    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;

    private Camera mainCamera;

    private const int iterations = 7;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

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
        if(portals[0].IsRendererVisible() || portals[1].IsRendererVisible())
        {
            for (int i = 1; i <= iterations; ++i)
            {
                RenderCamera(portals[0], portals[1], portalCameras[0], iterations - i);
                RenderCamera(portals[1], portals[0], portalCameras[1], iterations - i);
            }
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, Camera renderCamera, int iterationID)
    {
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = renderCamera.transform;
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;

        for(int i = 0; i <= iterationID; ++i)
        {
            // Position the camera behind the other portal.
            Vector3 relativePos = inTransform.InverseTransformPoint(cameraTransform.position);
            relativePos.x *= -1;
            relativePos.z *= -1;
            cameraTransform.position = outTransform.TransformPoint(relativePos);

            // Rotate the camera to look through the other portal.
            Vector3 relativeUpDir = inTransform.InverseTransformDirection(cameraTransform.up);
            Vector3 relativeForwardDir = inTransform.InverseTransformDirection(cameraTransform.forward);

            Vector3 newUpDir = outTransform.TransformDirection(relativeUpDir);
            Vector3 newForwardDir = outTransform.TransformDirection(relativeForwardDir);

            Quaternion newLookRotation = Quaternion.LookRotation(newForwardDir, newUpDir);
            cameraTransform.localRotation = Quaternion.Euler(0, 180, 0) * newLookRotation;
        }

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
