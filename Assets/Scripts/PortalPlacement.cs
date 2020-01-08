using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraMove))]
public class PortalPlacement : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Crosshair crosshair;

    private CameraMove cameraMove;

    private void Awake()
    {
        cameraMove = GetComponent<CameraMove>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            PlacePortal(0);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            PlacePortal(1);
        }
    }

    private void PlacePortal(int portalID)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 250.0f, layerMask);

        if(hit.collider != null)
        {
            var cameraRotation = cameraMove.TargetRotation;

            var portalRight = cameraRotation * Vector3.right;

            if(Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            }

            var portalForward = -hit.normal;
            var portalUp = -Vector3.Cross(portalRight, portalForward);

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
            
            var placed = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);

            crosshair.SetPortalPlaced(portalID, placed);

            if(placed)
            {
                Debug.Log(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("Not placed");
            }
        }
    }
}
