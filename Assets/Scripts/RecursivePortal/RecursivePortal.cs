using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursivePortal : MonoBehaviour
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private Camera[] portalCameras = new Camera[2];

    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;

    private RenderTexture tempTexture3;
    private RenderTexture tempTexture4;

    private Camera mainCamera;

    int a = 0;
    
    private const int iterations = 5;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
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
        renderCamera.transform.localRotation = Quaternion.Euler(0, 180, 0) * newLookRotation;

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
