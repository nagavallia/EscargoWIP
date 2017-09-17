﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
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

	[SerializeField]
	private float jumpForce;

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D> ();
		facingRight = false;
	}
		
	void Update(){
		HandleInput ();
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		isGrounded = IsGrounded ();
		move (horizontal);
		Flip (horizontal);

		ResetValues ();
	}

	private void move (float horizontal)
	{
		myRigidbody.velocity = new Vector2 (horizontal * movementSpeed, myRigidbody.velocity.y);

		if (isGrounded && jump) {
			isGrounded = false;
			myRigidbody.AddForce (new Vector2 (0, jumpForce));
		}

	}

	private void HandleInput(){

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)){
			jump = true;
		}
			
	}

	private void Flip (float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;

			theScale.x *= -1;

			transform.localScale = theScale;
		}
	}

	private void ResetValues (){
		jump = false;
	}

	private bool IsGrounded()
	{
		if (myRigidbody.velocity.y <= 0) 
		{
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) {
						return true;
					}
				}
			}
		}
		return false;
	}
		
	void EnemyCollide ()
	{
		transform.SetPositionAndRotation (new Vector3 (-4.84f, -2.748991f, 0f), transform.rotation);
	}
}