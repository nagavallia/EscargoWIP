using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSpawnerSwitch : MonoBehaviour {
	
	[SerializeField] private float switchCooldown = 2f;
	private float timeStamp = 0f;

	private Sprite current;
	private Sprite change;

	[SerializeField] private AudioClip ActivateSound;
	private AudioSource audioSource;

	private GameObject popUp;

	public Vector3 offset = new Vector3(0,1,0);

	private void Start() {

		current = Resources.Load<Sprite> ("LeverRightBlue");
		change = Resources.Load<Sprite> ("LeverLeftBlue");

		gameObject.GetComponent<SpriteRenderer> ().sprite = current;

		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = ActivateSound;
		popUp = (GameObject) Instantiate(Resources.Load("interactPopup"));
		popUp.transform.SetParent (this.transform);
		popUp.transform.position = transform.position + offset;
		popUp.SetActive (false);
	}

	private void OnTriggerEnter2D(Collider2D collision){
		
		// load Interact popup if snail enters
		if (collision.tag == "Player") {
			popUp.SetActive (true);
		}
	}

	// Checks if colliding objects is not a trigger and if current time
	// is larger than timeStamp. timeStamp is set to current time plus
	// "switchCooldown"
	private void OnTriggerStay2D(Collider2D collision) {

		bool use = Input.GetButtonDown ("Use");

		if (use && collision.attachedRigidbody.tag == "Player") {
			if (Time.time >= timeStamp && !collision.isTrigger) {

				// Call switch trigger in the shellSpawner script
				this.transform.parent.GetComponent<ShellSpawner>().switchTrigger();

				// Change the image to reflect the interaction
				if (gameObject.GetComponent<SpriteRenderer> ().sprite == current) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = change;
				} else {
					gameObject.GetComponent<SpriteRenderer> ().sprite = current;
				}

				timeStamp = Time.time + switchCooldown/10f;

				audioSource.Play (); // play activate sound
			}
		}

	} 


	private void OnTriggerExit2D(Collider2D collision){
		popUp.SetActive (false);
	}
}
