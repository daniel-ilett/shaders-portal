using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField]
    private Portal otherPortal;

    [SerializeField]
    private Renderer outlineRenderer;

    [SerializeField]
    private Color portalColour;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    private Material material;
    private new Renderer renderer;
    private new BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    private void Start()
    {
        var diameter = PlayerController.instance.GetColliderRadius() * 3.0f;
        collider.size = new Vector3(collider.size.x - diameter,
            collider.size.y - diameter, collider.size.z);

        PlacePortal(transform.position, transform.forward, transform.up);
        SetColour(portalColour);
    }

    private void Update()
    {
        for (int i = 0; i < rigidbodies.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(rigidbodies[i].position);

            if (objPos.z > 0.0f)
            {
                Warp(rigidbodies[i]);
            }
        }
    }

    public void SetColour(Color colour)
    {
        material.SetColor("_Colour", colour);
        outlineRenderer.material.SetColor("_OutlineColour", colour);
    }

    public void SetMaskID(int id)
    {
        material.SetInt("_MaskID", id);
    }

    public void SetTexture(RenderTexture tex)
    {
        material.mainTexture = tex;
    }

    public bool IsRendererVisible()
    {
        return renderer.isVisible;
    }

    private void Warp(Rigidbody warpObj)
    {
        Vector3 pos = transform.InverseTransformPoint(warpObj.position);
        Vector3 upDir = transform.InverseTransformDirection(warpObj.transform.up);
        Vector3 fwdDir = transform.InverseTransformDirection(warpObj.transform.forward);

        pos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * pos;

        warpObj.transform.position = otherPortal.transform.TransformPoint(pos);

        var newUpDir = otherPortal.transform.TransformDirection(upDir);
        var newFwdDir = otherPortal.transform.TransformDirection(fwdDir);
        Quaternion lookRot = Quaternion.LookRotation(newFwdDir, newUpDir);

        warpObj.transform.rotation = Quaternion.Euler(0, 180, 0) * lookRot;

        var velocity = transform.InverseTransformDirection(warpObj.velocity);
        velocity = Quaternion.Euler(0.0f, 180.0f, 0.0f) * velocity;
        velocity = otherPortal.transform.TransformDirection(velocity);

        warpObj.velocity = velocity;
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

    private void PlacePortal(Vector3 pos, Vector3 hitNormal, Vector3 up)
    {
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(hitNormal, up);
        transform.position -= transform.forward * 0.001f;
    }
}
