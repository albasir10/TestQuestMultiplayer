using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class DefaultFiles : MonoBehaviour
{

    public enum TypePath
    {
        CurrentFolder,
        PersistentDataPath,
        DataPath,
        Full

    }

    [SerializeField] protected string filePath;
    [SerializeField] protected TypePath typePath;

    public virtual void Initialize(string filePath, TypePath typePath)
    {
        if (filePath.Length > 0)
            this.filePath = filePath;

        
        this.typePath = typePath;

        FirstCheckFile();
    }

    protected abstract void FirstCheckFile();

    public virtual string GetPathFile()
    {
        switch (typePath)
        {
            case TypePath.CurrentFolder:
                return Path.Combine(Path.GetDirectoryName(Application.dataPath), filePath);
            case TypePath.PersistentDataPath:
                return Path.Combine(Application.persistentDataPath, filePath);
            case TypePath.DataPath:
                return Path.Combine(Application.dataPath, filePath);
            case TypePath.Full:
                return filePath;
            default:
                throw new Exception("Not Have This Type");
        }

        
    }

}
