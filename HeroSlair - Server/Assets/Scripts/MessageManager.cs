using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using LgOctEngine.CoreClasses;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class MessageType
{
    // Login Message and Response
    public static short LOGIN_MSG = 1000; // Login w/username, pword
    public static short LOGIN_NEW = 1001; // New user w/pword

    // Request for data
    public static short REQUEST_LIST = 1010; // Request level list
    public static short REQUEST_LEVEL = 1011; // Request level list
    public static short REQUEST_ECONOMY = 1012; // Request economy data
    public static short REQUEST_PLAYER = 1013; // Request player data

    // Acknowledge of message
    public static short ACKNOWLEDGE = 1020; // Success/Failure Ack

    // Level Management and Responses
    public static short LEVEL_LIST = 1030; // List of levels
    public static short LEVEL_MSG = 1031;  // Individual level

    // Update Player Level Data
    public static short LEVEL_COMPLETE = 1040; // Level completed
    public static short LEVEL_FAVORITE = 1041; // Favorite a level

    // Update the Economy
    public static short ECONOMY_UPDATE = 1050; // Update the economy

    // Update the Player data
    public static short PLAYER = 1060; // Player data

    // Error message
    public static short ERROR = 1111; // Any errors
}

// Constant values passed across the network
public static class MessageValue
{
    public static short FAILURE = 0;
    public static short SUCCESS = 1;
}

// Constant values for economy non-prefabs
public static class EconomyValue
{
    public static string LEVEL_COMPLETE = "COMPLETE";
    public static string IMPROVE_STANDING = "IMPROVE";
}

// Use messages of this type to send ANY JSON formatted message
// Once you pull the string from this object, you can then
// decode it based on the message type

interface IJsonable
{
    void HandleNewObject(int connID);
}

public class JsonMessage<T> : MessageBase
{
    public string message;
}

public class LevelObject : LgJsonDictionary, IJsonable
{
    public string id { get { return GetValue<string>("id", ""); } set { SetValue<string>("id", value); } }       // The unique string identifier that corresponds to the Prefab to load
    public int row { get { return GetValue<int>("row", 0); } set { SetValue<int>("row", value); } }             // The row of the "grid" that the object occupies
    public int column { get { return GetValue<int>("column", 0); } set { SetValue<int>("column", value); } }      // The column of the "grid" that the object occupies
    public float rotation { get { return GetValue<float>("rotation", 0); } set { SetValue<float>("rotation", value); } }  // The rotation of the object, in degrees, in a clockwise manner.  A zero rotation would be "upright".
    public int status { get { return GetValue<int>("status", 0); } set { SetValue<int>("status", value); } }     // The status of the object.

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling LevelObject");
        // TODO: put code that does something with this object
    }
}

public class Level : LgJsonDictionary, IJsonable
{
    public string title { get { return GetValue<string>("title", ""); } set { SetValue<string>("title", value); } }

    public LgJsonArray<LevelObject> LevelObjectArray
    {
        get { return GetNode<LgJsonArray<LevelObject>>("Level"); }
        set { SetNode<LgJsonArray<LevelObject>>("Level", value); }
    }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Received Level. Saving to server.");

        //Check if the level is valid
        if(checkLevel())
        {
            saveLevel();
        }
    }

    private bool checkLevel()
    {
        //Verify is the level is valid for the player
        bool isValid = false;
        Debug.Log("Checking if level is valid to player");

        isValid = true; //Debug, sets to true

        Debug.Log("Level Valid = " + isValid);
        return isValid;
    }

    private void saveLevel()
    {
        /*
         * Building Level File
        */

        //Setup Save File Writer
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/serverLogin.dat");
        FileStream file = File.Create("ServerData/Levels/" + title + ".dat");

        //Serialize data and save, then closes file
        bf.Serialize(file, LevelObjectArray);
        file.Close();

        Debug.Log("Saved level to server: " + "ServerData/Levels/" + title + ".dat");

        //Rebuild the lair list
        GameObject.Find("NetManager").GetComponent<NetManager>().saveLevelList();

        /*
         * Building Level's Meta File
        */

        //FileStream file = File.Create(Application.persistentDataPath + "/serverLogin.dat");
        file = File.Create("ServerData/Meta/" + title + ".dat");

        //Serialize data and save, then closes file
        LevelMetaData data = new LevelMetaData();
        data.title = title;
        data.author = "Temp Author";
        data.time = 1;
        data.player = "Temp Player";
        data.rating = 4.5f;

        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Saved meta for level to server: " + "ServerData/Meta/" + title + ".dat");
    }
}

