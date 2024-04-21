using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;

public class Player : NetworkBehaviour
{

    [Header("Components Player")]

    [Header("\tRotate")]

    private PlayerRotate rotater;

    [Header("\tGun")]

    private PlayerGun playerGun;

    [SerializeField] MeshRenderer gunMesh;

    [Header("\tUI")]

    private UIForceLevel uIForceLevel;

    [Header("Player Info")]

    [SyncVar]
    public string playerName;

    [SyncVar]
    public Color playerSkin;




    [ClientRpc]
    public void Initialize()
    {

        gameObject.name = playerName;

        gunMesh.material.color = playerSkin;

        if (isLocalPlayer)
        {
            string json = File.ReadAllText(SettingsFile.singleton.GetPathFile());

            PlayerSettings settingsPlayer = JsonUtility.FromJson<PlayerSettings>(json);

            uIForceLevel = GetComponentInChildren<UIForceLevel>();

            playerGun = GetComponent<PlayerGun>();

            rotater = GetComponent<PlayerRotate>();

            Cursor.visible = false;

            GetComponentInChildren<Canvas>().enabled = true;

            GetComponentInChildren<Camera>().enabled = true;

            GetComponentInChildren<AudioListener>().enabled = true;

            GetComponentInChildren<HDAdditionalCameraData>().enabled = true;

            uIForceLevel.enabled = true;

            playerGun.enabled = true;

            rotater.enabled = true;

            uIForceLevel.Initialize(playerGun);

            rotater.Initialize(settingsPlayer);

            playerGun.Initialize();

            

        }
        else
            enabled = false;

    }

}
