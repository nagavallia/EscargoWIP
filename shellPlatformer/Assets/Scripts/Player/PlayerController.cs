using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D myRigidbody;

	[SerializeField]
	private float maxSpeed;
	private float normAcc;
	private float backAcc;
	private float jumpAcc;
	private float gravity;
	private float maxFallSpeed;

	private bool facingRight;
	private bool died = false;

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private LayerMask whatIsGround;

	private bool isGrounded = false;

	private int horizontal;
	private bool jump;
	private bool pouring;

	private bool canDoubleJump;

	private bool didJump = false;
	private bool didDoubleJump = false;
	private int jumpTimer = 0;
	private int jumpCount = 0;


	[SerializeField]
	private float jumpForce;

	[SerializeField] 
	private float maxDrag;

	// Initialize the animator component
	private Animator anim;

	[SerializeField] private AudioClip jumpSound, deathSound;
	private AudioSource audioSource;

	private Vector3 deathPosition;
	private Vector3 exitPosition;

	public bool shellThrowing = false;
	public bool exitAnimation = false;

	//shell pickup stuff
	private float timestamp;
	private GameObject lastPickedUp;

	private bool processMove;
	private bool processJump;

	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> (); // get the animator component
		facingRight = false;

		myRigidbody.gravityScale = 0; // on my computer, 0.0165 is the average seconds/frame
		normAcc = .021875f * maxSpeed / 0.0165f; // we need to divide by that number to convert accelerations from units/seconds*frame to units/seconds^2
		backAcc = .075f * maxSpeed / 0.0165f; // later, we multiply by Time.fixedDeltaTime to convert back to units/seconds*frame
		jumpAcc = 1.2f * maxSpeed;
		gravity = -.035f * maxSpeed / 0.0165f;
		maxFallSpeed = -1.725f * maxSpeed;

		processMove = false;
		processJump = false;

		audioSource = gameObject.AddComponent<AudioSource> ();

		Debug.Log (maxSpeed);
		Debug.Log (backAcc);

	}

	void Update ()
	{
		HandleInput (); 

		if (this.transform.Find ("Shell") == null) {
			canDoubleJump = true;
		} else {
			canDoubleJump = false;
		}

		if (died) {
			transform.position = deathPosition;
		} else if (exitAnimation) {
			transform.position = exitPosition;
		} 
			
		if (isGrounded && !shellThrowing && !exitAnimation && !died) {
			anim.SetInteger ("State", 0);
		}

		if (didJump)
			jumpTimer++;
	}

	void FixedUpdate ()
	{
		move ();
		Flip ();
	}

	/*
	void FixedUpdate ()
	{

		HandleInput ();

		if (this.transform.Find ("Shell") == null) {
			canDoubleJump = true;
		} else {
			canDoubleJump = false;
		}

//		isGrounded = IsGrounded ();

		// Freeze the player when dead
		if (died) {
			transform.position = deathPosition;
		} else if (exitAnimation) {
			transform.position = exitPosition;
		} else {
			move ();
			Flip ();
		}

		// change back to idle if isGrounded 
		if (isGrounded && !shellThrowing && !exitAnimation && !died) {
			anim.SetInteger ("State", 0);
		}
	}*/

	// Move procedure handles player movement, jump, and double jump
	private void move ()
	{
		float xAcc = 0f;
		float yAcc = gravity * Time.fixedDeltaTime;

		if (processMove) {
			//If the player continues to move forward, they will accelerate normally.
			//If the player tries to change directions, they will decelerate at a greater rate
			if (horizontal == 1 && myRigidbody.velocity.x >= 0) {
				xAcc = normAcc;
			} else if (horizontal == -1 && myRigidbody.velocity.x <= 0) {
				xAcc = -normAcc;
			} else if (horizontal == 1 && myRigidbody.velocity.x < 0) {
				xAcc = backAcc;
			} else if (horizontal == -1 && myRigidbody.velocity.x > 0) {
				xAcc = -backAcc;
			}
			xAcc *= Time.fixedDeltaTime; // convert to units/seconds*frame

			processMove = false;
		}

		//friction
		if (isGrounded && horizontal == 0) {
//			Debug.Log (myRigidbody.velocity.x);
//			Debug.Log (backAcc);
			if (myRigidbody.velocity.x > 0) {
				xAcc = -backAcc * Time.fixedDeltaTime;
				if (myRigidbody.velocity.x < backAcc * Time.fixedDeltaTime) {
					xAcc = -myRigidbody.velocity.x; 
				}
			} else {
				xAcc = backAcc * Time.fixedDeltaTime;
				if (myRigidbody.velocity.x > -backAcc * Time.fixedDeltaTime) {
					xAcc = -myRigidbody.velocity.x; 
				}
			}
		}
			

		if (processJump || didDoubleJump) {
			if (isGrounded) {
				//jumping
				didJump = true;
				yAcc = jumpAcc;
				audioSource.PlayOneShot (jumpSound);

				// play the jump animation
				//anim.SetInteger ("State", 4);

				// log that a jump has occurred and the position of the player
				Managers.logging.RecordEvent (0, "" + gameObject.transform.position);

			} 
		//Not on the ground
		else if (jumpCount == 0) {
				Debug.Log ("jumpTimer is " + jumpTimer);
				if (canDoubleJump) {
					didDoubleJump = true;
				}
				if (didDoubleJump && (!didJump || jumpTimer >= 1)) {
					jumpCount++;
					myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
					yAcc = .85f * jumpAcc;
					audioSource.PlayOneShot (jumpSound);

					// play the jump animation
					anim.SetInteger ("State", 4);

					// log that a double jump has occurred and the position of the player
					Managers.logging.RecordEvent (1, "" + gameObject.transform.position);
				}
			}

			processJump = false;
		}

		accelerate (xAcc, yAcc);

		// Walk Animation
		//anim.SetInteger("State", 1);
	}

	private void accelerate (float xAcc, float yAcc)
	{
		//apply accelerations
		myRigidbody.velocity = myRigidbody.velocity + new Vector2 (xAcc, yAcc);

		//Enforce max horizontal velocity. 
		if (myRigidbody.velocity.x > maxSpeed) {
			myRigidbody.velocity = new Vector2 (maxSpeed, myRigidbody.velocity.y);
		} else if (myRigidbody.velocity.x < -maxSpeed) {
			myRigidbody.velocity = new Vector2 (-maxSpeed, myRigidbody.velocity.y);
		}

		//Enforce max downward velocity. 
		if (myRigidbody.velocity.y < maxFallSpeed) {
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, maxFallSpeed);
		}

		// Walk Animation
		//anim.SetInteger("State", 1);
	}

	public bool CanPickupShell (GameObject shell)
	{
		return (Time.time >= timestamp || shell == lastPickedUp) && shell.layer != LayerMask.NameToLayer ("No Collision");
	}

	public void ReleaseShell (GameObject shell)
	{
		lastPickedUp = shell;
		timestamp = Time.time + 0.1f;
	}

	// Sets the jump boolean and the drag value depending on key inputs
	private void HandleInput ()
	{
		bool left = Input.GetButton ("Left");
		bool right = Input.GetButton ("Right");

		if ((left && right) || (!left && !right)) {
			if (!processMove)
				horizontal = 0;
		} else if (left) {
			horizontal = -1;
			processMove = true;
		} else if (right) {
			horizontal = 1;
			processMove = true;
		} 

		jump = Input.GetButtonDown ("Jump");
		if (jump)
			processJump = true;
		if (pouring) {
			pour ();
		} else {
			pouring = Input.GetButtonDown ("Pour");
		}



	}

	// Flip the snail image to reflect facing location
	private void Flip ()
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;

			theScale.x *= -1;

			transform.localScale = theScale;
		}

	}

	private void groundCheck (bool grounded)
	{
		isGrounded = grounded;
		if (grounded) {
			jumpCount = 0;
			didJump = false;
			didDoubleJump = false;
			jumpTimer = 0;
		}
	}

	void Kill ()
	{
		deathPosition = transform.position;
		audioSource.PlayOneShot (deathSound);
		StartCoroutine (killRoutine ());
	}

	private void FillShell ()
	{
		if (transform.Find ("Shell") != null) {
			Shell shell = this.transform.Find ("Shell").gameObject.GetComponent<Shell> ();

			if (shell != null) {
				shell.FillShell ();
			}
		}
	}

	private void pour ()
	{
		if (transform.Find ("Shell") != null) {
			Shell shell = this.transform.Find ("Shell").gameObject.GetComponent<Shell> ();
			if (shell.isFull ()) {
				Vector3 curPosition = this.transform.position;
				Vector3 offset = facingRight ? new Vector3 (1f, 0f, 0f) : new Vector3 (-1f, 0f, 0f);
				GameObject water = (GameObject)Instantiate (Resources.Load ("waterParticle"), curPosition + offset, Random.rotation);
				water.transform.rotation = new Quaternion (0, 0, water.transform.rotation.z, water.transform.rotation.w);
				water.GetComponent<Rigidbody2D> ().AddForce (offset);
				Instantiate (water, curPosition + offset + new Vector3 (-.1f, .1f, 0), Random.rotation);
				Instantiate (water, curPosition + offset + new Vector3 (.1f, -.1f, 0), Random.rotation);
				shell.waterLevel--;
			} else {
				pouring = false;
				shell.SendMessage ("EmptyShell");
			}
		}
	}

	IEnumerator killRoutine ()
	{

		// Turn on the died flag to prevent player movement
		died = true; 

		// Death Animation before delay to reset level
		anim.SetInteger ("State", 3);

		yield return new WaitForSeconds (1);

		Messenger.Broadcast (GameEvent.RELOAD_LEVEL);

		// log that a death has taken place and the position of the player
		Managers.logging.RecordEvent (3, "" + gameObject.transform.position);

		// reset anim state to 0
		anim.SetInteger ("State", 0);

	}

	IEnumerator jumpRoutine ()
	{

		anim.SetInteger ("State", 4);
	
		yield return new WaitForSeconds (0.5f);

		anim.SetInteger ("State", 0);
	}

	public void hasExited (Vector3 exitPos)
	{
		exitPosition = exitPos;
	}

		
}