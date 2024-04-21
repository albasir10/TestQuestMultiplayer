using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIFindServers : MonoBehaviour
{
    [SerializeField] GameObject findServerUIPrefab;

    List<UIFindServer> findServersUI = new();

    ServerResponseGame[] serversInfo;

    public async void ViewServers()
    {

        NetworkGame.singleton.SearchServers();

        serversInfo = NetworkGame.singleton.GetServers();

        if (serversInfo.Length == 0 && findServersUI.Count > 0)
            await DeleteAllFindServersAsync();

        while (serversInfo.Length == 0)
        {
            serversInfo = NetworkGame.singleton.GetServers();
            await Task.Yield();
        }



        DeleteOldFindServers();

 


        foreach (ServerResponseGame serverInfo in serversInfo)
        {

            bool isHaveServer = false;

            foreach (UIFindServer findServer in findServersUI)
            {
                if (serverInfo.EndPoint.Address.ToString() == findServer.GetIP())
                {

                    isHaveServer = true;
                    break;

                }

            }

            if (!isHaveServer)
            {
                UIFindServer findServer =  Instantiate(findServerUIPrefab, transform).GetComponent<UIFindServer>();

                findServer.Initialize(serverInfo.serverName, serverInfo.EndPoint.Address.ToString());

                findServer.GetButton().onClick.AddListener(() => Connect(serverInfo));

                findServersUI.Add(findServer);
            }

        }

    }

    public async void DeleteOldFindServers()
    {

        List<UIFindServer> needDelete = new();

        foreach (UIFindServer findServer in findServersUI)
        {

            bool isServerActive = false;

            foreach (ServerResponseGame serverInfo in serversInfo)
            {

                if (serverInfo.EndPoint.Address.ToString() == findServer.GetIP())
                {
                    isServerActive = true;
                    break;
                }
            }

            if (!isServerActive)
            {
                needDelete.Add(findServer);
            }

        }

        while(needDelete.Count > 0)
        {
            findServersUI.Remove(needDelete[0]);
            needDelete[0].GetButton().onClick.RemoveAllListeners();
            Destroy(needDelete[0].gameObject);
            await Task.Yield();
        }


    }



    public async Task DeleteAllFindServersAsync()
    {


        while (findServersUI.Count > 0)
        {
            UIFindServer findServer = findServersUI[0];
            findServersUI.Remove(findServer);
            findServer.GetButton().onClick.RemoveAllListeners();
            Destroy(findServer.gameObject);
            await Task.Yield();
        }

    }

    public void DeleteAllFindServers()
    {
        while (findServersUI.Count > 0)
        {
            UIFindServer findServer = findServersUI[0];
            findServersUI.Remove(findServer);
            findServer.GetButton().onClick.RemoveAllListeners();
            Destroy(findServer.gameObject);
        }
    }


    public void Connect(ServerResponseGame serverInfo)
    {
        NetworkGame.singleton.Connect(serverInfo);
    }
}
