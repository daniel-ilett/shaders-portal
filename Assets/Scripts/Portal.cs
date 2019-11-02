using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField]
    private List<Transform> portalCorners;

    [SerializeField]
    private Portal otherPortal;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private new BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        var diameter = PlayerController.instance.GetColliderRadius() * 3.0f;

        var transformScale = transform.localScale;
        var size = new Vector3(1.01f - diameter / transformScale.x, 
            1.01f - diameter / transform.localScale.y, collider.size.z);

        collider.size = size;
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < rigidbodies.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(rigidbodies[i].position);
            
            if(objPos.z > 0.0f)
            {
                Warp(rigidbodies[i]);
            }
        }
    }

    public List<Transform> GetCorners()
    {
        return portalCorners;
    }

    private void Warp(Rigidbody warpObj)
    {
        Vector3 pos = transform.InverseTransformPoint(warpObj.position);
        Vector3 upDir = transform.InverseTransformDirection(warpObj.transform.up);
        Vector3 fwdDir = transform.InverseTransformDirection(warpObj.transform.forward);

        pos.z *= -1;
        pos.x *= -1;

        warpObj.transform.position = otherPortal.transform.TransformPoint(pos);

        var newUpDir = otherPortal.transform.TransformDirection(upDir);
        var newFwdDir = otherPortal.transform.TransformDirection(fwdDir);
        Quaternion lookRot = Quaternion.LookRotation(newFwdDir, newUpDir);

        warpObj.transform.rotation = Quaternion.Euler(0, 180, 0) * lookRot;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            rigidbodies.Add(otherRigidbody);

            var player = other.GetComponent<PlayerController>();
            if(player != null)
            {
                player.ChangePortalTriggerCount(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var otherRigidbody = other.GetComponent<Rigidbody>();

        if(rigidbodies.Contains(otherRigidbody))
        {
            rigidbodies.Remove(otherRigidbody);

            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangePortalTriggerCount(-1);
            }
        }
    }
}
