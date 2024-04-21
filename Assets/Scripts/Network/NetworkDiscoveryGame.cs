using Mirror.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ServerFoundUnityEvent<TResponseType> : UnityEvent<TResponseType> { };

public class NetworkDiscoveryGame : NetworkDiscoveryBase<ServerRequest, ServerResponseGame>
{
    protected override ServerResponseGame ProcessRequest(ServerRequest request, IPEndPoint endpoint)
    {

        try
        {
            return new ServerResponseGame
            {
                serverName = NetworkGame.singleton.serverName,
                serverId = ServerId,
                uri = transport.ServerUri()
            };
        }
        catch (NotImplementedException)
        {
            Debug.LogError($"Transport {transport} does not support network discovery");
            throw;
        }
    }

    protected override void ProcessResponse(ServerResponseGame response, IPEndPoint endpoint)
    {
        response.EndPoint = endpoint;

        UriBuilder realUri = new UriBuilder(response.uri)
        {
            Host = response.EndPoint.Address.ToString()
        };
        response.uri = realUri.Uri;

        OnServerFound.Invoke(response);
    }
}
