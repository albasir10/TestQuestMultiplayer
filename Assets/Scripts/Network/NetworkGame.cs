using Mirror;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class NetworkGame : NetworkRoomManager
{
    [Header("Main")]

    public static new NetworkGame singleton;

    [Header("Discovery")]

    readonly Dictionary<long, ServerResponseGame> discoveredServers = new ();

    NetworkDiscoveryGame networkDiscovery;

    [Header("Settings Server")]

    public string serverName = "server";

    [Header("Settings Player")]

    public string playerName = "player";

    [Header("Other Prefabs")]

    [SerializeField] GameObject playerLobby;

    [SerializeField] GameObject gatesPrefab;

    [Header("Room")]

    List<PlayerLobbyReady> roomPlayers = new();

    [Header("Game")]

    [Header("\tObjects")]

    [Header("\t\tGates")]

    Dictionary<Transform, GameObject> infoGatesPlayers = new();

    [SerializeField] Vector3 spawnLocalPositionGates = new(0, -1.8f, 0);

    [Header("Info Server")]

    int countLoadedPlayers = 0;

    #region Awake
    public override void Awake()
    {
        base.Awake();

        singleton = this;

        networkDiscovery = GetComponent<NetworkDiscoveryGame>();

        networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);

    }

    #endregion Awake


    #region Host

    public new void StartHost()
    {
        base.StartHost();
        discoveredServers.Clear();
        networkDiscovery.AdvertiseServer();
    }

    #endregion Host

    #region SearchServer

    public void SearchServers()
    {
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    public void OnDiscoveredServer(ServerResponseGame info)
    {
        discoveredServers[info.serverId] = info;
    }

    public ServerResponseGame[] GetServers()
    {

        List<ServerResponseGame> serversInfo = new (discoveredServers.Count);

        foreach (ServerResponseGame info in discoveredServers.Values)
        {
            serversInfo.Add(info);
        }

        return serversInfo.ToArray();

    }

    public void Connect(ServerResponseGame info)
    {
        networkDiscovery.StopDiscovery();
        StartClient(info.uri);
    }

    #endregion SearchServer

    #region ChangeServerInfo

    public void ChangeHostName(string newValue)
    {

        serverName = newValue;

    }

    #endregion ChangeServerInfo

    #region ChangePlayerInfo

    public void ChangePlayerName(string newValue)
    {

        playerName = newValue;

    }

    #endregion ChangePlayerInfo



    #region InRoom

    public override void OnRoomStartHost()
    {
        base.OnRoomStartHost();

        NetworkServer.RegisterHandler<CreatePlayerLobbyMessage>(CreatePlayerLobby);

    }

    void CreatePlayerLobby(NetworkConnectionToClient conn, CreatePlayerLobbyMessage message)
    {
        GameObject playerLobbyObj = Instantiate(playerLobby);

        PlayerLobbyReady player = playerLobbyObj.GetComponent<PlayerLobbyReady>();

        player.playerName = message.name;

        roomPlayers.Add(player);

        NetworkServer.AddPlayerForConnection(conn, playerLobbyObj);
    }

    public override void OnRoomClientConnect()
    {
        base.OnRoomClientConnect();

        CreatePlayerLobbyMessage characterMessage = new()
        {
            name = playerName
        };

        NetworkClient.Send(characterMessage);

    }

    #endregion InRoom

    #region FromRoomToGame

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        NetworkServer.SetClientReady(conn);

        if (conn != null && conn.identity != null)
        {
            GameObject roomPlayer = conn.identity.gameObject;
            if (roomPlayer != null && roomPlayer.GetComponent<NetworkRoomPlayer>() != null)
                SceneLoadedForPlayer(conn, roomPlayer);
        }
    }

 
    void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {

        if (Utils.IsSceneActive(RoomScene))
        {
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            return;
        }
        else
        {
            GameObject playerGame = OnRoomServerCreateGamePlayer(conn, roomPlayer);

            if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, playerGame))
                return;

            NetworkServer.ReplacePlayerForConnection(conn, playerGame, true);

            PlayerGameSpawned();

        }

    }
    
    private void PlayerGameSpawned()
    {

        countLoadedPlayers++;

        if (countLoadedPlayers == numPlayers)
        {

            AllPlayerSpawned();
            countLoadedPlayers = 0;

        }

    }

    private void AllPlayerSpawned()
    {
        foreach (PlayerLobbyReady playerLobbyReady in roomPlayers)
        {
            playerLobbyReady.Deactivate();

        }

        foreach (KeyValuePair<Transform, GameObject> infoGatesPlayer in infoGatesPlayers)
        {

            Player player = infoGatesPlayer.Value.GetComponent<Player>();

            player.Initialize();

            GameObject gateObject = Instantiate(gatesPrefab, infoGatesPlayer.Key);

            gateObject.transform.localPosition = spawnLocalPositionGates;

            gateObject.transform.localRotation = Quaternion.identity;

            NetworkServer.Spawn(gateObject, infoGatesPlayer.Value);

            Gates gates = gateObject.GetComponent<Gates>();

            gates.Initialize(player.playerSkin);
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {

        GameObject gamePlayer =  Instantiate(playerPrefab);

        return gamePlayer;
    }


    
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {

        try
        {

            Transform startPos = GetStartPosition();

            gamePlayer.transform.SetPositionAndRotation(startPos.position, startPos.rotation);

            Player player = gamePlayer.GetComponent<Player>();

            player.playerName = roomPlayer.name;

            player.playerSkin = roomPlayer.GetComponent<PlayerLobbySettings>().GetSaveColor();

            infoGatesPlayers.Add(startPos, gamePlayer);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        
    }

    #endregion FromRoomToGame

}
