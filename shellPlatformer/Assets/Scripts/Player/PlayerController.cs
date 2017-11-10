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

    [SerializeField] private AudioClip jumpSound;
    private AudioSource audioSource;

	private Vector3 deathPosition;

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> (); // get the animator component
		facingRight = false;

		myRigidbody.gravityScale = 0;
		normAcc = .021875f * maxSpeed;
		backAcc = .075f * maxSpeed;
		jumpAcc = 1.2f * maxSpeed;
		gravity = -.035f * maxSpeed;
		maxFallSpeed = -1.725f * maxSpeed;

        audioSource = gameObject.AddComponent<AudioSource>();

	}
		
	void Update(){

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
		} else {
			move ();
			Flip ();
		}
	}

	// Move procedure handles player movement, jump, and double jump
	private void move ()
	{
		float xAcc = 0f;
		float yAcc = gravity;

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

		if (isGrounded) {
			//friction
			if (horizontal == 0) {
				if (myRigidbody.velocity.x > 0) {
					xAcc = -normAcc;
					if (myRigidbody.velocity.x < normAcc) {
						xAcc = -myRigidbody.velocity.x; 
					}
				} else {
					xAcc = normAcc;
					if (myRigidbody.velocity.x > -normAcc) {
						xAcc = -myRigidbody.velocity.x; 
					}
				}
			}

			//jumping
			if (jump) {
				didJump = true;
				yAcc = jumpAcc;
				audioSource.PlayOneShot (jumpSound);
				// log that a jump has occurred and the position of the player
				Managers.logging.RecordEvent(0, "" + gameObject.transform.position);
			}
		} 
		//Not on the ground
		else if (jumpCount == 0) {
			if (didJump) {
				jumpTimer++;
			} 
			if (jump && canDoubleJump) {
				didDoubleJump = true;
			}
			if (didDoubleJump && (!didJump || jumpTimer >= 20)) {
				jumpCount++;
				myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
				yAcc = .85f * jumpAcc;
				audioSource.PlayOneShot (jumpSound);
				// log that a double jump has occurred and the position of the player
				Managers.logging.RecordEvent(1, "" + gameObject.transform.position);
			}
		}

		accelerate(xAcc, yAcc);

		// Walk Animation
		//anim.SetInteger("State", 1);
	}

	private void accelerate (float xAcc, float yAcc) {
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

	// Sets the jump boolean and the drag value depending on key inputs
	private void HandleInput(){
		bool left = Input.GetButton("Left");
		bool right = Input.GetButton("Right");

		if ((left && right) || (!left && !right)) {
			horizontal = 0;
		} else if (left) {
			horizontal = -1;
		} else if (right) {
			horizontal = 1;
		} 

		jump = Input.GetButtonDown ("Jump");
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
		
	private void groundCheck (bool grounded) {
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
		StartCoroutine (killRoutine());
	}

	private void FillShell()
	{
        if (transform.Find("Shell") != null)
        {
            Shell shell = this.transform.Find("Shell").gameObject.GetComponent<Shell>();

            if (shell != null)
            {
                shell.FillShell();
            }
        }
	}

	private void pour() {
		if (transform.Find ("Shell") != null) {
			Shell shell = this.transform.Find ("Shell").gameObject.GetComponent<Shell> ();
			if (shell.waterLevel > 0) {
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

	IEnumerator killRoutine(){

		// Turn on the died flag to prevent player movement
		died = true; 

		// Death Animation before delay to reset level
		anim.SetInteger("State", 3);

		yield return new WaitForSeconds (1);

		Messenger.Broadcast(GameEvent.RELOAD_LEVEL);

		// log that a death has taken place and the position of the player
		Managers.logging.RecordEvent(3, "" + gameObject.transform.position);

		// reset anim state to 0
		anim.SetInteger("State", 0);

	}
		
}