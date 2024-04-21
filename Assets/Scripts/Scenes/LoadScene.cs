using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    AsyncOperation asyncLoad;

    public async Task LoadSceneStartAsync(int id)
    {

        asyncLoad = SceneManager.LoadSceneAsync(id);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                await Task.Yield();
            }
        }
    }

    public void LoadSceneEnd()
    {
            asyncLoad.allowSceneActivation = true;
    }

}
