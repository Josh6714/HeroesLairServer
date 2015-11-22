using UnityEngine;
using System.Collections;

public class gravityWheelControl : MonoBehaviour
{

    public int status;

    public Sprite gravDown;
    public Sprite gravLeft;
    public Sprite gravUp;
    public Sprite gravRight;

    // Use this for initialization
    void Start()
    {
        status = 0;

        gameObject.GetComponent<SpriteRenderer>().sprite = gravDown;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Switches status when triggered
    public void switchStatus()
    {
        //Starts at Down and rotates clockwise

        if (status == 0) //Down
        {
            status = 1; //Left
            gameObject.GetComponent<SpriteRenderer>().sprite = gravLeft;
        }

        else if (status == 1) //Left
        {
            status = 2; //Up
            gameObject.GetComponent<SpriteRenderer>().sprite = gravUp;
        }

        else if (status == 2) //Up
        {
            status = 3; //Right
            gameObject.GetComponent<SpriteRenderer>().sprite = gravRight;
        }

        else //Right
        {
            status = 0; //Down
            gameObject.GetComponent<SpriteRenderer>().sprite = gravDown;
        }
    }
}
