using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PortalableObject : MonoBehaviour
{
    private GameObject cloneObject;

    private bool isInPortal = false;
    
    private Portal inPortal;
    private Portal outPortal;

    private void Awake()
    {
        cloneObject = new GameObject();
        cloneObject.SetActive(false);
        var meshFilter = cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;
    }

    private void FixedUpdate()
    {
        if(cloneObject.activeSelf && inPortal.IsPlaced() && outPortal.IsPlaced())
        {
            var inTransform = inPortal.transform;
            var outTransform = outPortal.transform;

            // Update position of clone.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
    }

    public void SetIsInPortal(Portal inPortal, Portal outPortal)
    {
        this.inPortal = inPortal;
        this.outPortal = outPortal;

        if (PortalCamera.isRecursive)
        {
            cloneObject.SetActive(true);
        }
        
        isInPortal = true;
    }

    public void Warp()
    {
        var inTransform = inPortal.transform;
        var outTransform = outPortal.transform;

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;
    }

    public void ExitPortal()
    {
        cloneObject.SetActive(false);
        isInPortal = false;
    }
}
