using System.Collections;
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
	[SerializeField] private float fixedThrowAngle;
	[SerializeField] private float throwForce;
	[SerializeField] private float vertMultiplier = 1.1F;
	[SerializeField] private float horizMultiplier = 1.1F;
	//private bool addMomentum;
	//private float momentumMultiplier;

	private GameObject trajectoryPointPrefab;
	[SerializeField] private int numTrajectoryPoints = 16;
	private List<GameObject> trajectoryPoints;

	private GameObject shell;
	public CircleCollider2D shellHitbox { get; private set; }
	public CircleCollider2D shellClickbox { get; private set; }
	private Rigidbody2D shellRigidBody;

	private Transform player;

	private Vector2 fixedThrowVec;

	private Animator anim;

	private GameObject temporaryShellCollisionFix;

	private GameObject popUp;
	public Vector3 offset = new Vector3(0,1,0);

    public AudioClip throwSound;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		Debug.Log ("new shell, calling start");

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

		if (Managers.logging == null || Managers.logging.abValue == 0) { //A value for A/B testing
			horizMultiplier = 1.1f;
			vertMultiplier = 1.1f;
		} else { //B value for A/B testing
			horizMultiplier = 1.1f;
			vertMultiplier = 1.1f;
		}

		//Set throw vector
		float throwX = Mathf.Cos(fixedThrowAngle* Mathf.PI/180);
		float throwY = Mathf.Sin (fixedThrowAngle* Mathf.PI/180);
		fixedThrowVec = new Vector2 (throwX, throwY) * throwForce;

		// pickup popup
		popUp = (GameObject) Instantiate(Resources.Load("pickupPopup"), transform);
		popUp.transform.position = transform.position + offset;
		popUp.SetActive (false);

		trajectoryPointPrefab = Resources.Load ("dot") as GameObject;
		GameObject trajPoints = new GameObject ("Trajectory Points");
		trajPoints.transform.parent = transform;
		trajectoryPoints = new List<GameObject>();
		for (int i = 0; i < numTrajectoryPoints; i++) {
			GameObject dot = Instantiate(trajectoryPointPrefab, trajPoints.transform) as GameObject;
			dot.SetActive(false);
			trajectoryPoints.Add(dot);
		}

		if (transform.parent == null) 
		{
			ReleaseShell();
		} 
		else 
		{
			PickUpShell();
		}

        audioSource = gameObject.AddComponent<AudioSource>();
        throwSound = Resources.Load("throw") as AudioClip;
	}


	private void Update() {		
	
		bool throwHappened = false;

		if (Input.GetButtonDown ("Down")) {
			if (transform.parent != player && shellRigidBody.velocity.magnitude < shellPickupSpeed
			    && ((Vector2)(player.position - transform.position)).magnitude < interactDist) {

				if (PickUpShell ()) {
					Vector3 shellPos = transform.localScale;
					shellPos.x = Mathf.Abs (shellPos.x);// * Mathf.Sign(player.localScale.x);
					transform.localScale = shellPos;
				}
			} else if (transform.parent != null && transform.parent.GetComponent<PlayerController> () != null) {
				ReleaseShell ();
			}
		} else if (Input.GetButton ("Throw") && transform.parent == player) {
			float direction = transform.parent.localScale.x > 0 ? -1 : 1;
			throwVec.Set (fixedThrowVec.x * direction, fixedThrowVec.y);
			DrawTrajectory (shell.transform.position, throwVec);
		} else if (Input.GetButtonUp ("Throw") && transform.parent == player) {
			StartCoroutine (throwAnimRoutine ());
			Throw ();
			throwHappened = true;
		} else if (trajectoryPoints[0].activeInHierarchy) { DeleteTrajectory(); }

		if (!throwHappened) {
			// popup if can pick up shell
			Transform childShell = player.Find ("Shell");
			if ((childShell == null || childShell == this.transform) && player.GetComponent<PlayerController> ().CanPickupShell (gameObject) &&
			   transform.parent != player && shellRigidBody.velocity.magnitude < shellPickupSpeed
			   && ((Vector2)(player.position - transform.position)).magnitude < interactDist) {
				StartCoroutine ("popUpRoutine");

			} else {
				StopCoroutine ("popUpRoutine");
				popUp.SetActive (false);
			}
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

            audioSource.PlayOneShot(throwSound);

			// log that a throw has taken place and the position of the player\
			Managers.logging.RecordEvent(2, "" + player.position);
		}
	}

	//true if shell was successfully picked up, false otherwise
	public bool PickUpShell() 
	{
		Debug.Log("picking up shell");
		Transform childShell = player.Find ("Shell");
		if ((childShell == null || childShell == this.transform) && player.GetComponent<PlayerController>().CanPickupShell(gameObject)) {
			shell.transform.parent = player;
            shell.GetComponent<SpriteRenderer>().sortingOrder = 1;
			//shellHitbox.enabled = false;
			//shellClickbox.enabled = true;
			shellRigidBody.isKinematic = true;
			//shellRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
			shellRigidBody.velocity = Vector3.zero;
			shell.transform.localPosition = defaultShellPos;
			shell.gameObject.layer = LayerMask.NameToLayer ("PickedUpShell");

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
			temporaryShellCollisionFix.layer = LayerMask.NameToLayer ("PickedUpShell");

			// log that the shell has been picked up
			Managers.logging.RecordEvent (9, "" + player.transform.position);

			return true;
		}
		return false;
	}

	private void ReleaseShell() 
	{
        shell.GetComponent<SpriteRenderer>().sortingOrder = 2;
		shell.transform.parent = null;
		shellHitbox.enabled = true;
		//shellClickbox.enabled = false;
		shellRigidBody.isKinematic = false;
		//shellRigidBody.constraints = RigidbodyConstraints2D.None;
		shell.layer = LayerMask.NameToLayer("Shell");
        player.GetComponent<PlayerController>().ReleaseShell(gameObject);
		if (trajectoryPoints[0].activeInHierarchy) { DeleteTrajectory(); }

		Destroy (temporaryShellCollisionFix);

		// log that the shell has been dropped
		Managers.logging.RecordEvent (8, "" + player.transform.position);
	}

	IEnumerator throwAnimRoutine(){

		player.GetComponent<PlayerController> ().shellThrowing = true;

		// play the throw animation on the player
		anim.SetInteger("State", 2);

		yield return new WaitForSeconds (0.58f);

		player.GetComponent<PlayerController> ().shellThrowing = false;



	}

	private void DrawTrajectory(Vector3 trajStartPos, Vector2 forceVector) {
		Vector2 trajVelocity = (forceVector/shellRigidBody.mass)*Time.fixedDeltaTime;
		float velocity = Mathf.Sqrt(Mathf.Pow(trajVelocity.x, 2) + Mathf.Pow(trajVelocity.y, 2));
		float angle = Mathf.Atan2(trajVelocity.y, trajVelocity.x);

		float t = 0.1f;
		for (int i = 0; i < numTrajectoryPoints; i++) {
			float dx = velocity * t * Mathf.Cos(angle);
			float dy = velocity * t * Mathf.Sin(angle) - (Physics2D.gravity.magnitude * Mathf.Pow(t, 2) / 2.0f);
			Vector3 pos = new Vector3(trajStartPos.x + dx, trajStartPos.y + dy, -3);
			trajectoryPoints[i].transform.position = pos;
			trajectoryPoints[i].SetActive(true);
			trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(trajVelocity.y - (Physics.gravity.magnitude) * t, trajVelocity.x) * Mathf.Rad2Deg);
			t += 0.1f;
		}
	}

	void DeleteTrajectory() {
		for (int i = 0; i < numTrajectoryPoints; i++) {
			trajectoryPoints[i].SetActive(false);
		}
	}

	IEnumerator popUpRoutine(){

		yield return new WaitForSeconds (1);

		popUp.SetActive (true);
	}

}