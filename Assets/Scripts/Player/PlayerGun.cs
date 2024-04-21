using Mirror;
using Mirror.Examples.Pong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;

public class PlayerGun : NetworkBehaviour
{

    [Header("Force")]

    [SerializeField] Transform forcePoint;

    float force = 0;

    float maxForce = 50;

    float minForce = 1;

    float speedChangeForce = 10;

    bool forceChangeUp = true;

    float speed => speedChangeForce * Time.fixedDeltaTime;

    public UnityEvent<float> ChangeForceInProcentEvent = new();

    [Header("Balls")]

    [SerializeField] GameObject ballPrefab;

    [SerializeField] int maxBalls = 10;

    List<GameObject> ballsActive = new (10);

    List<GameObject> ballsInactive = new(10);

    [Client]
    public void Initialize()
    {
        if (isLocalPlayer)
        { 

            ForceNull();

            StartCoroutine(WaitStartForce());
        }

    }

    #region Force

    IEnumerator WaitStartForce()
    {



        while (true)
        {

            if (PlayerInputs.singleton.GetShoot())
                StartForce();

            yield return null;
        }

    }

    [Client]
    private void StartForce()
    {
        if (isLocalPlayer)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeForce());
        }
    }

    IEnumerator ChangeForce()
    {


        ForceNull();




        while (true)
        {

            if (!PlayerInputs.singleton.GetShoot())
            {

                EndForce();
                yield break;

            }

            ChangeForceCmd();

            yield return new WaitForFixedUpdate();
        }

    }

    [Command]
    private void ForceNull()
    {

        force = 0;
        ChangeForceTarget(force);

    }

    [Command]
    private void ChangeForceCmd()
    {
        

        if (forceChangeUp)
        {
            force += speed;

            if (force > maxForce)
            {
                force = maxForce;
                forceChangeUp = !forceChangeUp;
            }

        }
        else
        {

            force -= speed;

            if (force < minForce)
            {
                force = minForce;
                forceChangeUp = !forceChangeUp;
            }

        }

        ChangeForceTarget(force);
    }

    [TargetRpc]
    private void ChangeForceTarget(float force)
    {
        ChangeForceInProcentEvent.Invoke(force / maxForce);
    }



    [Client]
    private void EndForce()
    {

        if (isLocalPlayer)
        {

            StopAllCoroutines();

            CreateBallCmd();

            StartCoroutine(WaitStartForce());
        }

    }

    [TargetRpc]
    private void ForceNullTarget()
    {
        ForceNull();
    }

    #endregion Force

    #region Ball

    [Command]
    private void CreateBallCmd()
    {


        if (ballsActive.Count == maxBalls)
        {

            GameObject lastBall = ballsActive[0];

            ballsActive.Remove(lastBall);
            ballsActive.Add(lastBall);

            lastBall.GetComponent<Ball>().Respawn(forcePoint, force);

        }
        else if (ballsInactive.Count > 0)
        {
            GameObject lastBall = ballsInactive[0];

            ballsInactive.Remove(lastBall);
            ballsActive.Add(lastBall);

            lastBall.GetComponent<Ball>().Respawn(forcePoint, force);
        }
        else
        {

            GameObject ball = Instantiate(ballPrefab, forcePoint.position, forcePoint.rotation);

            NetworkServer.Spawn(ball, connectionToClient);

            ballsActive.Add(ball);

            ball.GetComponent<Ball>().Initialize(this, force);

        }



        ForceNullTarget();

    }

    [Server]
    public void RemoveBall(GameObject ball)
    {
        ballsActive.Remove(ball);
        ballsInactive.Add(ball);
    }

    #endregion


}
