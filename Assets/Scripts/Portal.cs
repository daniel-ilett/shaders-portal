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

    private bool isPlaced = true;
    [SerializeField]
    private Collider wallCollider;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();

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
        /*
        var diameter = PlayerController.instance.GetColliderRadius() * 3.0f;
        collider.size = new Vector3(collider.size.x - diameter,
            collider.size.y - diameter, collider.size.z);
        */

        PlacePortal(wallCollider, transform.position, transform.forward, transform.up);
        SetColour(portalColour);
    }

    private void Update()
    {
        for (int i = 0; i < portalObjects.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);

            if (objPos.z > 0.0f)
            {
                portalObjects[i].Warp();
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

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null)
        {
            portalObjects.Add(obj);
            obj.SetIsInPortal(this, otherPortal, wallCollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if(portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    private void PlacePortal(Collider wallCollider, Vector3 pos, Vector3 hitNormal, Vector3 up)
    {
        this.wallCollider = wallCollider;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(hitNormal, up);
        transform.position -= transform.forward * 0.001f;

        isPlaced = true;
    }

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        isPlaced = false;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}
