using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerMovement : NetworkBehaviour
{
    public float speed;
    public MessageSender blah;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, speed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -speed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-speed, 0, 0);

        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed, 0, 0);
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
