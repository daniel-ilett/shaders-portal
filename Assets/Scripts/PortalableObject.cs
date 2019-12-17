using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalableObject : MonoBehaviour
{
    private GameObject cloneObject;

    private bool isInPortal = true;

    [SerializeField]
    private Transform inPortal;
    [SerializeField]
    private Transform outPortal;

    private void Awake()
    {
        cloneObject = new GameObject();
        cloneObject.SetActive(false);
        var meshFilter = cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;
    }

    private void Update()
    {
        if(isInPortal)
        {
            // Update position of clone.
            Vector3 relativePos = inPortal.InverseTransformPoint(transform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
            cloneObject.transform.position = outPortal.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inPortal.rotation) * transform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
            cloneObject.transform.rotation = outPortal.rotation * relativeRot;
        }
    }

    public void SetIsInPortal(Portal inPortal, Portal outPortal)
    {
        this.inPortal = inPortal.transform;
        this.outPortal = outPortal.transform;

        isInPortal = true;
    }

    public void Warp()
    {

    }

    public void ExitPortal()
    {

    }
}
