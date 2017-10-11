using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = false; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }
	}
	
    // Function called when attatched interactable collides with gameObject.
	void Interact() {
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = !collider.enabled; // Enable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }
    }
}