public class Login : LgJsonDictionary, IJsonable
{
    public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
    public string password { get { return GetValue<string>("password", ""); } set { SetValue<string>("password", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling Login from Connection" +connID +". Username = " +username +" Password = " +password);

        //Create compare string
        string login = username + "," + password;

        //Get NetManager
        NetManager net = GameObject.Find("NetManager").GetComponent<NetManager>();

        //Create list to compare string
        List<string> list = null;

        //Check if username and password received is correct
        if (File.Exists("ServerData/Logins/Clients.csv"))
        { //If save file exists
            var reader = new StreamReader(File.OpenRead("ServerData/Logins/Clients.csv"));

            //Build Client list
            list = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                list.Add(line);
            }
        }
        else
        {
            Debug.Log("ERROR LOADING LEVEL LIST FILE FROM viewLairs()");
            return;
        }

        //Check if any line is correct
        for (int x = 0; x < list.Count - 1; x++ )
        {
            if (login.CompareTo( list[x] ) == 0)
            {
                Debug.Log("LOGIN SUCCESS");

                //Send login success to player
                net.OnServerSendLoginSuccess(connID);

                //Send player their data
                sendPlayerData(connID);

                return;
            }
        }

        //Otherwise send login failure
        Debug.Log("LOGIN FAILED");
        net.OnServerSendLoginFailure(connID);
    }

    private void sendPlayerData(int conn)
    {
        //Message setup
        var msg = new JsonMessage<Player>();
        Player player = LgJsonNode.Create<Player>();

        //Create default player info for now
        player.credits = 10;
        player.played = 5;
        player.created = 5;
        player.beaten = 5;

        //Serialize message
        msg.message = player.Serialize();

        //Send message to clinet
        NetworkServer.SendToClient(conn, MessageType.ACKNOWLEDGE, msg);
    }
}

public class Acknowledgement : LgJsonDictionary, IJsonable
{
    public int ack { get { return GetValue<int>("ack", 0); } set { SetValue<int>("ack", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling Acknowledgement");
        // TODO: put code that does something with this object
    }
}

// Client requests data from the server
public class Request : LgJsonDictionary, IJsonable
{
    public string request { get { return GetValue<string>("request", ""); } set { SetValue<string>("request", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling Request");
        // TODO: put code that does something with this object
    }
}

// A list of levelMetaData objects to be displayed to user
public class LevelMetaDataList : LgJsonDictionary, IJsonable
{
    public LgJsonArray<LevelMetaData> LevelMetaDataArray
    {
        get { return GetNode<LgJsonArray<LevelMetaData>>("LevelMetaDataList"); }
        set { SetNode<LgJsonArray<LevelMetaData>>("LevelMetaDataList", value); }
    }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling Level");
        // TODO: put code that does something with this object
    }
}

// The metaData for a single level
public class LevelMetaData : LgJsonDictionary, IJsonable
{
    public string title { get { return GetValue<string>("title", ""); } set { SetValue<string>("title", value); } }
    public string author { get { return GetValue<string>("author", ""); } set { SetValue<string>("author", value); } }
    public int time { get { return GetValue<int>("time", 0); } set { SetValue<int>("time", value); } }
    public string player { get { return GetValue<string>("player", ""); } set { SetValue<string>("player", value); } }
    public float rating { get { return GetValue<float>("rating", 0); } set { SetValue<float>("rating", value); } }

    // Player-specific data
    public bool isFav { get { return GetValue<bool>("isFav", false); } set { SetValue<bool>("isFav", value); } }
    public int player_time { get { return GetValue<int>("player_time", 0); } set { SetValue<int>("player_time", value); } }
    public float player_rating { get { return GetValue<float>("player_rating", 0); } set { SetValue<float>("player_rating", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling LevelMetaData");
        // TODO: put code that does something with this object
    }
}

// The player's data for a single level used when a player completes
// a level.  Used to store player-specific data for that level
public class PlayerLevelData : LgJsonDictionary, IJsonable
{
    public string title { get { return GetValue<string>("title", ""); } set { SetValue<string>("title", value); } }
    public bool isFav { get { return GetValue<bool>("isFav", false); } set { SetValue<bool>("isFav", value); } }
    public int time { get { return GetValue<int>("time", 0); } set { SetValue<int>("time", value); } }
    public float rating { get { return GetValue<float>("rating", 0); } set { SetValue<float>("rating", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling LevelMetaData");
        // TODO: put code that does something with this object
    }
}

// Client requests data from the server
public class FavoriteLevel : LgJsonDictionary, IJsonable
{
    public string title { get { return GetValue<string>("title", ""); } set { SetValue<string>("title", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling FavoriteLevel");
        // TODO: put code that does something with this object
    }
}

// An object within the economy
public class EconomyObject : LgJsonDictionary, IJsonable
{
    public string id { get { return GetValue<string>("id", ""); } set { SetValue<string>("id", value); } }
    public int credits { get { return GetValue<int>("credits", 0); } set { SetValue<int>("credits", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling EconomyObject");
        // TODO: put code that does something with this object
    }
}

// An entire level or collection of level objects
public class Economy : LgJsonDictionary, IJsonable
{
    public LgJsonArray<EconomyObject> EconomyObjectArray
    {
        get { return GetNode<LgJsonArray<EconomyObject>>("Economy"); }
        set { SetNode<LgJsonArray<EconomyObject>>("Economy", value); }
    }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling Economy");
        // TODO: put code that does something with this object
    }
}

// Player data
public class Player : LgJsonDictionary, IJsonable
{
    public int credits { get { return GetValue<int>("credits", 0); } set { SetValue<int>("credits", value); } }
    public int played { get { return GetValue<int>("played", 0); } set { SetValue<int>("played", value); } }
    public int beaten { get { return GetValue<int>("beaten", 0); } set { SetValue<int>("beaten", value); } }
    public int created { get { return GetValue<int>("created", 0); } set { SetValue<int>("created", value); } }

    public void HandleNewObject(int connID)
    {
        Debug.Log("Handling Player");
        // TODO: put code that does something with this object
    }
}