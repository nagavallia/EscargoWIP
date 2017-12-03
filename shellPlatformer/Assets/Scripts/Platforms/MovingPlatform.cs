using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private GameObject positioner;
	[SerializeField] private float moveDuration = 3.0f;
	[SerializeField] private bool startEnabled = false;
	[SerializeField] private bool moveForeverOnceActive = true;

	private Vector3 startPos;
	private Quaternion startRot;
	private Vector3 endPos;
	private Quaternion endRot;

	private float curTime;
	private bool isMoving;
	private bool queuedAnimation;

	private BoxCollider2D selfCollider;
	private float GROUND_CHECK;

	private void Start ()
	{
		startPos = gameObject.transform.position;
		startRot = gameObject.transform.rotation;
		endPos = positioner.transform.position;
		endRot = positioner.transform.rotation;
		Destroy (positioner);

		curTime = 0f;
		isMoving = false;
		queuedAnimation = false;

		GROUND_CHECK = 0.25f + GetComponent<BoxCollider2D> ().size.y;
		selfCollider = GetComponent<BoxCollider2D> ();

		if (startEnabled)
			StartCoroutine ("Move");
	}

	private void Update ()
	{
		if (queuedAnimation && !isMoving) {
			queuedAnimation = false;
			Interact ();
		}
	}

	private void Interact ()
	{
		if (!isMoving)
			StartCoroutine ("Move");
		else {
			queuedAnimation = !queuedAnimation;
		}
	}

	private IEnumerator Move ()
	{
		isMoving = true;
		while (isMoving) {
			while (curTime < moveDuration) {
				var t = curTime / moveDuration;
				gameObject.transform.SetPositionAndRotation (Vector3.Lerp (startPos, endPos, t),
					Quaternion.Slerp (startRot, endRot, t));
				curTime += Time.deltaTime;
				yield return null;
			}
			gameObject.transform.SetPositionAndRotation (endPos, endRot);

			Vector3 tmpPos = startPos;
			Quaternion tmpRot = startRot;

			startPos = endPos;
			endPos = tmpPos;
			startRot = endRot;
			endRot = tmpRot;

			curTime = 0f;
			if (!moveForeverOnceActive)
				isMoving = false;
		}
	}

	private void OnCollisionEnter2D (Collision2D collision)
	{
		GameObject colObject;
		if (collision.gameObject.name == "MovementHitbox")
			colObject = collision.transform.parent.gameObject;
		else
			colObject = collision.gameObject;

		if (checkOnTop (colObject))
			colObject.transform.SetParent (transform);
	}

	private void OnCollisionExit2D (Collision2D collision)
	{
		GameObject colObject;
		if (collision.gameObject.name == "MovementHitbox")
			colObject = collision.transform.parent.gameObject;
		else
			colObject = collision.gameObject;

		if (colObject.transform.parent == transform && !checkOnTop(colObject))
			colObject.transform.SetParent (null);
	}

	private bool checkOnTop(GameObject obj) {
		foreach (Collider2D collisionCollider in obj.GetComponents<Collider2D>()) {
			Bounds collisionBound = collisionCollider.bounds;

			Vector2 left = new Vector2 (collisionBound.min.x, collisionBound.center.y);
			Vector2 right = left + new Vector2 (collisionBound.size.x, 0f);

			RaycastHit2D leftHit = Physics2D.Raycast (left, Vector2.down, GROUND_CHECK);
			RaycastHit2D rightHit = Physics2D.Raycast (right, Vector2.down, GROUND_CHECK);
			if ((leftHit.collider == selfCollider || rightHit.collider == selfCollider)
				&& obj.layer != LayerMask.NameToLayer ("Background")) {
				return true;
			}
		}

		return false;
	}
}
