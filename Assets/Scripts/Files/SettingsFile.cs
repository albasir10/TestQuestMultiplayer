using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsFile : DefaultFiles
{
    public static SettingsFile singleton;

    public override void Initialize(string filePath, TypePath typePath)
    {
        base.Initialize(filePath, typePath);
        singleton = this;

    }

    protected override void FirstCheckFile()
    {
        if (!File.Exists(GetPathFile()))
        {
            PlayerSettings settingsPlayer = new ();

            string json = JsonUtility.ToJson(settingsPlayer);

            File.WriteAllText(GetPathFile(), json);

        }
    }

}
