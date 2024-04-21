using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDataFiles : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
