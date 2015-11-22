using UnityEngine;
using System.Collections;

public class metalControl : MonoBehaviour
{

    public int status;

    public Sprite electricOff;
    public Sprite electricOn;

    // Use this for initialization
    void Start()
    {
        status = 0;

        //Testing wise, will place it into ON
        switchStatus();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Switches status when triggered
    public void switchStatus()
    {
        if (status == 0)
        {
            status = 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = electricOn;
        }
        else
        {
            status = 0;
            gameObject.GetComponent<SpriteRenderer>().sprite = electricOff;
        }
    }
}
