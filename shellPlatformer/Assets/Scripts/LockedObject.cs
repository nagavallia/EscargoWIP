using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Collider2D> ().enabled = false;
	}
	
	void Interact() {
		
		gameObject.GetComponent<Collider2D> ().enabled = !gameObject.GetComponent<Collider2D> ().enabled;
	}
}
