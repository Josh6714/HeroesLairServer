using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetManager : NetworkManager {

    int connectionId = -1;
    const short clientMsgType = 1002;
    const short serverMsgType = 1003;
    const short tagMsgType = 1004;
    const short locationXMsgType = 1005;
    const short locationYMsgType = 1006;
    const short locationZMsgType = 1007;

    const short scaleXMsgType = 1008;
    const short scaleYMsgType = 1009;
    const short scaleZMsgType = 1010;

    const short spawnMsgType = 1011;
    const short doneMsgType = 1012;
    const short levelMsgType = 1013;
    short MsgType;

    NetworkPlayer player1;
    public static LevelMessage msg = new LevelMessage();
    LevelObject levelMessage = new LevelObject();
    public Manager gManage;
    public void Start()
    {
        SetUpServer();
        NetworkServer.RegisterHandler(serverMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(clientMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(tagMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(doneMsgType, OnClientReadyToBeginMessage);


        NetworkServer.RegisterHandler(locationXMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(locationYMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(locationZMsgType, OnClientReadyToBeginMessage);

        NetworkServer.RegisterHandler(scaleXMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(scaleYMsgType, OnClientReadyToBeginMessage);
        NetworkServer.RegisterHandler(scaleZMsgType, OnClientReadyToBeginMessage);


        msg.message = "Yo";
        MsgType = serverMsgType;
    }

    public void SendReadyToBeginMessage(int myId)
    {
        if (connectionId != 0)
        {
            Debug.Log("Attempting to send to " + connectionId);
            msg.width = 10;
            msg.height = 20;
            NetworkServer.SendToClient(connectionId, MsgType, msg);
        }
        else
        {
            Debug.Log("ERROR: Not connected to client");
        }
    }

    public void SendReadyToBeginLevel(int myId)
    {
        if(connectionId != 0)
        {
            Debug.Log("Attempting to Send Level");
            {
               // NetworkServer.SendToClient(connectionId, levelMsgType, levelMessage);
            }
        }
    }

    void OnClientReadyToBeginMessage(NetworkMessage netMsg)
    {
        var beginMessage = netMsg.ReadMessage<LevelMessage>();
        Debug.Log("received OnClientReadyToBeginMessage " + beginMessage.message);
        if(netMsg.msgType == clientMsgType)
        {
            Application.LoadLevel(1);
        }
        else if(netMsg.msgType == doneMsgType)
        {
            Application.LoadLevel(0);
        }
    }


    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        connectionId = conn.connectionId;
        Debug.Log("Client Connected: " + connectionId);
    }

    //initializes the server
    public void SetUpServer()
    {
        //Network.InitializeServer(1, 4444, true);
        //NetworkServer.Listen(4444);
        StartServer();
        Debug.Log("Player " + player1.ToString());
        Debug.Log("Is Client Active " + NetworkClient.active.ToString());
        Debug.Log("Connections " + Network.connections.Length.ToString());
    }

    //when a player connects
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player Connected" + player.ipAddress.ToString());
        player1 = player;

        Debug.Log("Player " + player1.ToString());
        Debug.Log("Is Client Active " + NetworkClient.active.ToString());
        Debug.Log("Connections " + Network.connections.Length.ToString());

        SendLevel();
        
    }

    //when a player disconnects
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Player Disconnected");
    }

    //when the server is initialized
    void OnServerInitialized()
    {
        Debug.Log("Server Initialized");
    }

    //send level
    public void SendLevel()
    {
        var msg = new LevelMessage();
        msg.width = 10;
        msg.height = 20;
        msg.message = "FHRITP";
        NetworkServer.SendToAll(serverMsgType, msg);
        Debug.Log("Sending Message: " + msg.message);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started");
    }

    void Update()
    {
        if (Manager.ReadySend == true)
        {
            //levelMessage.Whatever = gManage.blahh.Whatever;
            //for (int i = 0; i < gManage.LevelObjects.Count; i++)
            //{
            //    msg.message = gManage.LevelObjects[i].tag;
            //    MsgType = tagMsgType;
            //    SendReadyToBeginMessage(0);

            //    msg.message = gManage.LevelObjects[i].transform.position.x.ToString();
            //    MsgType = locationXMsgType;
            //    SendReadyToBeginMessage(0);

            //    msg.message = gManage.LevelObjects[i].transform.position.y.ToString();
            //    MsgType = locationYMsgType;
            //    SendReadyToBeginMessage(0);

            //    msg.message = gManage.LevelObjects[i].transform.position.z.ToString();
            //    MsgType = locationZMsgType;
            //    SendReadyToBeginMessage(0);

            //    msg.message = gManage.LevelObjects[i].transform.localScale.x.ToString();
            //    MsgType = scaleXMsgType;
            //    SendReadyToBeginMessage(0);

            //    msg.message = gManage.LevelObjects[i].transform.localScale.y.ToString();
            //    MsgType = scaleYMsgType;
            //    SendReadyToBeginMessage(0);

            //    msg.message = gManage.LevelObjects[i].transform.localScale.z.ToString();
            //    MsgType = scaleZMsgType;
            //    SendReadyToBeginMessage(0);
            //}
            Manager.ReadySend = false;
            MsgType = levelMsgType;
            SendReadyToBeginMessage(0);
            MsgType = spawnMsgType;
            msg.message = "true";
            SendReadyToBeginMessage(0);
            MsgType = serverMsgType;
        }
    }
}
