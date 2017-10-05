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
	[SerializeField] private GameObject trajectoryPointPrefab;
	[SerializeField] private int numTrajectoryPoints;
	[SerializeField] private float fixedThrowAngle;
	[SerializeField] private float throwForce;
	[SerializeField] private bool addMomentum;
	[SerializeField] private float momentumMultiplier;

    private GameObject shell;
    public CircleCollider2D shellHitbox { get; private set; }
    public CircleCollider2D shellClickbox { get; private set; }
    private Rigidbody2D shellRigidBody;

    private Transform player;
    private List<GameObject> trajectoryPoints;
	private KeyCode THROW_BUTTON = KeyCode.Space;
	private KeyCode USE_BUTTON = KeyCode.E;

	private Vector2 fixedThrowVec;

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

        Physics2D.IgnoreCollision(shellHitbox, player.transform.Find("MovementHitbox").GetComponent<Collider2D>());
        foreach (Collider2D collider in player.GetComponents<Collider2D>()) 
            Physics2D.IgnoreCollision(collider, shellHitbox);

        trajectoryPoints = new List<GameObject>();

        for (int i = 0; i < numTrajectoryPoints; i++) 
		{
            GameObject dot = Instantiate(trajectoryPointPrefab) as GameObject;
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

		//Set throw vector
		float throwX = Mathf.Cos(fixedThrowAngle);
		float throwY = Mathf.Sin (fixedThrowAngle);
		fixedThrowVec = new Vector2 (throwX, throwY) * throwForce;
	}

    private void OnDestroy() {
        foreach (GameObject point in trajectoryPoints) { Destroy(point); }
    }

    private void Update() {
        if (Input.GetKeyDown(USE_BUTTON)) {
            if (transform.parent == null && shellRigidBody.velocity.magnitude < shellPickupSpeed  
                && (player.position - transform.position).magnitude < interactDist) {
                Vector3 shellPos = transform.localScale;
                shellPos.x = Mathf.Sign(player.localScale.x) * Mathf.Abs(shellPos.x);
                transform.localScale = shellPos;
                PickUpShell();
            } else if (transform.parent != null && transform.parent.GetComponent<PlayerController>() != null) {
                ReleaseShell();
            }
        } else if (Input.GetKeyDown(THROW_BUTTON)) {
			Throw();
        }
    }
		
	private void Throw () 
	{
		if (transform.parent != null) 
		{
			float direction = transform.parent.localScale.x > 0 ? -1 : 1;
            ReleaseShell();
            throwVec.Set (fixedThrowVec.x * direction, fixedThrowVec.y);
			shellRigidBody.AddForce (throwVec);
			if (addMomentum) {
				Rigidbody2D playerRigidBody = player.gameObject.GetComponent<Rigidbody2D> ();
				shellRigidBody.velocity += playerRigidBody.velocity * momentumMultiplier;
			}
        }
	}

	public void PickUpShell() 
	{
        shell.transform.parent = player;
        shellHitbox.enabled = false;
        //shellClickbox.enabled = true;
        shellRigidBody.isKinematic = true;
        shellRigidBody.velocity = Vector3.zero;
        shell.transform.localPosition = defaultShellPos;
    }

    private void ReleaseShell() 
	{
        shell.transform.parent = null;
        shellHitbox.enabled = true;
        //shellClickbox.enabled = false;
        shellRigidBody.isKinematic = false;
    }
}
