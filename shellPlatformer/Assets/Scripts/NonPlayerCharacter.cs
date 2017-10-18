using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool startEnabled = true;

    public bool isMoving { get; private set; }
    public Vector3 moveVector { get; private set; }

    private float WALL_CHECK_DIST;

    private void Start() {
        isMoving = startEnabled;
        WALL_CHECK_DIST = 0.2f + GetComponent<BoxCollider2D>().bounds.extents.x;
        moveVector = transform.right * -Mathf.Sign(transform.localScale.x);

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
        rigidbody.freezeRotation = true;
    }

    private void Update() {
        if (isMoving) {
            transform.Translate(moveVector * Time.deltaTime * moveSpeed);
        }
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
        return isMoving && (other.layer == LayerMask.NameToLayer("Background") || other.layer == LayerMask.NameToLayer("Player")
            || other.tag == "Shell" || other.layer == LayerMask.NameToLayer("Movement Hitbox"));
    }

    private void Interact() {
        isMoving = true;
    }

    private void ChangeDirection() {
        moveVector = -moveVector;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Kill() {
		GameObject shell = Instantiate(Resources.Load("Shell")) as GameObject;
		shell.transform.localScale = new Vector3 (.5f, .5f, 1f);
        shell.transform.position = this.gameObject.transform.position;
        Destroy(this.gameObject);
    }

}
