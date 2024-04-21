using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Ball : NetworkBehaviour
{

    PlayerScore playerScoreOwnerBall;
    Rigidbody rb;
    PlayerGun playerGun;
   

    [Server]
    public void Initialize(PlayerGun playerGun, float force)
    {

        this.playerGun = playerGun;

        rb = GetComponent<Rigidbody>();

        playerScoreOwnerBall = connectionToClient.identity.GetComponent<PlayerScore>();

        ForceBall(force);
    }

    [Server]
    public void Respawn(Transform newTransform, float force)
    {


        transform.SetPositionAndRotation(newTransform.position, newTransform.rotation);


        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.velocity = Vector3.zero;


        ForceBall(force);

    }

    [Server]
    private void ForceBall(float force)
    {
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isServer && other.TryGetComponent(out GatesTriggerGoal gatesTriggerGoal))
        {
            HitGate(other);
        }
    }

    [Server]
    private void HitGate(Collider other)
    {
        Gates gates = other.GetComponentInParent<Gates>();

        PlayerScore playerOwnerGate = gates.netIdentity.connectionToClient.identity.GetComponent<PlayerScore>();

        if (netIdentity.connectionToClient != gates.netIdentity.connectionToClient)
            playerScoreOwnerBall.ChangeScore(true);

        playerOwnerGate.ChangeScore(false);

        playerGun.RemoveBall(gameObject);

        transform.position = new Vector3(999, 999, 999);

        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        rb.isKinematic = true;
    }



}
