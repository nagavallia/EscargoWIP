using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
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

    private void Start() {
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
        endPos = positioner.transform.position;
        endRot = positioner.transform.rotation;
        Destroy(positioner);

        curTime = 0f;
        isMoving = false;
        queuedAnimation = false;

        if (startEnabled) StartCoroutine("Move");
    }

    private void Update() {
        if (queuedAnimation && !isMoving) {
            queuedAnimation = false;
            Interact();
        }
    }

    private void Interact() {
        if (!isMoving) StartCoroutine("Move");
        else { queuedAnimation = !queuedAnimation; }
    }

    private IEnumerator Move() {
        isMoving = true;
        while (isMoving) {
            while (curTime < moveDuration) {
                var t = curTime / moveDuration;
                gameObject.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, t),
                    Quaternion.Slerp(startRot, endRot, t));
                curTime += Time.deltaTime;
                yield return null;
            }
            gameObject.transform.SetPositionAndRotation(endPos, endRot);

            Vector3 tmpPos = startPos;
            Quaternion tmpRot = startRot;

            startPos = endPos;
            endPos = tmpPos;
            startRot = endRot;
            endRot = tmpRot;

            curTime = 0f;
            if (!moveForeverOnceActive) isMoving = false;
        }
    }
}
