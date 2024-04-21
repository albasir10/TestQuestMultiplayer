using Mirror;
using System;
using System.Net;


public struct ServerResponseGame : NetworkMessage
{

    public string serverName;

    public IPEndPoint EndPoint { get; set; }

    public Uri uri;

    public long serverId;
}
