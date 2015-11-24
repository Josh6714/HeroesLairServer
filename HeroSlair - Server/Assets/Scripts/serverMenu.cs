using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Networking;

public class serverMenu : MonoBehaviour {
    public Canvas loginCanvas;
    public InputField loginUInput;
    public InputField loginPInput;
    public Text loginAlert;

    public Canvas menuCanvas;
    public GameObject mainMenuPanel;
    public GameObject lairListPanel;
    public GameObject clientListPanel;
    public GameObject economyPanel;


    public GameObject lairList;
    public GameObject clientList;

    public NetManager netManager;

	// Use this for initialization
	void Start () {

        //Create Directories if they don't exist
        if( !Directory.Exists("ServerData") )
        {
            Directory.CreateDirectory("ServerData");
        }

        if (!Directory.Exists("ServerData/Logins"))
        {
            Directory.CreateDirectory("ServerData/Logins");
        }

        if (!Directory.Exists("ServerData/Levels"))
        {
            Directory.CreateDirectory("ServerData/Levels");
        }

        //Create admin account
        tempCreateAccount();

        //Setup Canvases
        menuCanvas.gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(true);
        loginAlert.gameObject.SetActive(false);

        //Find NetworkManger script
        netManager = GameObject.Find("NetworkManager").GetComponent<NetManager>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(NetworkServer.connections.Count);
	}

    public void attemptLogin()
    {
        //Setup variables
        string username = "No Input";
        string password = "No Input";

        /*
         * Grab Server Logins
        */ 
        //Check if file exists. If so, read it and set stats
        if (File.Exists("ServerData/Logins/serverLogin.dat"))
        { //If save file exists
            //READ FILE AND SET INTEGERS CORRECTLY
            BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(Application.persistentDataPath + "/serverLogin.dat", FileMode.Open);
            FileStream file = File.Open("ServerData/Logins/serverLogin.dat", FileMode.Open);

            //Deserialize game so it can be understood
            loginData data = (loginData)bf.Deserialize(file);

            //Close file since we have loaded the file into game
            file.Close();

            //Set variables from load
            username = data.username;
            password = data.password;

            Debug.Log("Server Login Loaded: " + "ServerData/Logins/serverLogin.dat");
        }
        else
        {
            Debug.Log("ERROR LOADING SERVER LOGIN FILE FROM attemptLogin()");
        }

        /*
         * Compare logins
        */ 
        //Debug
        Debug.Log("CORRECT LOGIN: " + username + "  " + password);
        Debug.Log("INPUTTED LOGIN: " + loginUInput.text + "  " + loginPInput.text);

        //If logins are correct
        if( (username.CompareTo(loginUInput.text) == 0) && (password.CompareTo(loginPInput.text) == 0) )
        {
            Debug.Log("LOGGING IN");
            loginAlert.gameObject.SetActive(false);
            showServerMenu();
        }

        //If logins are incorrect
        else
        {
            Debug.Log("INCORRECT LOGIN IN");
            loginAlert.gameObject.SetActive(true);
            loginUInput.text = "";
            loginPInput.text = "";
        }
    }

    private void tempCreateAccount()
    {
        //Setup Save File Writer
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/serverLogin.dat");
        FileStream file = File.Create("ServerData/Logins" +"/serverLogin.dat");

        //Create Save File
        loginData data = new loginData();
        data.username = "administrator";
        data.password = "password";

        //Serialize data and save, then closes file
        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Created Admin Account: " + "ServerData/Logins/serverLogin.dat");
    }

    public void showServerMenu()
    {
        loginCanvas.gameObject.SetActive(false); //Hide login canvas
        mainMenuPanel.SetActive(true); //Show main menu panel within main menu canvas
        lairListPanel.SetActive(false); //Hide lair list panel within main menu canvas
        clientListPanel.SetActive(false); //Hide client list panel within main menu canvas
        economyPanel.SetActive(false);
        menuCanvas.gameObject.SetActive(true); //Show main menu canvas now that everything is setup
    }

