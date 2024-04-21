using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLobbyQuit : NetworkBehaviour
{

    [Client]
    public void QuitLobby()
    {

        if (isServer)
        {
            NetworkGame.singleton.StopHost();
        }
        else
        {
            NetworkGame.singleton.StopClient();
        }

    }

}
