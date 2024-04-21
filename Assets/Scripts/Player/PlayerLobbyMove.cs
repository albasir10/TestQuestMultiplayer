using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerLobbyMove : NetworkBehaviour
{


    public override void OnStartServer()
    {
        base.OnStartServer();
        transform.position = NetworkManager.startPositions[0].position;
    }

    public override void OnStartLocalPlayer()
    {
        
        base.OnStartLocalPlayer();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }

    [Client]
    private void Update()
    {
        
        if (isLocalPlayer && NetworkClient.ready)
        {
            Move(new Vector3(PlayerInputs.singleton.GetHorizontalMove(), 0, PlayerInputs.singleton.GetVerticalMove()));
        }

    }

    [Command]
    private void Move(Vector3 direction)
    {

        Vector3 move = (direction.normalized * 2);

        move = transform.TransformDirection(new Vector3(move.x, 0, move.z));

        transform.position = Vector3.MoveTowards(transform.position, transform.position + move, 1 * Time.fixedDeltaTime);
    }
}