    /*
     * Lair Panel Functions
    */ 

    public void viewLairs()
    {
        mainMenuPanel.SetActive(false);
        lairListPanel.SetActive(true);

        //Setup variables
        levelList data = null;

        //Delete all previous lairs if possible
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("LairList");
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }

        //Load server list
        //Check if file exists. If so, read it and set stats
        if (File.Exists("ServerData/Levels/levelList.dat"))
        { //If save file exists
            //READ FILE AND SET INTEGERS CORRECTLY
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open("ServerData/Levels/levelList.dat", FileMode.Open);

            //Deserialize game so it can be understood
            data = (levelList)bf.Deserialize(file);

            //Close file since we have loaded the file into game
            file.Close();

            Debug.Log("Server Level List Loaded: " + "ServerData/Levels/levelList.dat");
        }
        else
        {
            Debug.Log("ERROR LOADING LEVEL LIST FILE FROM viewLairs()");
            return;
        }

        //Build the Lair List into the panel
        for(int x = 0; x < data.levelName.Count; x++)
        {
            //Create a client gameobject
            GameObject addLevel = buildLair(data.levelName[x], data.levelBestTime[x], data.levelRating[x]);

            //Add to the listing
            addLevel.transform.parent = lairList.transform;
        }
    }

    private GameObject buildLair(string levelName, string bestTime, float rating )
    {
        GameObject lair = Instantiate(Resources.Load("Lair") as GameObject);

        Text lairTitle = lair.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text lairBestTime = lair.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        Text lairRating = lair.gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();

        lairTitle.text = levelName;
        lairBestTime.text = bestTime;
        lairRating.text = rating.ToString();

        return lair;
    }

    /*
     * Client Panel Functions
    */ 

    public void viewClients()
    {
        mainMenuPanel.SetActive(false);
        clientListPanel.SetActive(true);

        //Delete previous client list if any
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ClientList");
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }

        //Build the client list after showing menu
        buildClientList();
    }

    private void buildClientList()
    {
        //If there is no clients conected
        /*
        if (NetworkServer.connections.Count > 0)
        {
            Debug.Log("No Clients connected to build list");
            return;
        }
         * */

        //else, Build the client list
            Debug.Log("Building client list");
            //for (int x = 1; x < NetworkServer.connections.Count; x++)
            for (int x = 0; x < netManager.nameList.Count; x++)
            {
                string tempUser = "User " + x; //Give a default username for now. DO AN ARRAY OF NAMES ONCE LOGGED IN

                //Create a client gameobject
                //GameObject addClient = buildClient(tempUser, Network.connections[x].ipAddress, x);
                string name = netManager.nameList[x];
                string ip = netManager.ipList[x];
                int connID = netManager.idList[x];
                
                GameObject addClient = buildClient(name, ip, connID);

                //Add to the listing
                addClient.transform.parent = clientList.transform;
            }
    }

    private GameObject buildClient(string username, string IP, int connection)
    {
        GameObject client = Instantiate(Resources.Load("ConnectedClient") as GameObject);

        Text usernameText = client.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        Text IPText = client.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        Text connectionID = client.gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();

        usernameText.text = username;
        IPText.text = IP;
        connectionID.text = connection.ToString();

        return client;
    }

    /*
     * Client Panel Functions
    */
    public void showEconomy()
    {
        economyPanel.SetActive(true);
    }


    /*
     * Other Functions
    */

    public void LogoutGame()
    {
        //Setup Canvases
        menuCanvas.gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(true);
        loginAlert.gameObject.SetActive(false);

        //Clear Login Input
        loginUInput.text = "";
        loginPInput.text = "";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

[Serializable]
class loginData
{
    public string username;
    public string password;
}

[Serializable]
class levelList
{
    public List<string> levelName = new List<string>();
    public List<string> levelBestTime = new List<string>();
    public List<float> levelRating = new List<float>();
}