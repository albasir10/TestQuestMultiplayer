using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIForceLevel : MonoBehaviour
{
    [SerializeField] Image progressImage;

    PlayerGun playerGun;

    public void Initialize(PlayerGun playerGun)
    {
        progressImage.fillAmount = 0;
        this.playerGun = playerGun;
        playerGun.ChangeForceInProcentEvent.AddListener(UpdateForceUI);

    }

    private void UpdateForceUI(float valueInProcent)
    {
        progressImage.fillAmount = valueInProcent;
    }


    private void OnDestroy()
    {
        try
        {
            playerGun.ChangeForceInProcentEvent.RemoveListener(UpdateForceUI);
        }
        catch
        {

        }
    }


}
