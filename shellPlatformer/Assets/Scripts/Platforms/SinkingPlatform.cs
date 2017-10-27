using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour {

	[SerializeField] private float sinkAmount = 1f;
	[SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform light;
	private Vector3 startPosition, lightStartPosition;
	private Vector3 sunkenPosition, lightSunkenPosition;

    private Vector2 startSize, sunkenSize;

	private bool isSinking;

    private float GROUND_CHECK;
	// Use this for initialization
	void Start () {
		startPosition = this.transform.localPosition;
        lightStartPosition = new Vector3(0f, -(GetComponent<SpriteRenderer>().size.y - 1)/2f, -1f);
        light.localPosition = lightStartPosition;
        GetComponent<BoxCollider2D>().offset = new Vector2(lightStartPosition.x, lightStartPosition.y);
        startSize = GetComponent<SpriteRenderer>().size;

        sunkenPosition = startPosition - (new Vector3(0, sinkAmount * 0.5f, 0)); 
        lightSunkenPosition = lightStartPosition - (new Vector3 (0, sinkAmount*0.5f, 0));
        sunkenSize = startSize + new Vector2(0, sinkAmount);

        GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.y;

		isSinking = false;
		StartCoroutine ("Move");
	}

	void OnCollisionStay2D(Collision2D collision) {
        collision.transform.SetParent(light);
        RaycastHit2D hit = Physics2D.Raycast(collision.transform.position, Vector2.down, GROUND_CHECK);
		if (hit.collider != null && collision.gameObject.tag == "Shell") {
            Debug.Log("colliding with shell");
			Shell shell = collision.gameObject.GetComponent<Shell> ();
			if (shell.waterLevel > 0) {
                //collision.transform.SetParent(light);
				isSinking = true;

                //light.localPosition = lightSunkenPosition;
                //GetComponent<BoxCollider2D>().offset = new Vector2(lightSunkenPosition.x, lightSunkenPosition.y);
                //this.transform.localPosition = sunkenPosition;
                //GetComponent<SpriteRenderer>().size = sunkenSize;

                //collision.transform.SetParent(null);
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.parent == this.transform) collision.transform.SetParent(null);

        if (collision.gameObject.tag == "Shell") {
			isSinking = false;

			//light.localPosition = lightStartPosition;
            //GetComponent<BoxCollider2D>().offset = new Vector2(lightStartPosition.x, lightStartPosition.y);
            //this.transform.localPosition = startPosition;
            //GetComponent<SpriteRenderer>().size = startSize;

        }
	}

	private IEnumerator Move() {
		float step = moveSpeed * Time.deltaTime;

		while (true) {
			while ((isSinking && light.localPosition == lightSunkenPosition) ||
			       (!isSinking && light.localPosition == lightStartPosition)) {
				yield return null; 
			}

			BoxCollider2D collider = GetComponent<BoxCollider2D> ();
			SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

			if (isSinking) {
				light.localPosition = Vector3.MoveTowards (light.localPosition, lightSunkenPosition, step);
				GetComponent<BoxCollider2D> ().offset = Vector2.MoveTowards (collider.offset, new Vector2 (lightSunkenPosition.x, lightSunkenPosition.y), step);
				this.transform.localPosition = Vector3.MoveTowards (this.transform.localPosition, sunkenPosition, step);
				GetComponent<SpriteRenderer> ().size = Vector2.MoveTowards (sprite.size, sunkenSize, 2*step);
			} else {
				light.localPosition = Vector3.MoveTowards (light.localPosition, lightStartPosition, step);
				GetComponent<BoxCollider2D> ().offset = Vector2.MoveTowards (collider.offset, new Vector2 (lightStartPosition.x, lightStartPosition.y), step);
				this.transform.localPosition = Vector3.MoveTowards (this.transform.localPosition, startPosition, step);
				GetComponent<SpriteRenderer> ().size = Vector2.MoveTowards (sprite.size, startSize, 2*step);
			}

			yield return null;
		}
	}
}
