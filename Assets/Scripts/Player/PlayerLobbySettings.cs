using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLobbySettings : NetworkBehaviour
{

    public static PlayerLobbySettings singleton;

    public Color[] colors = new Color[4] { Color.white, Color.red, Color.blue, Color.black };
    string[] colorNames = new string[4] { "white", "red", "blue", "black" };

    public UnityEvent<string> ChangeColorEvent = new();

    [SyncVar]
    public int idCurrentColor = 0;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        singleton = this;
    }

    [Client]
    public void ChangeColor()
    {
        if (isLocalPlayer)
        {
            ChangeColorCmd();
        }
    }

    [Command]
    private void ChangeColorCmd()
    {
        idCurrentColor = (idCurrentColor + 1) % colors.Length;
        ChangeColorTarget(idCurrentColor);
    }

    [TargetRpc]
    private void ChangeColorTarget(int idCurrentColor)
    {
        ChangeColorEvent.Invoke(colorNames[idCurrentColor]);
    }

    [Client]
    public  Color GetSaveColor()
    {
        return colors[idCurrentColor];
    }

}
