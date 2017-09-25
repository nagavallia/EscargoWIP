using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Renderer> ().enabled = false;
		gameObject.GetComponent<Collider2D> ().enabled = false;
	}

	void Interact() {
		gameObject.GetComponent<Renderer> ().enabled = !gameObject.GetComponent<Renderer> ().enabled;
		gameObject.GetComponent<Collider2D> ().enabled = !gameObject.GetComponent<Collider2D> ().enabled;
	}
}
