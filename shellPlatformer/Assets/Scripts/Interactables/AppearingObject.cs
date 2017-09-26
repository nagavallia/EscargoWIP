using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().enabled = false;
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = false; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }
    }

	void Interact() {
		gameObject.GetComponent<Renderer> ().enabled = !gameObject.GetComponent<Renderer> ().enabled;
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = !collider.enabled; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }
    }
}
