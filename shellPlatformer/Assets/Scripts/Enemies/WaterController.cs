using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Shell") {
			collision.gameObject.GetComponent<Shell> ().FillShell (); // Call fill shell on the shell to turn into water shell 
			Destroy(this.gameObject); // Destroy water particle when colliding with shell"
		}

		if (collision.gameObject.tag == "Ground" || collision.gameObject.layer == LayerMask.NameToLayer("Background") || collision.gameObject.tag == "Player") {
			Destroy (this.gameObject);
		}
	}
}
