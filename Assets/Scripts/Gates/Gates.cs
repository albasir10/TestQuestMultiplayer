using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates : NetworkBehaviour
{

    [ClientRpc]
    public void Initialize(Color gatesSkin)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = gatesSkin;

        }
    }

}
