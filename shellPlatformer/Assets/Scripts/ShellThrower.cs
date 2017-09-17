﻿using System.Collections;
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
    private Rigidbody2D shellRigidBody;

    private Transform player;
    [SerializeField] private Vector3 defaultShellPos;
    [SerializeField] private float interactDist;

	// Use this for initialization
	void Start () {
        //shell = transform.Find("Shell").gameObject;
        shell = this.gameObject;
        throwVec = new Vector2();
        foreach (CircleCollider2D collider in shell.GetComponents<CircleCollider2D>()) {
            if (!collider.isTrigger) { shellHitbox = collider; }
        }
        shellRigidBody = shell.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

        if (transform.parent == null) {
            ReleaseShell();
        } else {
            PickUpShell();
        }
	}

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (transform.parent == null && shellRigidBody.velocity.magnitude < 1f  
                && (player.position - transform.position).magnitude < interactDist) {
                PickUpShell();
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
        if (transform.parent != null) {
            Debug.Log("drag finished");
            ReleaseShell();
            shell.GetComponent<Rigidbody2D>().AddForce(throwVec);
        }
    }

    private void PickUpShell() {
        shell.transform.parent = player;
        shellHitbox.enabled = false;
        shellRigidBody.isKinematic = true;
        shell.transform.localPosition = defaultShellPos;
    }

    private void ReleaseShell() {
        shell.transform.parent = null;
        shellHitbox.enabled = true;
        shellRigidBody.isKinematic = false;
    }
}
