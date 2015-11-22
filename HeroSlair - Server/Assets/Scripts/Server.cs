using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Server : MonoBehaviour {

	public NetworkManager manager;

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkManager>();
		//manager.StartServer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Call to send the level to the clients
	public void sendLevel()
	{
		//Checks if there is clients active

		//Compile list of objects
		compileObjects ();
	}

	//Used to compile all objects in scene to send to client
	public void compileObjects()
	{
		//Gets all objects with the tag "Level"
		Debug.Log ("Getting all tagged objects");
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Level");

		/* DEBUG FOR ALLOBJECTS
		foreach (GameObject thisObject in allObjects) {
			Debug.Log ( thisObject.transform.name + ":" + thisObject.transform.position.x + ":" + thisObject.transform.position.y);
		}
		*/

		//Convert to proper list
		Debug.Log ("Converting to proper listing");
		foreach (GameObject thisObject in allObjects) {

			//Gets Objects proper name without all the duplicate issues
			string longName = thisObject.transform.name;
			string shortName;

			//If the object has "duplicate" text added to object name
			if( (longName.IndexOf(' ') + 1) != 0 )
			{
				shortName = longName.Substring(0, longName.IndexOf(' '));
			}
			else{
				shortName = longName;
			}

			//Compile it all to a single string
			string result = shortName + ":" + thisObject.transform.position.x + ":" + thisObject.transform.position.y;
			Debug.Log ( result );

			//Send result to client via message
		}

	}
}
