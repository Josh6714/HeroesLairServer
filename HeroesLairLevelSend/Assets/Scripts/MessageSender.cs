using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MessageSender : NetworkBehaviour
{
	const short clientMsgType = 1002;
	const short serverMsgType = 1003;

	public NetworkManager myManager;
	public NetworkClient myClient;

    //LevelMessage msg = new LevelMessage();

   // bool finish = false;
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SendReadyToBeginMessage(int myId)
    {
        var msg = new LevelMessage();
        msg.width = 10;
        msg.height = 20;
        msg.message = "ture";
        myClient.Send(clientMsgType, msg);
    }

    void OnServerReadyToBeginMessage(NetworkMessage netMsg)
    {
        var beginMessage = netMsg.ReadMessage<LevelMessage>();
        Debug.Log("received OnServerReadyToBeginMessage " + beginMessage.message);
    }
}
