using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlatform : MonoBehaviour {
	[SerializeField] private GameObject other;
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private float moveDist = 1f;

	private Vector3 startPos;
	private Vector3 otherStartPos;
	private Vector3 targetPos;
	private Vector3 otherTargetPos;

	private int weight;
	private int otherWeight; 

	private bool weightChanged;
	private bool otherWeightChanged;

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

	private void OnCollisionEnter2D(Collision2D collision) {
		weight++;
		weightChanged = true;
		collision.transform.SetParent (transform);
	}

	private void OnCollisionExit2D(Collision2D collision) {
		weight = Mathf.Max(weight-1, 0);
		weightChanged = true;
		collision.transform.SetParent (null);
	}

	public void OtherWeightChange(int amount) {
		otherWeight = Mathf.Max (otherWeight + amount, 0);
		otherWeightChanged = true;
	}
}
