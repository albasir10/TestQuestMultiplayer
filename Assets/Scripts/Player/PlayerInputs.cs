using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    public static PlayerInputs singleton;

    Rewired.Player player;

    [Header("Inputs")]

    string inputHorizontalRotateName = "Horizontal";

    string inputVerticalRotateName = "Vertical";

    string inputHorizontalMoveName = "HorizontalMove";

    string inputVerticalMoveName = "VerticalMove";


    string inputShootName = "Shoot";

    public void Initialize(int idPlayer)
    {

        singleton = this;

        DontDestroyOnLoad(gameObject);

        player = ReInput.players.GetPlayer(idPlayer);
    }


    #region Getters


    public float GetHorizontalRotate()
    {

        return player.GetAxis(inputHorizontalRotateName);

    }

    public float GetVerticalRotate()
    {

        return player.GetAxis(inputVerticalRotateName);

    }

    public float GetHorizontalMove()
    {

        return player.GetAxis(inputHorizontalMoveName);

    }

    public float GetVerticalMove()
    {

        return player.GetAxis(inputVerticalMoveName);

    }

    public bool GetShoot()
    {
        return player.GetButton(inputShootName);
    }

    #endregion

}
