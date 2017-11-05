﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSwitch : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
	[SerializeField] private float switchCooldown = 2f;
	public int id = 0;
	private float timeStamp = 0f;

	[SerializeField] private Sprite current;
	[SerializeField] private Sprite change;

	[SerializeField] private AudioClip ActivateSound;
	private AudioSource audioSource;

	private bool triggered; // exit only triggers once. once on, it never deactivates.

	private void Start() {
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = ActivateSound;

		triggered = false;
	}

	// Checks if colliding objects is not a trigger and if current time
	// is larger than timeStamp. timeStamp is set to current time plus
	// "switchCooldown"
	private void OnTriggerStay2D(Collider2D collision) {

		bool use = Input.GetButtonDown ("Use");

		if (use && !triggered) {
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

				// log that the switch has been used and the location
				Managers.logging.RecordEvent (5, "" + gameObject.transform.position);

				triggered = true;
			}
		}

	} 

	// same as onTriggerStay2D but with only enter for the shell and the npc snail
	private void OnTriggerEnter2D(Collider2D collision) {
		
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

				// log that the switch has been used and the location
				Managers.logging.RecordEvent (5, "" + gameObject.transform.position);

				triggered = true;
			}
		}

	} 
}