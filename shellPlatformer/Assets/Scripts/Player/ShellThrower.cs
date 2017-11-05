﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellThrower : MonoBehaviour {
	private Vector3 startPos;
	private Vector3 curPos;
	private Vector2 throwVec;


	[SerializeField] private float MAX_SPEED;
	[SerializeField] private float MAX_DRAG_DIST;
	[SerializeField] private Vector3 defaultShellPos;
	[SerializeField] private float shellPickupSpeed;
	[SerializeField] private float interactDist;
	[SerializeField] private int numTrajectoryPoints;
	[SerializeField] private float fixedThrowAngle;
	[SerializeField] private float throwForce;
	[SerializeField] private float vertMultiplier = 1.1F;
	[SerializeField] private float horizMultiplier = 1.1F;
	//private bool addMomentum;
	//private float momentumMultiplier;

	private GameObject shell;
	public CircleCollider2D shellHitbox { get; private set; }
	public CircleCollider2D shellClickbox { get; private set; }
	private Rigidbody2D shellRigidBody;

	private Transform player;

	private Vector2 fixedThrowVec;

	private Animator anim;

	private GameObject temporaryShellCollisionFix;

	// Use this for initialization
	void Start () {

		shell = this.gameObject;
		throwVec = new Vector2();

		foreach (CircleCollider2D collider in shell.GetComponents<CircleCollider2D>()) 
		{
			if (!collider.isTrigger) 
			{ 
				shellHitbox = collider; 
			}
			else 
			{ 
				shellClickbox = collider; 
			}
		}

		shellClickbox.enabled = false;

		shellRigidBody = shell.GetComponent<Rigidbody2D>();
		player = GameObject.FindWithTag("Player").transform;

		anim = player.GetComponent<Animator>();

		foreach (Collider2D collider in player.transform.Find("MovementHitbox").GetComponents<Collider2D>())
			Physics2D.IgnoreCollision(collider, shellHitbox);
		foreach (Collider2D collider in player.GetComponents<Collider2D>()) 
			Physics2D.IgnoreCollision(collider, shellHitbox);

		if (transform.parent == null) 
		{
			ReleaseShell();
		} 
		else 
		{
			PickUpShell();
		}

		//Set throw vector
		float throwX = Mathf.Cos(fixedThrowAngle* Mathf.PI/180);
		float throwY = Mathf.Sin (fixedThrowAngle* Mathf.PI/180);
		fixedThrowVec = new Vector2 (throwX, throwY) * throwForce;
	}


	private void Update() {			
		if (Input.GetButtonDown("Down")) {
			if (transform.parent != player && shellRigidBody.velocity.magnitude < shellPickupSpeed  
				&& (player.position - transform.position).magnitude < interactDist) {
				Vector3 shellPos = transform.localScale;
				shellPos.x = Mathf.Sign(player.localScale.x) * Mathf.Abs(shellPos.x);
				transform.localScale = shellPos;
				PickUpShell();
			} else if (transform.parent != null && transform.parent.GetComponent<PlayerController>() != null) {
				ReleaseShell();
			}
		} else if (Input.GetButtonDown("Throw")) {
			StartCoroutine (throwAnimRoutine ());
			Throw();
		}
	}

	private void Throw () 
	{
		if (transform.parent == player) 
		{
			float direction = transform.parent.localScale.x > 0 ? -1 : 1;
			ReleaseShell();
			Vector2 vel = player.GetComponent<Rigidbody2D>().velocity;
			float xMult = Mathf.Abs(vel.x) < 1 ? 1 : horizMultiplier;
			float yMult = vel.y < 1 ? 1 : vertMultiplier;

			throwVec.Set (fixedThrowVec.x * direction * xMult, fixedThrowVec.y * yMult);
			shellRigidBody.AddForce (throwVec);
			Debug.Log(throwVec);
			// log that a throw has taken place and the position of the player\
			Managers.logging.RecordEvent(2, "" + player.position);
		}
	}

	public void PickUpShell() 
	{
		Debug.Log("picking up shell");
		Transform childShell = player.Find ("Shell");
		Debug.Log (shell);
		if (childShell == null || childShell == this.transform) {
			shell.transform.parent = player;
			//shellHitbox.enabled = false;
			//shellClickbox.enabled = true;
			shellRigidBody.isKinematic = true;
			//shellRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
			shellRigidBody.velocity = Vector3.zero;
			shell.transform.localPosition = defaultShellPos;

			temporaryShellCollisionFix = new GameObject ("tempShellCollisionFix");
			temporaryShellCollisionFix.transform.SetParent (player);
			temporaryShellCollisionFix.transform.localPosition = this.transform.localPosition;
			CircleCollider2D tempCollider = temporaryShellCollisionFix.AddComponent<CircleCollider2D> ();
			tempCollider.radius = 0.4f;
			foreach (Collider2D collider in player.transform.Find("MovementHitbox").GetComponents<Collider2D>())
				Physics2D.IgnoreCollision (collider, tempCollider);
			foreach (Collider2D collider in player.GetComponents<Collider2D>())
				Physics2D.IgnoreCollision (collider, tempCollider);
			Physics2D.IgnoreCollision (tempCollider, shellHitbox);
			temporaryShellCollisionFix.tag = "TempShell";

			//temporaryShellCollisionFix.layer = LayerMask.NameToLayer ("PickedUpShell");
			//Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("PickedUpShell"), LayerMask.NameToLayer ("Enemies"));
		}
	}

	private void ReleaseShell() 
	{
		shell.transform.parent = null;
		shellHitbox.enabled = true;
		//shellClickbox.enabled = false;
		shellRigidBody.isKinematic = false;
		//shellRigidBody.constraints = RigidbodyConstraints2D.None;

		Destroy (temporaryShellCollisionFix);
	}

	IEnumerator throwAnimRoutine(){

		// play the throw animation on the player
		anim.SetInteger("State", 2);

		yield return new WaitForSeconds (0.5f);

		Throw();

		// set the animation back to idle
		anim.SetInteger("State", 0);
	}

}