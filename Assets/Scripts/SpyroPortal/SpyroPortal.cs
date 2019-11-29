using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyroPortal : MonoBehaviour
{
    [SerializeField]
    private Rigidbody player;

    [SerializeField]
    private Material[] portalMaterials = new Material[2];

    private bool isInPortal = false;
    private Vector3 lastPlayerPos;

    private new MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // If the player is in the portal, see if we've crossed the boundary.
    private void Update()
    {
        if(isInPortal)
        {
            Vector3 playerPos = transform.InverseTransformPoint(player.position);

            if (Mathf.Sign(playerPos.z) != Mathf.Sign(lastPlayerPos.z))
            {
                EnterPortal();
            }

            lastPlayerPos = playerPos;
        }
    }

    // Swap the materials on the portal surface and skybox.
    // Then swap array entries ready for next time.
    private void EnterPortal()
    {
        var oldMaterial = portalMaterials[0];
        var newMaterial = portalMaterials[1];

        renderer.material = newMaterial;
        SpyroCamera.instance.SetSkybox(oldMaterial);

        portalMaterials[0] = newMaterial;
        portalMaterials[1] = oldMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        // The player is inside the portal.
        if(other.tag == "Player")
        {
            isInPortal = true;
            lastPlayerPos = transform.InverseTransformPoint(player.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // The player is not inside the portal.
        if(other.tag == "Player")
        {
            isInPortal = false;
        }
    }
}
