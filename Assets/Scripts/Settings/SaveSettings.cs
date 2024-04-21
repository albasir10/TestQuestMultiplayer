using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSettings : MonoBehaviour
{

    [SerializeField] Slider slider;

    void Start()
    {

        string json = File.ReadAllText(SettingsFile.singleton.GetPathFile());

        PlayerSettings settingsPlayer = JsonUtility.FromJson<PlayerSettings>(json);

        slider.value = settingsPlayer.sensitivity;

    }

    public void SensChange()
    {
        PlayerSettings settingsPlayer = new ();

        settingsPlayer.sensitivity = slider.value;

        string json = JsonUtility.ToJson(settingsPlayer);

        File.WriteAllText(SettingsFile.singleton.GetPathFile(), json);

    }
    
}
