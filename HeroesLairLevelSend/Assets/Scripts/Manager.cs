using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {
    public bool ReadySend = false;
    public List<GameObject> LevelObjects = new List<GameObject>();
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
                    LevelObjects.Add(obj);
                }
            }
            ReadySend = true;
        }
    }
}
