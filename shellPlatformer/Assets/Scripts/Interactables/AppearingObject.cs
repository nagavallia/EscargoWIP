using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObject : MonoBehaviour {
    [SerializeField] private bool startEnabled = false;
    private AudioClip activateSound, deactivateSound;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().enabled = startEnabled;
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = startEnabled; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        var active = gameObject.GetComponent<Exit>() != null ? "exit_appear" : "exit_appear"; // load in correct activate sound
        var deactive = gameObject.GetComponent<Exit>() != null ? "exit_disappear" : "exit_disappear";

        activateSound = Resources.Load(active) as AudioClip;
        deactivateSound = Resources.Load(deactive) as AudioClip;
    }

	void Interact() {
        bool previousStatus = gameObject.GetComponent<Renderer>().enabled;
        gameObject.GetComponent<Renderer> ().enabled = !previousStatus;
        foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
            collider.enabled = !previousStatus; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
        }

		if (gameObject.GetComponent<Exit>() != null) {
       		audioSource.PlayOneShot(previousStatus ? deactivateSound : activateSound); // play activate or deactivate sound
		}
    }
}
