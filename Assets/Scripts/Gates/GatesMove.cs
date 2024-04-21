using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesMove : NetworkBehaviour
{

    [SerializeField] float moveSpeed = 2f;

    bool isRight = true;

    GatesTriggerWalls gatesTriggerWalls;


    public override void OnStartServer()
    {
        base.OnStartServer();

        gatesTriggerWalls = GetComponentInChildren<GatesTriggerWalls>();

        gatesTriggerWalls.ChangeMoveAction += ChangeMove;

    }

    private void FixedUpdate()
    {
      if (isServer)
        {

            Vector3 move;

            if (isRight)
                move = new Vector3(moveSpeed, 0, 0);
            else
                move = new Vector3(-moveSpeed, 0, 0);

            transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + move, Time.fixedDeltaTime);
        }
    }

    [Server]
    public void ChangeMove()
    {
        isRight = !isRight;
    }

    private void OnDestroy()
    {
        if (isServer)
            gatesTriggerWalls.ChangeMoveAction -= ChangeMove;
    }

}
