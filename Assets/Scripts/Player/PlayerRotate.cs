using Mirror;
using UnityEngine;

public class PlayerRotate : NetworkBehaviour
{

    PlayerSettings settingsPlayer;

    public void Initialize(PlayerSettings settingsPlayer)
    {
            this.settingsPlayer = settingsPlayer;
    }
    private void Update()
    {

        if (isLocalPlayer && settingsPlayer != null)
        {
            Rotate(settingsPlayer.sensitivity * Time.deltaTime, new Vector3(PlayerInputs.singleton.GetVerticalRotate(), PlayerInputs.singleton.GetHorizontalRotate(), 0));
        }

    }

    [Command]
    private void Rotate(float sens, Vector3 dir)
    {

        dir = dir.normalized;

        transform.Rotate(dir.x * sens, dir.y * sens, 0);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

    }

}
