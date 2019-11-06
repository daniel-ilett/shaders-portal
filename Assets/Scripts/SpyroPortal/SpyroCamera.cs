using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyroCamera : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer skyboxRenderer;

    public static SpyroCamera instance
    {
        get; private set;
    }

    private void Awake()
    {
        instance = this;
    }

    public void SetSkybox(Material material)
    {
        skyboxRenderer.material = material;
    }
}
