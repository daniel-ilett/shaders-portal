using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerController : PortalableObject
{
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

    public override void Warp()
    {
        base.Warp();
        cameraMove.ResetTargetRotation();
    }
}
