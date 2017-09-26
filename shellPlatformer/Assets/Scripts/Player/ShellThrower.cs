using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellThrower : MonoBehaviour {
    private Vector3 startPos;
    private Vector3 curPos;
    private Vector2 throwVec;

    [SerializeField] private float MAX_SPEED;
    [SerializeField] private float MAX_DRAG_DIST;

    private GameObject shell;
    public CircleCollider2D shellHitbox { get; private set; }
    public CircleCollider2D shellClickbox { get; private set; }
    private Rigidbody2D shellRigidBody;

    private Transform player;
    [SerializeField] private Vector3 defaultShellPos;
    [SerializeField] private float shellPickupSpeed;
    [SerializeField] private float interactDist;

    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private int numTrajectoryPoints;
    private List<GameObject> trajectoryPoints;

    [SerializeField] private float minYoshiAngle; //angle in degrees
    [SerializeField] private float maxYoshiAngle;
    private float yoshiStep = 1f;
    private float yoshiStrength = 400f;

    private KeyCode THROW_BUTTON = KeyCode.Space;
    private KeyCode USE_BUTTON = KeyCode.E;

    public enum ThrowMode {
        YoshiThrowing,
        MomentumThrowing,
        FixedThrowing,
        MouseThrowing
    }
    [SerializeField] private ThrowMode throwMode;
	[SerializeField] private Vector2 fixedThrowVec;
	[SerializeField] private float velocityMultiplier;

	// Use this for initialization
	void Start () {
        //shell = transform.Find("Shell").gameObject;
        shell = this.gameObject;
        throwVec = new Vector2();
        foreach (CircleCollider2D collider in shell.GetComponents<CircleCollider2D>()) {
            if (!collider.isTrigger) { shellHitbox = collider; }
            else { shellClickbox = collider; }
        }
        shellRigidBody = shell.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), shellHitbox);

        trajectoryPoints = new List<GameObject>();
        for (int i = 0; i < numTrajectoryPoints; i++) {
            GameObject dot = Instantiate(trajectoryPointPrefab) as GameObject;
            dot.SetActive(false);
            trajectoryPoints.Add(dot);
        }

        if (transform.parent == null) {
            ReleaseShell();
        } else {
            PickUpShell();
        }
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
            switch (throwMode) {
                case ThrowMode.YoshiThrowing:
                    //handle yoshi throwing
                    StartCoroutine("YoshiThrow");
                    break;
                case ThrowMode.FixedThrowing:
                    //handle fixe throwing
                    FixedThrow();
                    break;
                case ThrowMode.MomentumThrowing:
                    // handle momentum throwing
                    MomentumThrow();
                    break;
                default:
                    Debug.Log("you forgot to set throw mode of shell in editor!");
                    break;
            }
        }
    }
		
    private IEnumerator YoshiThrow () 
	{
        if (transform.parent != null) {
            var increasing = true;
            var angle = minYoshiAngle;
            while (Input.GetKey(THROW_BUTTON)) {
                if (increasing) {
                    angle += yoshiStep;
                } else {
                    angle -= yoshiStep;
                }
                if (angle >= maxYoshiAngle || angle <= minYoshiAngle) { increasing = !increasing; }

                throwVec.Set(Mathf.Cos(angle * Mathf.Deg2Rad) * yoshiStrength * -Mathf.Sign(player.localScale.x), 
                    Mathf.Sin(angle * Mathf.Deg2Rad) * yoshiStrength);
                DrawTrajectory(shell.transform.position, (throwVec/shellRigidBody.mass)*Time.fixedDeltaTime);
                yield return null;
            }
            ReleaseShell();
            DeleteTrajectory();
            shellRigidBody.AddForce(throwVec);
        }
    }

	private void FixedThrow () 
	{
		if (transform.parent != null) 
		{
			float direction = transform.parent.localScale.x > 0 ? -1 : 1;
            ReleaseShell();
            throwVec.Set (fixedThrowVec.x * direction, fixedThrowVec.y);
			shellRigidBody.AddForce (throwVec);
        }
	}
	private void MomentumThrow ()
	{
		ReleaseShell ();
		Rigidbody2D playerRigidBody = player.gameObject.GetComponent<Rigidbody2D>();
		shellRigidBody.velocity = playerRigidBody.velocity*velocityMultiplier;
	}
		

    // Called when mouse is clicked within collider
    private void OnMouseDown() {
        if (transform.parent != null) {
            Debug.Log("mouse touched");
            startPos = Input.mousePosition;
        }
    }

    // Called every frame mouse button is down after clicking in collider
    private void OnMouseDrag() {
        if (throwMode == ThrowMode.MouseThrowing && transform.parent != null) {
            Debug.Log("moving mouse, throwVec: " + throwVec.magnitude);
            curPos = Input.mousePosition;
            var tempVec = -(curPos - startPos);
            throwVec.Set(tempVec.x, tempVec.y);
            float oldMag = throwVec.magnitude;
            throwVec.Normalize();
            throwVec *= Mathf.Min(oldMag, MAX_DRAG_DIST) * MAX_SPEED / MAX_DRAG_DIST;
            DrawTrajectory(shell.transform.position, (throwVec/shellRigidBody.mass)*Time.fixedDeltaTime);
        }
    }

    // Called when mouse button is released after clicking in collider
    private void OnMouseUp() {
        if (throwMode == ThrowMode.MouseThrowing && transform.parent != null) {
            Debug.Log("drag finished");
            DeleteTrajectory();
            ReleaseShell();
            shellRigidBody.AddForce(throwVec);
        }
    }

	public void PickUpShell() {
        shell.transform.parent = player;
        shellHitbox.enabled = false;
        shellClickbox.enabled = true;
        shellRigidBody.isKinematic = true;
        shellRigidBody.velocity = Vector3.zero;
        shell.transform.localPosition = defaultShellPos;
    }

    private void ReleaseShell() {
        shell.transform.parent = null;
        shellHitbox.enabled = true;
        shellClickbox.enabled = false;
        shellRigidBody.isKinematic = false;
    }

    private void DrawTrajectory(Vector3 trajStartPos, Vector2 trajVelocity) {
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

    public void SwitchThrowMode(int val) {
        throwMode = (ThrowMode) val;
    }
}
