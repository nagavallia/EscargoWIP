using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : MonoBehaviour {
    [SerializeField] private bool rotateRight = true;
    [SerializeField] private bool animateMovement = true;
    [SerializeField] private float animationDuration = 3f;

    [SerializeField] private AudioClip MovingSound, FinishSound;
    private AudioSource audioSource;

    private bool isAnimating; // is the bridge currently in the middle of an animation?
    private bool queuedAnimation; // do we have to activate the bridge again after the current animation finishes?

    private float angle; // rotation euler angle (about z axis)
    private float curTime;

    private void Start() {
        angle = rotateRight ? -90f : 90f;
        curTime = 0f;
        isAnimating = false;
        queuedAnimation = false;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update() {
        if (queuedAnimation && !isAnimating) {
            queuedAnimation = false;
            Interact();
        }
    }

    private void Interact() {
        if (!animateMovement) {
            transform.Rotate(0f, 0f, angle);
            angle *= -1;
        } else {
            if (isAnimating) queuedAnimation = !queuedAnimation;
            else {
                isAnimating = true;
                //audioSource.clip = MovingSound;
                //audioSource.Play();
                StartCoroutine("LowerBridge");
            }
        }
    }

    private IEnumerator LowerBridge() {
        float t = 0f;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.AngleAxis(transform.eulerAngles.z + angle, Vector3.forward);

        while (curTime < animationDuration) {
            curTime += Time.deltaTime;
            t = curTime / animationDuration;
            transform.SetPositionAndRotation(transform.position, Quaternion.Slerp(start, end, t));
            yield return null;
        }

        curTime = 0f;
        isAnimating = false;
        angle *= -1;
        transform.SetPositionAndRotation(transform.position, Quaternion.Slerp(start, end, 1f));

        //audioSource.Stop();
        //audioSource.PlayOneShot(FinishSound);
    }
	
}
