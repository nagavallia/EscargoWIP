using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlatformOther : MonoBehaviour {
	[SerializeField] private ScalePlatform parent;

	private float GROUND_CHECK;

	private BoxCollider2D selfCollider;
	public List<GameObject> weighingDown;

	private void Start() {
		GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.y;

		selfCollider = GetComponent<BoxCollider2D> ();
		weighingDown = new List<GameObject> ();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		collision.transform.SetParent (transform);
		Bounds collisionBound = collision.collider.bounds;

		Vector2 left = new Vector2 (collisionBound.min.x, collisionBound.center.y);
		Vector2 right = left + new Vector2 (collisionBound.size.x, 0f);

		RaycastHit2D leftHit = Physics2D.Raycast (left, Vector2.down, GROUND_CHECK);
		RaycastHit2D rightHit = Physics2D.Raycast (right, Vector2.down, GROUND_CHECK);
		if ((leftHit.collider == selfCollider || rightHit.collider == selfCollider)
		    && (collision.gameObject.tag == "Shell" || collision.transform.Find ("Shell") != null)) {
			Shell shell = collision.gameObject.tag == "Shell" ? collision.gameObject.GetComponent<Shell> () : collision.transform.Find ("Shell").GetComponent<Shell> ();
			weighingDown.Add (collision.gameObject);

			if (shell.waterLevel > 0 && weighingDown.Count == 1) {
				parent.OtherWeightChange (1);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		collision.transform.SetParent (null);
		if (collision.gameObject.tag == "Shell" || collision.transform.Find ("Shell") != null) {
			weighingDown.Remove (collision.gameObject);

			if (weighingDown.Count == 0)
				parent.OtherWeightChange (0);
		}
	}
}
