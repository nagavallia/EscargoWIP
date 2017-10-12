using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool startEnabled = true;

    public bool isMoving { get; private set; }
    public Vector3 moveVector { get; private set; }

    private float WALL_CHECK;

    private void Start() {
        isMoving = startEnabled;
        WALL_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.x;
        moveVector = transform.right * -Mathf.Sign(transform.localScale.x);
    }

    private void Update() {
        if (isMoving) {
            transform.Translate(moveVector * Time.deltaTime * moveSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(moveVector), WALL_CHECK);
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Background")) {
            changeDirection();
        }
    }

    private void Interact() {
        isMoving = true;
    }

    private void changeDirection() {
        moveVector = -moveVector;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void KillPlayer() {
        GameObject shell = Resources.Load("Shell") as GameObject;
        shell.transform.position = this.gameObject.transform.position;
        Destroy(this.gameObject);
    }

}
