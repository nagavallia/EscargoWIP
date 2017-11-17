using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool startEnabled = true;

    public bool isMoving { get; private set; }
    public Vector3 moveVector { get; private set; }

    private float WALL_CHECK_DIST;

    [SerializeField] private AudioClip DeathSound;
    private AudioSource audioSource;

	private Vector3 shellOffset = new Vector3(0.3f,0.1f,0);

    private void Start() {
        isMoving = startEnabled;
        WALL_CHECK_DIST = 0.2f + GetComponent<BoxCollider2D>().bounds.extents.x;
        moveVector = transform.right * -Mathf.Sign(transform.localScale.x);

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        rigidbody.freezeRotation = true;

        GetComponent<SpriteRenderer>().sortingOrder = 1;

        audioSource = gameObject.AddComponent<AudioSource>();

		// set the child to be a shell
		GameObject shell = Instantiate(Resources.Load("Shell")) as GameObject;
        shell.name = "Shell";
		SetAllCollidersStatus (shell, false);
		shell.GetComponent<Rigidbody2D> ().gravityScale = 0;
		shell.transform.SetParent (this.transform);
		shell.transform.position = transform.position + shellOffset;
		shell.GetComponent<SpriteRenderer> ().sortingLayerID = SortingLayer.NameToID ("Default");
    }

    private void Update() {
        if (isMoving) {
            transform.Translate(moveVector * Time.deltaTime * moveSpeed);
        }

		Debug.Log ("shell is in layer" + LayerMask.LayerToName (transform.Find ("Shell").gameObject.layer));
		if (LayerMask.LayerToName (transform.Find ("Shell").gameObject.layer) != "No Collision")
			transform.Find ("Shell").gameObject.layer = LayerMask.NameToLayer ("No Collision");

		//take child shell to be the same position as the parent moves
		this.transform.GetChild(0).transform.position = transform.position + shellOffset;

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(moveVector), WALL_CHECK_DIST);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null && ShouldTurnAround(hit.collider.gameObject)) {
                ChangeDirection();
                break;
            }
        }
    }

    // returns true if this NPC should turn around when colliding with other, false otherwise
    private bool ShouldTurnAround(GameObject other) {
		return isMoving && (other.layer == LayerMask.NameToLayer ("Background") || other.layer == LayerMask.NameToLayer ("Shell")); 
			//|| other.layer == LayerMask.NameToLayer("Player") || other.layer == LayerMask.NameToLayer("Movement Hitbox"));
    }

    private void Interact() {
        isMoving = true;
    }

    private void ChangeDirection() {
        moveVector = -moveVector;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

		// invert the shell offset
		shellOffset.x = - shellOffset.x;
    }

    private void Kill() {
		//GameObject shell = Instantiate(Resources.Load("Shell")) as GameObject;
		////shell.transform.localScale = new Vector3 (.5f, .5f, 1f);
        //shell.transform.position = this.gameObject.transform.position;
        //shell.name = "Shell";

		// drop child (shell)
		GameObject childShell = this.transform.GetChild(0).gameObject;
		childShell.transform.SetParent(this.transform.parent);
		childShell.transform.position = transform.position;
		SetAllCollidersStatus (childShell, true);
		childShell.GetComponent<Rigidbody2D> ().gravityScale = 1;
		childShell.layer = LayerMask.NameToLayer ("Shell");
		childShell.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID ("Player");

        audioSource.PlayOneShot(DeathSound);

        Destroy(this.gameObject);
    }

	public void SetAllCollidersStatus (GameObject go, bool active){
		foreach (CircleCollider2D c in go.GetComponents<CircleCollider2D>()) {
			c.enabled = active;
		}
	}

}
