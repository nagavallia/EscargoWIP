using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	[SerializeField] private bool startEnabled = false;
	[SerializeField] private Sprite locked, unlocked;
	private bool wasActive;
	private AudioClip activateSound, deactivateSound;
	private AudioSource audioSource;

	void Start ()
	{
		wasActive = startEnabled;
		gameObject.GetComponent<SpriteRenderer> ().sprite = startEnabled ? unlocked : locked;
		foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
			collider.enabled = startEnabled; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
		}

		audioSource = gameObject.AddComponent<AudioSource> ();

		var active = "exit_appear"; // load in correct activate sound
		var deactive = "exit_disappear";

		activateSound = Resources.Load (active) as AudioClip;
		deactivateSound = Resources.Load (deactive) as AudioClip;
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		//get playerControl component from colliding object
		PlayerController player = collision.gameObject.GetComponent<PlayerController> ();         
		//if playerControl existed, ie actually collided with player
		if (player != null) {
			// record that the level has ended
			//Managers.logging.RecordLevelEnd();

			//broadcast level complete message. listeners handle level completion functionality
			Messenger.Broadcast (GameEvent.LEVEL_COMPLETE);

			// set the boolean for level start to false
			LoggingManager.lvlStart = false;
		}
	}

	void Interact ()
	{
		if (!wasActive) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = unlocked;
			foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
				collider.enabled = true; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
			}
			wasActive = true;
			
			audioSource.PlayOneShot (activateSound); // play activate or deactivate sound
		}
	}
}
