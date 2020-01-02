using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerController : PortalableObject
{
    [SerializeField]
    private Collider wallCollider;

    private int portalTriggerCount = 0;

    private CameraMove cameraMove;
    protected new SphereCollider collider;

    public static PlayerController instance
    {
        get;
        private set;
    }

    protected override void Awake()
    {
        base.Awake();

        cameraMove = GetComponent<CameraMove>();
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

    public override void Warp()
    {
        base.Warp();
        cameraMove.ResetTargetRotation();
    }
}
