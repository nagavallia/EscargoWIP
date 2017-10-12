using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour {

	[SerializeField] private int sinkAmount;
	private Vector3 startPosition;
	private Vector3 sunkenPosition;
	// Use this for initialization
	void Start () {
		startPosition = this.transform.localPosition;
		sunkenPosition = startPosition - (new Vector3 (0, sinkAmount, 0));
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.tag == "Shell") {
			Shell shell = collision.gameObject.GetComponent<Shell> ();
			if (shell.waterLevel > 0) {
				this.transform.localPosition = sunkenPosition;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Shell") {
			this.transform.localPosition = startPosition;
		}
	}
}
