using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Collider wallCollider;

    private int portalTriggerCount = 0;

    private new SphereCollider collider;

    public static PlayerController instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        instance = this;
    }

    public float GetColliderRadius()
    {
        return collider.radius;
    }

    public void ChangePortalTriggerCount(int delta)
    {
        portalTriggerCount += delta;

        //Physics.IgnoreCollision(collider, wallCollider, (portalTriggerCount > 0));
        Physics.IgnoreLayerCollision(8, 9, (portalTriggerCount > 0));
    }
}
