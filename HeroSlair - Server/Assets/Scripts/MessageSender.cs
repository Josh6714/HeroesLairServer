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
	public InputField input;
	public Text chatText;

	private Vector3 exitObject;
	
	public int chatCount;
	
	public void Start()
	{
		Init(myManager.client);
		
		chatText.text = "New Chat Started";
		chatCount = 0;
	}
	
	public void Init(NetworkClient client)
	{
		myClient = client;
		NetworkServer.RegisterHandler(clientMsgType, OnServerReadyToBeginMessage);
		NetworkServer.RegisterHandler(serverMsgType, OnServerReadyToBeginMessage);
		
		if (myClient != null) {
			myClient.RegisterHandler (serverMsgType, OnServerReadyToBeginMessage);
			myClient.RegisterHandler (clientMsgType, OnServerReadyToBeginMessage);
		}
	}
	
	public void SendReadyToBeginMessage(int myId)
	{
		if (input.text != "")
		{
			var msg = new LevelMessage();
			//msg.width = 10;
			//msg.height = 20;
			msg.message = input.text;
			myClient.Send(clientMsgType, msg);
			
			//Clears new chat text
			if(chatText.text.CompareTo("New Chat Started") == 0)
			{
				chatText.text = "";
			}
			
			//Checks if there is room on the chat
			if (chatCount == 10) {
				chatText.text = "";
			}
			
			//Checks if it needs to go down a line or not
			if(chatText.text.CompareTo("") == 0)
			{
				chatText.text += "YOU: " + msg;
			}
			else
			{
				chatText.text += "\nYOU: " + msg;
			}
			
			chatCount++;
		}
		else
		{
			Debug.Log("ERROR: No message to send");
		}
	}
	
	void OnServerReadyToBeginMessage(NetworkMessage netMsg)
	{
		var beginMessage = netMsg.ReadMessage<LevelMessage>();
		//Debug.Log("received OnServerReadyToBeginMessage " + beginMessage.message);

		//CHECKS IF THE MESSAGE RECEIVED WAS A LEVEL
		string message = beginMessage.message;
		string checker;
		//Debug.Log ("Received " + message + ". Checking if message is SENDLEVEL.");
		
		//Checks what was sent over
		if ((message.IndexOf (':') + 1) != 0) {
			checker = message.Substring (0, message.IndexOf (':'));

			//If checker is SL then it is a SENDLEVEL
			if (checker.CompareTo ("SL") == 0) {
				//Debug.Log ("MESSAGE IS SENDLEVEL");
				buildObject(message);
			}
		}

		//Else is just a chat message
		else
		{
			//Debug.Log ("MESSAGE IS CHAT");

			//Clears new chat text
			if(chatText.text.CompareTo("New Chat Started") == 0)
			{
				chatText.text = "";
			}
			
			//Checks if there is room on the chat
			if (chatCount == 10) {
				chatText.text = "";
			}
			
			//Checks if it needs to go down a line or not
			if(chatText.text.CompareTo("") == 0)
			{
				chatText.text += "Other: " + beginMessage.message;
			}
			else
			{
				chatText.text += "\nOther: " + beginMessage.message;
			}
			
			chatCount++;
		}
	}
	
	public void OnSelectInputField()
	{
		input.text = "";
	}

	/*
	 * MY CODE
	 */

	public void buildObject(string message)
	{
		Debug.Log (message);

		//If message is a trigger word
		if (message.CompareTo ("SL:End") == 0) {
			Debug.Log ("Done Building. Creating Player.");
			GameObject Player = (GameObject)Instantiate(Resources.Load("Player"));
			exitObject = GameObject.Find ("DoorEnter(Clone)").transform.position;
			//Debug.Log ("Door:" + exitObject.x +":" + exitObject.y);
			Player.transform.position = exitObject;
			//Debug.Log ("Player:" + Player.transform.position.x +":" + Player.transform.position.y);
		}

		//Else build the object
		else{
			Debug.Log ("Building Object");
			//Remove SL
			message = message.Remove (0, message.IndexOf (':')+1);

			//Grab Object name then remove that part of string
			string mObject = message.Substring (0, message.IndexOf (':'));
			message = message.Remove (0, message.IndexOf (':')+1);

			//Grab X Coordinate, then Y
			float mX = float.Parse ((message.Substring (0, message.IndexOf (':'))));
			message = message.Remove (0, message.IndexOf (':')+1);
			float mY = float.Parse (message);
			
			//Create Object
			Debug.Log ("Building " +mObject);
			GameObject levelObject = (GameObject)Instantiate(Resources.Load(mObject));
			levelObject.transform.position = new Vector3(mX,mY,0);
		}
	}
}
