using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : NetworkBehaviour
{


    public static Score singleton;

    //public readonly SyncDictionary<>


    public override void OnStartClient()
    {
        base.OnStartClient();
        singleton = this;
        
    }




}
