using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlatform : MonoBehaviour {
	[SerializeField] private GameObject other;
	[SerializeField] private float moveSpeed = .75f;
	[SerializeField] private float moveDist = 1f;

	private Vector3 startPos;
	private Vector3 otherStartPos;
	public Vector3 targetPos;
	public Vector3 otherTargetPos;

	private int weight;
	private int otherWeight; 

	private bool weightChanged;
	private bool otherWeightChanged;
	private BoxCollider2D selfCollider;
	public List<GameObject> weighingDown;

	private float GROUND_CHECK;

	private void Start() {
		other.GetComponent<SpriteRenderer> ().color = Color.white;

		startPos = gameObject.transform.position;
		otherStartPos = other.transform.position;
		targetPos = startPos;
		otherTargetPos = otherStartPos;

		weight = 0;
		otherWeight = 0;

		weightChanged = false;
		otherWeightChanged = false;

		GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D>().size.y;

		selfCollider = GetComponent<BoxCollider2D> ();
		weighingDown = new List<GameObject> ();

		StartCoroutine ("Move");
	}

	private void Update() {
		if (weight < 0 || otherWeight < 0) Debug.Log ("Something terrible went wrong with ScalePlatform!");

		if (weightChanged || otherWeightChanged) {
			targetPos = startPos + new Vector3 (0f, (otherWeight - weight)*moveDist, 0f);
			otherTargetPos = otherStartPos + new Vector3 (0f, (weight - otherWeight)*moveDist, 0f);

			weightChanged = false;
			otherWeightChanged = false;
		}
	}

	private IEnumerator Move() {
		float step = moveSpeed * Time.deltaTime;

		while (true) {
			while (gameObject.transform.position == targetPos && other.transform.position == otherTargetPos)
				yield return null;

			Vector3 tmp = other.transform.position;
			gameObject.transform.position = Vector3.MoveTowards (transform.position, targetPos, step);
			other.transform.position = Vector3.MoveTowards (tmp, otherTargetPos, step);

			yield return null;
		}
	}

	private void OnCollisionStay2D(Collision2D collision) {
		collision.transform.SetParent (transform);
		Bounds collisionBound = collision.collider.bounds;

		Vector2 left = new Vector2 (collisionBound.min.x, collisionBound.center.y);
		Vector2 right = left + new Vector2 (collisionBound.size.x, 0f);

		RaycastHit2D leftHit = Physics2D.Raycast(left, Vector2.down, GROUND_CHECK);
		RaycastHit2D rightHit = Physics2D.Raycast (right, Vector2.down, GROUND_CHECK);
		if ((leftHit.collider == selfCollider || rightHit.collider == selfCollider)
			&& (collision.gameObject.tag == "Shell" || collision.transform.Find("Shell") != null)) {
			Shell shell = collision.gameObject.tag == "Shell" ? collision.gameObject.GetComponent<Shell> () : collision.transform.Find("Shell").GetComponent<Shell>();
			if (!weighingDown.Contains(collision.gameObject)) 
				weighingDown.Add (collision.gameObject);

			if (shell.isFull() && weighingDown.Count == 1) {
				weight = 1;
				weightChanged = true;
			}
		} else if (collision.gameObject.tag != "Shell" && collision.transform.Find("Shell") == null && weighingDown.Contains(collision.gameObject)) {
			weighingDown.Remove (collision.gameObject);
			if (weighingDown.Count == 0) {
				weight = 0;
				weightChanged = true;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if ((collision.gameObject.tag == "Shell" || collision.transform.Find("Shell") != null) 
			&& weighingDown.Contains(collision.gameObject)) {

			weighingDown.Remove(collision.gameObject);

			if (weighingDown.Count == 0) {
				weight = 0;
				weightChanged = true;
			}
		}
		if (collision.transform.parent == transform) collision.transform.SetParent (null);
	}

	public void OtherWeightChange(int amount) {
		otherWeight = amount;
		otherWeightChanged = true;
	}
}
