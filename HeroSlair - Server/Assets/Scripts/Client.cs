using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Client : MonoBehaviour {

	public NetworkManager manager;

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkManager>();
		manager.StartClient();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
