using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {
    public static bool ReadySend = false;
    public static List<GameObject> listOfObjects = new List<GameObject>();
    public List<GameObject> listOfShit = new List<GameObject>();
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnLevelWasLoaded(int level)
    {
        if(level == 1)
        {
            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                if(obj.tag != "DontDestroy")
                {
                    listOfObjects.Add(obj);
                   //LevelObjects blah = new LevelObjects(obj.tag, obj.transform.position.x, obj.transform.position.y, obj.transform.rotation.z);
                   //blahh.Whatever.Add(blah);
                }
            }
            listOfShit = listOfObjects;
            LevelObject.startSend = true;
            //ReadySend = true;
        }
    }
}
