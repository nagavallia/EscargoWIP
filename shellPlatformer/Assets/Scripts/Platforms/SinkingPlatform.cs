using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingPlatform : MonoBehaviour {

	[SerializeField] private float sinkAmount = 1f;
	[SerializeField] private float moveSpeed = .75f;
    [SerializeField] private Transform light;
	private Vector3 startPosition, lightStartPosition;
	private Vector3 sunkenPosition, lightSunkenPosition;

    private Vector2 startSize, sunkenSize;

	private bool isSinking;

	private BoxCollider2D selfCollider;
	private List<GameObject> weighingDown;

    [SerializeField] private AudioClip moveSound, finishSound;
    private AudioSource audioSource;

    private float GROUND_CHECK;
	// Use this for initialization
	void Start () {
		startPosition = this.transform.localPosition;
        lightStartPosition = new Vector3(0f, -(GetComponent<SpriteRenderer>().size.y - 1)/2f, -1f);
        light.localPosition = lightStartPosition;
        light.transform.localScale = new Vector3(1 / transform.localScale.x, 1f, 1f);
        GetComponent<BoxCollider2D>().offset = new Vector2(lightStartPosition.x, lightStartPosition.y);
        startSize = GetComponent<SpriteRenderer>().size;

        sunkenPosition = startPosition - (new Vector3(0, sinkAmount * 0.5f, 0)); 
        lightSunkenPosition = lightStartPosition - (new Vector3 (0, sinkAmount*0.5f, 0));
        sunkenSize = startSize + new Vector2(0, sinkAmount);

        GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.y;

		selfCollider = GetComponent<BoxCollider2D> ();
		weighingDown = new List<GameObject> ();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = moveSound;
        audioSource.loop = true;

		isSinking = false;
		StartCoroutine ("Move");
	}

	void OnCollisionStay2D(Collision2D collision) {
        collision.transform.SetParent(light);
		Bounds collisionBound = collision.collider.bounds;

		Vector2 left = new Vector2 (collisionBound.min.x, collisionBound.center.y);
		Vector2 right = left + new Vector2 (collisionBound.size.x, 0f);

		RaycastHit2D leftHit = Physics2D.Raycast(left, Vector2.down, GROUND_CHECK);
		RaycastHit2D rightHit = Physics2D.Raycast (right, Vector2.down, GROUND_CHECK);
		if ((leftHit.collider == selfCollider || rightHit.collider == selfCollider)
		    && (collision.gameObject.tag == "Shell" || collision.transform.Find ("Shell") != null)) {
			Debug.Log ("colliding with shell");
			Shell shell = collision.gameObject.tag == "Shell" ? collision.gameObject.GetComponent<Shell> () : collision.transform.Find ("Shell").GetComponent<Shell> ();
			if (!weighingDown.Contains (collision.gameObject))
				weighingDown.Add (collision.gameObject);

			if (shell.isFull () && weighingDown.Count == 1) {
				//collision.transform.SetParent(light);
				isSinking = true;

				//light.localPosition = lightSunkenPosition;
				//GetComponent<BoxCollider2D>().offset = new Vector2(lightSunkenPosition.x, lightSunkenPosition.y);
				//this.transform.localPosition = sunkenPosition;
				//GetComponent<SpriteRenderer>().size = sunkenSize;

				//collision.transform.SetParent(null);
			}
		} else if (collision.gameObject.tag != "Shell" && collision.transform.Find("Shell") == null && weighingDown.Contains(collision.gameObject)) {
			weighingDown.Remove (collision.gameObject);
			if (weighingDown.Count == 0)
				isSinking = false;
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.parent == light) collision.transform.SetParent(null);

		if ((collision.gameObject.tag == "Shell" || collision.transform.Find("Shell") != null)
			&& weighingDown.Contains(collision.gameObject)) {
			weighingDown.Remove (collision.gameObject);

			if (weighingDown.Count == 0)
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

            if (!audioSource.isPlaying)
                audioSource.Play();

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

            if ((isSinking && light.localPosition == lightSunkenPosition) || 
                (!isSinking && light.localPosition == lightStartPosition)) {

                audioSource.Stop();
                audioSource.PlayOneShot(finishSound);
            }

			yield return null;
		}
	}
}
