using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLobbyReady : NetworkBehaviour
{

    NetworkRoomPlayer roomPlayer;

    [SerializeField] UnityEvent ReadyEvent = new();

    [SerializeField] UnityEvent UnReadyEvent = new();

    bool isReady = false;

    [SyncVar]
    public string playerName;

    public override void OnStartClient()
    {
        base.OnStartClient();

        roomPlayer = GetComponent<NetworkRoomPlayer>();

        gameObject.name = playerName;

        if (isLocalPlayer)
        {

            GetComponentInChildren<Canvas>().enabled = true;
            GetComponent<PlayerLobbyMove>().enabled = true;
            GetComponent<PlayerLobbyReady>().enabled = true;
            GetComponent<PlayerLobbyQuit>().enabled = true;
            GetComponent<PlayerLobbySettings>().enabled = true;

        }
        else
            enabled = false;

    }

    [Client]
    public void ReadyClient()
    {
        isReady = true;
        roomPlayer.CmdChangeReadyState(isReady);
        ReadyEvent?.Invoke();
    }


    [Client]
    public void UnReadyClient()
    {
        isReady = false;
        roomPlayer.CmdChangeReadyState(isReady);
        UnReadyEvent?.Invoke();

    }


    [Client]
    public void ChangeReady()
    {
        if (isLocalPlayer)
        {
            if (isReady)
                UnReadyClient();
            else
                ReadyClient();
        }
    }


    [ClientRpc]
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
