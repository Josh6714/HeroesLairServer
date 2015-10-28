using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelObject : MessageBase {
    public List<LevelObjects> Whatever = new List<LevelObjects>();
}

[Serializable]

public class LevelObjects
{
    public string tag;
    public float x;
    public float y;
    public float rotation;
    public LevelObjects(string n, float xx, float xy, float r)
    {
        tag = n;
        x = xx;
        y = xy;
        rotation = r;
    }
}