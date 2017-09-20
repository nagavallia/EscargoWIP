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
    private CircleCollider2D shellHitbox;
    private CircleCollider2D shellClickbox;
    private Rigidbody2D shellRigidBody;

    private Transform player;
    [SerializeField] private Vector3 defaultShellPos;
    [SerializeField] private float shellPickupSpeed;
    [SerializeField] private float interactDist;

    public enum ThrowMode {
        YoshiThrowing,
        MomentumThrowing,
        FixedThrowing,
        MouseThrowing
    }
    [SerializeField] private ThrowMode throwMode;

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

        if (transform.parent == null) {
            ReleaseShell();
        } else {
            PickUpShell();
        }
	}

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (transform.parent == null && shellRigidBody.velocity.magnitude < shellPickupSpeed  
                && (player.position - transform.position).magnitude < interactDist) {
                Vector3 shellPos = transform.localScale;
                shellPos.x = Mathf.Sign(player.localScale.x) * Mathf.Abs(shellPos.x);
                transform.localScale = shellPos;
                PickUpShell();
            } else if (transform.parent != null && transform.parent.GetComponent<PlayerController>() != null) {
                ReleaseShell();
            }
        } else if (Input.GetKeyDown(KeyCode.F)) {
            switch (throwMode) {
                case ThrowMode.YoshiThrowing:
                    //handle yoshi throwing
                    break;
                case ThrowMode.FixedThrowing:
                    //handle fixe throwing
                    break;
                case ThrowMode.MomentumThrowing:
                    // handle momentum throwing
                    break;
                default:
                    Debug.Log("you forgot to set throw mode of shell in editor!");
                    break;
            }
        }
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
        if (transform.parent != null) {
            Debug.Log("moving mouse, throwVec: " + throwVec.magnitude);
            curPos = Input.mousePosition;
            var tempVec = -(curPos - startPos);
            throwVec.Set(tempVec.x, tempVec.y);
            float oldMag = throwVec.magnitude;
            throwVec.Normalize();
            throwVec *= Mathf.Min(oldMag, MAX_DRAG_DIST) * MAX_SPEED / MAX_DRAG_DIST;
        }
    }

    // Called when mouse button is released after clicking in collider
    private void OnMouseUp() {
        if (throwMode == ThrowMode.MouseThrowing && transform.parent != null) {
            Debug.Log("drag finished");
            ReleaseShell();
            shell.GetComponent<Rigidbody2D>().AddForce(throwVec);
        }
    }

    private void PickUpShell() {
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
}
