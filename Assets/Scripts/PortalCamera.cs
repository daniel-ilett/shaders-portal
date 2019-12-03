using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    protected Camera mainCamera;

    protected void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    protected void RenderCamera(Portal inPortal, Portal outPortal, Camera renderCamera)
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
