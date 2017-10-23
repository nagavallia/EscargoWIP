using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour {

	[SerializeField] private int sinkAmount = 1;
    [SerializeField] private Transform light;
	private Vector3 startPosition, lightStartPosition;
	private Vector3 sunkenPosition, lightSunkenPosition;

    private Vector2 startSize, sunkenSize;

    private float GROUND_CHECK;
	// Use this for initialization
	void Start () {
		startPosition = this.transform.localPosition;
        lightStartPosition = light.localPosition;
        startSize = GetComponent<SpriteRenderer>().size;

        sunkenPosition = startPosition - (new Vector3(0, sinkAmount * 0.5f, 0)); 
        lightSunkenPosition = lightStartPosition - (new Vector3 (0, sinkAmount*0.5f, 0));
        sunkenSize = startSize + new Vector2(0, sinkAmount);

        GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.y;
	}

	void OnCollisionStay2D(Collision2D collision) {
        RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, Vector2.down, GROUND_CHECK);
		if (hit.collider != null && collision.gameObject.tag == "Shell") {
            Debug.Log("colliding with shell");
			Shell shell = collision.gameObject.GetComponent<Shell> ();
			if (shell.waterLevel > 0) {
                collision.transform.SetParent(light);

                light.localPosition = lightSunkenPosition;
                GetComponent<BoxCollider2D>().offset = new Vector2(lightSunkenPosition.x, lightSunkenPosition.y);
                this.transform.localPosition = sunkenPosition;
                GetComponent<SpriteRenderer>().size = sunkenSize;

                collision.transform.SetParent(null);
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Shell") {
			light.localPosition = lightStartPosition;
            GetComponent<BoxCollider2D>().offset = new Vector2(lightStartPosition.x, lightStartPosition.y);
            this.transform.localPosition = startPosition;
            GetComponent<SpriteRenderer>().size = startSize;

        }
	}
}
