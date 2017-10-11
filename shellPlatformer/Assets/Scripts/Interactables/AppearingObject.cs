using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObject : MonoBehaviour {
    [SerializeField] private bool startEnabled = false;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().enabled = startEnabled;
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = startEnabled; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }
    }

	void Interact() {
		gameObject.GetComponent<Renderer> ().enabled = !gameObject.GetComponent<Renderer> ().enabled;
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = !collider.enabled; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }
    }
}
