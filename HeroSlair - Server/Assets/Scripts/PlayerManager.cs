using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public float speed = 10f;
	public float jumpPower = 4.5f;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;

	private bool doubleJumped;
	private bool canGravity;
	private bool canTele;
	private bool canTele2;

	public Vector3 teleportTo;
	public Vector3 teleportTo2;

	//private Animator anim;

	void Start ()
	{
		//anim = GetComponent<Animator> ();
		canGravity = false;
		canTele = false;
		//teleportTo = GameObject.Find ("teleportTo").transform.position;
		//teleportTo2 = GameObject.Find ("teleportTo2").transform.position;
	}

	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (grounded) {
			doubleJumped = false;
		}

		//anim.SetBool ("Grounded", grounded);

		if (Input.GetKeyDown (KeyCode.Space) && grounded) {
			//GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpPower);
			Jump();
		}

		if (Input.GetKeyDown (KeyCode.Space) && !doubleJumped && !grounded) {
			//GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpPower);
			Jump ();
			doubleJumped = true;
		}

		if (Input.GetKey (KeyCode.D)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (speed, GetComponent<Rigidbody2D> ().velocity.y);
		}

		if (Input.GetKey (KeyCode.A)) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-speed, GetComponent<Rigidbody2D> ().velocity.y);
		}

		if(Input.GetKeyDown(KeyCode.Escape) == true){
			Application.Quit();
		}
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		//If collides with stairs. Move onto next mission
		if (other.tag == "Exit") {
			//Invoke ("Restart", restartLevelDelay);
			//enabled = false;
			Debug.Log ("Player has reached exit");
			Application.Quit ();
		}
	}

	public void Jump()
	{
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (GetComponent<Rigidbody2D> ().velocity.x, jumpPower);
	}
}