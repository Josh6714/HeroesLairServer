//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//using System;
//using System.Collections.Generic;
//using LgOctEngine.CoreClasses;



//[Serializable]
//public class LevelObject : LgJsonDictionary
//{
//    public string id { get { return GetValue<string>("id", ""); } set { SetValue<string>("id", value); } }       // The unique string identifier that corresponds to the Prefab to load
//    public float x { get { return GetValue<int>("row", 0); } set { SetValue<float>("row", value); } }        // The row of the "grid" that the object occupies
//    public float y { get { return GetValue<int>("column", 0); } set { SetValue<float>("column", value); } }      // The column of the "grid" that the object occupies
//    public float rotation { get { return GetValue<float>("rotation", 0); } set { SetValue<float>("rotation", value); } }  // The rotation of the object, in degrees, in a clockwise manner.  A zero rotation would be "upright".
//    public int status { get { return GetValue<int>("status", 0); } set { SetValue<int>("status", value); } }     // The status of the object.  If an object does not 
//}

//public class Level : LgJsonDictionary
//{
//    public LgJsonArray<LevelObject> WhateverYouWantItDoesntMatter
//    {
//        get { return GetNode<LgJsonArray<LevelObject>>("Level"); }
//        set { SetNode<LgJsonArray<LevelObject>>("Level", value); }
//    }
//}

//public class LevelObjects
//{

//}

//
//
//
#pragma warning disable 0219 // unused assignment
#pragma warning disable 0168 // assigned not used
#pragma warning disable 0414 // unused variables

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// </commonheader>

using LgOctEngine.CoreClasses;

/// <summary>
/// A simple test class for the LgBaseClass functionality. You can run this script by attaching it to an object in the scene, and hitting play.
/// </summary>
public class LevelObject : MonoBehaviour
{
    SendLevel sendLevel;
    public static bool startSend = false;
    // Use this for initialization
    void Start()
    {
        sendLevel = new SendLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if(startSend == true)
        {
            SendLevel.SimpleArrayTest();
            startSend = false;
        }
    }

    //void OnGUI()
    //{
    //    Rect buttonRect = new Rect(30, 30, 200, 45);

    //    if (GUI.Button(buttonRect, "Level Array Test"))
    //    {
    //        SendLevel.SimpleArrayTest();
    //    }
    //}

    private class SendLevel : LgBaseClass
    {
        public enum EnumExample
        {
            None,
            ValueOne,
            ValueTwo
        }
        /// <summary>
        /// A json node of various basic types.
        /// </summary>
        public class LevelObject : LgJsonDictionary
        {
            public string id { get { return GetValue<string>("id", ""); } set { SetValue<string>("id", value); } }       // The unique string identifier that corresponds to the Prefab to load
            public float row { get { return GetValue<float>("row", 0); } set { SetValue<float>("row", value); } }        // The row of the "grid" that the object occupies
            public float column { get { return GetValue<float>("column", 0); } set { SetValue<float>("column", value); } }      // The column of the "grid" that the object occupies
            public float rotation { get { return GetValue<float>("rotation", 0); } set { SetValue<float>("rotation", value); } }  // The rotation of the object, in degrees, in a clockwise manner.  A zero rotation would be "upright".
            public int status { get { return GetValue<int>("status", 0); } set { SetValue<int>("status", value); } }     // The status of the object.  If an object does not 

        }
        /// <summary>
        /// An json node that contains two arrays - one simple, one complex.
        /// </summary>
        public class Level : LgJsonDictionary
        {
            public LgJsonArray<LevelObject> LevelObjectArray
            {
                get { return GetNode<LgJsonArray<LevelObject>>("Level"); }
                set { SetNode<LgJsonArray<LevelObject>>("Level", value); }
            }
        }

        private static LevelObject CreateLevelObject()
        {
            LevelObject levelObject = LgJsonNode.Create<LevelObject>();

            levelObject.id = "PrefabName";
            levelObject.row = 8;
            levelObject.column = 10;
            levelObject.rotation = 90;
            levelObject.status = 0;

            return levelObject;
        }

        public static void SimpleArrayTest()
        {
            Level simpleArrayClass = LgJsonNode.Create<Level>();
            //for (int i = 0; i < Manager.listOfObjects.Count; i++)
            //{
            //    // Method #1 - Directly add the type to array
            //    LevelObject levelObject = CreateLevelObject();
            //    simpleArrayClass.LevelObjectArray.Add(levelObject);
            //}
            for (int i = 0; i < Manager.listOfObjects.Count; i++)
            {
                // Method #2 - Use the array to add an entry and THEN fill it out
                //LevelObject levelObject = simpleArrayClass.LevelObjectArray.AddNew();
                //levelObject.id = "PrefabName";
                //levelObject.row = i;
                //levelObject.column = i * 2;
                //levelObject.rotation = 90;
                //levelObject.status = 0;
                // No need to 'save' it, we are writing directly to it

                LevelObject levelObject = simpleArrayClass.LevelObjectArray.AddNew();
                levelObject.id = Manager.listOfObjects[i].tag;
                levelObject.row = Manager.listOfObjects[i].transform.position.x;
                levelObject.column = Manager.listOfObjects[i].transform.position.y;
                levelObject.rotation = 0;
                levelObject.status = 0;
            }
            // Serialize it
            string serialized = simpleArrayClass.Serialize();
            NetManager.msg.message = simpleArrayClass.Serialize();
            Manager.ReadySend = true;
            // Deserialize it
            Level simpleArrayClassDeserialized = LgJsonNode.CreateFromJsonString<Level>(serialized);

            // Paste the output in www.jsonlint.com to easily view and debug it!
            Debug.Log("Serialized output: " + serialized);
            Debug.Log("Deserialized output: " + simpleArrayClassDeserialized.Serialize());
        }
    }
}