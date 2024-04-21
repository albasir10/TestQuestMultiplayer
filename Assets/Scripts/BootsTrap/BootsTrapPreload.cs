using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapPreload : MonoBehaviour
{

    [Header("Player")]

    [SerializeField] int idPlayer = 0;

    [Header("Scene")]

    [SerializeField] int idNextScene = 0;

    [Header("Files")]

    [Header("\tSettings")]

    [SerializeField] string settingsFileName;

    [SerializeField] DefaultFiles.TypePath typePathSettingsFile;


    private void Awake()
    {

        InitializePlayerInputs();

        CreateAllPathFiles();

        LoadScene();


    }

    private void CreateAllPathFiles()
    {

        GameObject allDataFiles = new("AllDataFiles", typeof(AllDataFiles));

        CreatePathFile("SettingsFile", typeof(SettingsFile), allDataFiles.transform, settingsFileName, typePathSettingsFile);

    }

    private void CreatePathFile(string nameObject, Type componentType, Transform parentTransform, string pathFile, DefaultFiles.TypePath typePathFile)
    {

        GameObject temp = new(nameObject, componentType);

        temp.transform.parent = parentTransform;

        temp.GetComponent<DefaultFiles>().Initialize(pathFile, typePathFile);

    }

    private void InitializePlayerInputs()
    {

        GameObject playerInputGO = new("PlayerInputs", typeof(PlayerInputs));

        playerInputGO.GetComponent<PlayerInputs>().Initialize(idPlayer);

    }

    private async void LoadScene()
    {

        GameObject loadSceneGO = new("LoadScene", typeof(LoadScene));

        LoadScene loadScene = loadSceneGO.GetComponent<LoadScene>();

        if (SceneManager.GetSceneByBuildIndex(idNextScene) == SceneManager.GetActiveScene())
        {
            throw new Exception("Error, loading Active Scene");
        }


        await loadScene.LoadSceneStartAsync(idNextScene);

        loadScene.LoadSceneEnd();

    }


}
