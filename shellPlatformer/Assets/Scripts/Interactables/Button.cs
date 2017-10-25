﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
    public int id = 0;
    private List<GameObject> touching; // List of gameObjects colliding with button

	[SerializeField] private Sprite current;
	[SerializeField] private Sprite change;

    [SerializeField] private AudioClip ActivateSound, DeactivateSound;
    private AudioSource audioSource;

    private void Start() {
        touching = new List<GameObject>();

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // On every collision, add that game to touching.
    // If first game object collided, call Interact on all objects atatched. 
    private void OnTriggerEnter2D(Collider2D collision) {
        if (touching.Count == 0) {
            foreach (GameObject i in interactables) {
                i.SendMessage("TriggerInteraction", id);
            }
			// Change the image to reflect the interaction
			if (gameObject.GetComponent<SpriteRenderer> ().sprite == current) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = change;
			} else {
				gameObject.GetComponent<SpriteRenderer> ().sprite = current;
			}

            audioSource.PlayOneShot(ActivateSound); // play activate sound

			// log that the button has been used and the position of the button
			Managers.logging.RecordEvent(6,"" + gameObject.transform.position);

        }
        touching.Add(collision.gameObject);
	} 

	private void OnTriggerExit2D(Collider2D collision) {
        touching.Remove(collision.gameObject);
        if (touching.Count == 0) {
            foreach (GameObject i in interactables) {
                i.SendMessage("TriggerInteraction", id);
            }
			// Change the image to reflect the interaction
			if (gameObject.GetComponent<SpriteRenderer> ().sprite == current) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = change;
			} else {
				gameObject.GetComponent<SpriteRenderer> ().sprite = current;
			}

            audioSource.PlayOneShot(DeactivateSound); // play deactivate sound
        }
	}  
}
