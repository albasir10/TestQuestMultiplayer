using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFindServer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI tmpNameServer;
    [SerializeField] Button btn;

    string ip;

    public void Initialize(string name, string ip)
    {
        tmpNameServer.SetText(name);
        this.ip = ip;
    }

    public string GetIP()
    {
        return ip;
    }

    public Button GetButton()
    {
        return btn;
    }

}
