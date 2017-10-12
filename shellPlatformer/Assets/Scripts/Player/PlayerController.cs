using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D myRigidbody;

	[SerializeField]
	private float movementSpeed;

	private bool facingRight;

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

	[SerializeField]
	private float jumpForce;

	[SerializeField] 
	private float maxDrag;

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
		facingRight = false;
	}
		
	void Update(){
		HandleInput ();

		if (this.transform.Find ("Shell") == null) {
			doubleJump = true;
		} else {
			doubleJump = false;
		}
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxis ("Horizontal");

		if (horizontal < 0) {
			horizontal = -1;
		} else if (horizontal > 0) {
			horizontal = 1;
		} 

		isGrounded = IsGrounded ();
		move (horizontal);
		Flip (horizontal);

		ResetValues ();
	}

	// Move procedure handles player movement, jump, and double jump
	private void move (float horizontal)
	{
		myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, myRigidbody.velocity.y);

		if (!isGrounded && doubleJump && jump) {
			if (jumpCount == 0) {
				myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, 0);
				myRigidbody.AddForce (new Vector2 (0, 0.8f * jumpForce));
				jumpCount += 1;

				// log that a double jump has occurred and the position of the player
				Managers.logging.RecordEvent(1, "" + gameObject.transform.position);
			}
		}

		if (isGrounded && jump) {
			isGrounded = false;
			myRigidbody.AddForce (new Vector2 (0, jumpForce));

			// log that a jump has occurred and the position of the player
			//Managers.logging.RecordEvent(0, "" + gameObject.transform.position);
		}

	}

	// Sets the jump boolean and the drag value depending on key inputs
	private void HandleInput(){

		if (Input.GetKeyDown(KeyCode.W)) {// || Input.GetKeyDown(KeyCode.Space)){
			jump = true;
		}

		if (Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A)) {
			gameObject.GetComponent<Rigidbody2D> ().drag = maxDrag;
		} else {
			gameObject.GetComponent<Rigidbody2D> ().drag = 0; 
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
        Messenger.Broadcast(GameEvent.RELOAD_LEVEL);

		// log that a death has taken place and the position of the player
		Managers.logging.RecordEvent(3, "" + gameObject.transform.position);
	}

	private void FillShell()
	{
		Shell shell = this.transform.Find ("Shell").gameObject.GetComponent<Shell>();

		if (shell != null) {
			shell.FillShell ();
		}
	}
}