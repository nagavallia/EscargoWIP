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

	private bool isGrounded;

	private bool jump;

	private bool doubleJump;

	private int jumpCount = 0;

	private bool didJump = false;
	private bool didDoubleJump = false;
	private int jumpTimer = 0;


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
		backAcc = .053f * maxSpeed;
		jumpAcc = 1.1f * maxSpeed;
		gravity = -.035f * maxSpeed;
		maxFallSpeed = -1.725f * maxSpeed;

        audioSource = gameObject.AddComponent<AudioSource>();

	}
		
	void Update(){

		HandleInput ();

		if (this.transform.Find ("Shell") == null) {
			doubleJump = true;
		} else {
			doubleJump = false;
		}

		// Idle Animation if velocities are all zero
		if (myRigidbody.velocity.x == 0 && myRigidbody.velocity.y == 0) {
			anim.SetInteger ("State", 0);
		}

		// Freeze the player when dead
		if (died) {
			transform.position = deathPosition;
		}
	}

	void FixedUpdate()
	{
        float horizontal = 0;
        bool left = Input.GetButton("Left");
        bool right = Input.GetButton("Right");

		if (left) {
			horizontal = -1;
		} else if (right) {
			horizontal = 1;
		} 

		isGrounded = IsGrounded ();

		// Prevent player movement if died
		if (!died) {
			move (horizontal);
			Flip (horizontal);
		}

		ResetValues ();
	}

	// Move procedure handles player movement, jump, and double jump
	private void move (float horizontal)
	{
		//set both accelerations to 0
		float xAcc = 0f;
		float yAcc = 0f;

		//friction
		if (isGrounded) {
			didJump = false;
			didDoubleJump = false;
			jumpTimer = 0;
			if (horizontal == 0 && myRigidbody.velocity.x > 0) {
				if (myRigidbody.velocity.x > normAcc) {
					xAcc = -normAcc;
				} else {
					xAcc = -myRigidbody.velocity.x; 
				}

			} else if (horizontal == 0 && myRigidbody.velocity.x < 0) {
				if (myRigidbody.velocity.x < normAcc) {
					xAcc = normAcc;
				} else {
					xAcc = -myRigidbody.velocity.x;
				}

			}
		}

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

		if (!isGrounded) {
			yAcc = gravity;

			if (jump && doubleJump) {
				if (jumpCount == 0) {
					jumpCount += 1;
					if (jumpTimer < 20) {
						didDoubleJump = true;
					} else {
						myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
						yAcc = .8f * jumpAcc;
						audioSource.PlayOneShot(jumpSound);
					}

					// log that a double jump has occurred and the position of the player
//					Managers.logging.RecordEvent(1, "" + gameObject.transform.position);
				}
			}
		} else if (jump) {
			isGrounded = false;
			didJump = true;
			yAcc = jumpAcc;
            audioSource.PlayOneShot(jumpSound);
			// log that a jump has occurred and the position of the player
//			Managers.logging.RecordEvent(0, "" + gameObject.transform.position);
		}

		if (didJump) {
			jumpTimer++;
			if (didDoubleJump && jumpTimer >= 20) {
				Debug.Log ("double jump thing");
				didDoubleJump = false;
				myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, 0);
				yAcc = .8f * jumpAcc;
				audioSource.PlayOneShot(jumpSound);
			}
		}


		myRigidbody.velocity = myRigidbody.velocity + new Vector2 (xAcc, yAcc);

		//Enforce max horizontal and vertical movement speeds. 
		if (myRigidbody.velocity.x > maxSpeed) {
			myRigidbody.velocity = new Vector2 (maxSpeed, myRigidbody.velocity.y);
		} else if (myRigidbody.velocity.x < -maxSpeed) {
			myRigidbody.velocity = new Vector2 (-maxSpeed, myRigidbody.velocity.y);
		}
		//(should never go too fast in the y direction
		if (myRigidbody.velocity.y < maxFallSpeed) {
			myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, maxFallSpeed);
		}

		// Walk Animation
		//anim.SetInteger("State", 1);

	}

	// Sets the jump boolean and the drag value depending on key inputs
	private void HandleInput(){
		if (Input.GetButtonDown ("Jump")) {
			jump = true;
		}
	}

	// Flip the snail image to reflect facing location
	private void Flip (float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;

			theScale.x *= -1;

			transform.localScale = theScale;
		}

	}

	// Resets jump booleans
	private void ResetValues (){
		jump = false;
		doubleJump = false;
	}

	// Determines if the snail is grounded 
	private bool IsGrounded()
	{
		if (myRigidbody.velocity.y <= 0) 
		{
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) {
						jumpCount = 0;
						return true;
					}
				}
			}
		}
		return false;
	}
		

	void Kill ()
	{
//        Messenger.Broadcast(GameEvent.RELOAD_LEVEL);
//
//		// log that a death has taken place and the position of the player
//		Managers.logging.RecordEvent(3, "" + gameObject.transform.position);
	
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