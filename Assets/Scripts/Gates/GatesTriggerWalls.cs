using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesTriggerWalls : MonoBehaviour
{
    public Action ChangeMoveAction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
            ChangeMoveAction?.Invoke();

    }
}
