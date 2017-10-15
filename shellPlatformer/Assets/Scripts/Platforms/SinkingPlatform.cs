using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour {

	[SerializeField] private int sinkAmount;
	private Vector3 startPosition;
	private Vector3 sunkenPosition;

    private float GROUND_CHECK;
	// Use this for initialization
	void Start () {
		startPosition = this.transform.localPosition;
		sunkenPosition = startPosition - (new Vector3 (0, sinkAmount, 0));

        GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.y;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay2D(Collision2D collision) {
        RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, Vector2.down, GROUND_CHECK);
		if (hit.collider != null && collision.gameObject.tag == "Shell") {
			Shell shell = collision.gameObject.GetComponent<Shell> ();
			if (shell.waterLevel > 0) {
                collision.transform.SetParent(this.transform);
                this.transform.localPosition = sunkenPosition;
                collision.transform.SetParent(null);
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Shell") {
			this.transform.localPosition = startPosition;
            
		}
	}
}
