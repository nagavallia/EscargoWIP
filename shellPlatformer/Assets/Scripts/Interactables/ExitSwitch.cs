using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitch : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
	[SerializeField] private float switchCooldown = 2f;
	public int id = 0;
	private float timeStamp = 0f;

	[SerializeField] private Sprite current;
	[SerializeField] private Sprite change;

	private GameObject popUp;

	[SerializeField] private AudioClip ActivateSound;
	private AudioSource audioSource;

	private bool triggered; // exit only triggers once. once on, it never deactivates.

	private void Start() {
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = ActivateSound;

		popUp = (GameObject) Instantiate(Resources.Load("interactPopup"), transform);
		popUp.transform.position = new Vector3 (transform.position.x, transform.position.y + 1f, 0);
		popUp.SetActive (false);

		triggered = false;
	}

	// Checks if colliding objects is not a trigger and if current time
	// is larger than timeStamp. timeStamp is set to current time plus
	// "switchCooldown"
	private void OnTriggerStay2D(Collider2D collision) {

		bool use = Input.GetButtonDown ("Use");

		if (use && !triggered && collision.attachedRigidbody.tag == "Player") {
			if (Time.time >= timeStamp && !collision.isTrigger) {
				foreach (GameObject i in interactables) {
					i.SendMessage ("TriggerInteraction", id);
				}

				// Change the image to reflect the interaction
				if (gameObject.GetComponent<SpriteRenderer> ().sprite == current) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = change;
				} else {
					gameObject.GetComponent<SpriteRenderer> ().sprite = current;
				}

				timeStamp = Time.time + switchCooldown;

				audioSource.Play (); // play activate sound

				if (popUp.activeInHierarchy)
					popUp.SetActive (false);
				triggered = true;

				// log that the switch has been used and the location
				Managers.logging.RecordEvent (5, "" + gameObject.transform.position);
			}
		}

	} 

	// same as onTriggerStay2D but with only enter for the shell and the npc snail
	private void OnTriggerEnter2D(Collider2D collision) {

		if (collision.tag == "Player" && !triggered)
			popUp.SetActive (true);
		
		if (!triggered && ((collision.tag == "Shell" && collision.gameObject.transform.parent == null) || collision.gameObject.tag == "NPC")) {
			if (Time.time >= timeStamp && !collision.isTrigger) {
				foreach (GameObject i in interactables) {
					i.SendMessage ("TriggerInteraction", id);
				}

				// Change the image to reflect the interaction
				if (gameObject.GetComponent<SpriteRenderer> ().sprite == current) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = change;
				} else {
					gameObject.GetComponent<SpriteRenderer> ().sprite = current;
				}

				timeStamp = Time.time + switchCooldown;

				audioSource.Play (); // play activate sound

				popUp.SetActive (false);
				triggered = true;

				// log that the switch has been used and the location
				Managers.logging.RecordEvent (5, "" + gameObject.transform.position);
			}
		}

	}

	private void OnTriggerExit2D(Collider2D collision) {
		popUp.SetActive (false);
	}
}
