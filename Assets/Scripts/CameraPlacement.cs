using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
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
        //Physics.Raycast(transform.position, transform.forward, )
    }
}
