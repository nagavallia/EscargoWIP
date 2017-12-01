using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	[SerializeField] private bool startEnabled = false;
	[SerializeField] private Sprite locked, unlocked, mid;
	private bool wasActive;
	private AudioClip activateSound, deactivateSound, completeSound;
	private AudioSource audioSource;

	void Start ()
	{
		wasActive = startEnabled;
		gameObject.GetComponent<SpriteRenderer> ().sprite = startEnabled ? unlocked : locked;
		foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
			collider.enabled = startEnabled; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
		}

		audioSource = gameObject.AddComponent<AudioSource> ();
        audioSource.volume = 0.667f;

		var active = "exit_appear"; // load in correct activate sound
		var deactive = "exit_disappear";

		activateSound = Resources.Load (active) as AudioClip;
		deactivateSound = Resources.Load (deactive) as AudioClip;
        completeSound = Resources.Load("level_finish") as AudioClip;
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		//get playerControl component from colliding object
		PlayerController player = collision.gameObject.GetComponent<PlayerController> ();         
		//if playerControl existed, ie actually collided with player
		if (player != null) {

            audioSource.PlayOneShot(completeSound);
			StartCoroutine(exitRoutine(collision.gameObject));
				
			// record that the level has ended
			//Managers.logging.RecordLevelEnd();

			//broadcast level complete message. listeners handle level completion functionality
			//Messenger.Broadcast (GameEvent.LEVEL_COMPLETE);

			// set the boolean for level start to false
			//LoggingManager.lvlStart = false;
		}
	}

	void Interact ()
	{
		if (!wasActive) {
//			gameObject.GetComponent<SpriteRenderer> ().sprite = unlocked;
//			foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
//				collider.enabled = true; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
//			}
//			wasActive = true;
//			
//			audioSource.PlayOneShot (activateSound); // play activate or deactivate sound
			StartCoroutine("exitAnim");
		}
	}

	IEnumerator exitRoutine(GameObject player){

		// Destroy all the children of the snail
		int childs = player.transform.childCount;

		for (int i = childs - 1; i >= 0; i--) {
			Destroy (player.transform.GetChild (i).gameObject);
		}
			

		// Change the bool of player controller
		player.GetComponent<PlayerController>().hasExited(transform.position);
		player.GetComponent<PlayerController>().exitAnimation = true;

		player.GetComponent<Animator> ().SetInteger ("State", 5);

		yield return new WaitForSeconds (1);

		//broadcast level complete message. listeners handle level completion functionality
		Messenger.Broadcast (GameEvent.LEVEL_COMPLETE);

		// set the boolean for level start to false
		LoggingManager.lvlStart = false;

	}

	IEnumerator exitAnim(){
		this.GetComponent<SpriteRenderer> ().sprite = mid;
		GameObject boom = (GameObject)Instantiate (Resources.Load ("boom"));
		boom.transform.position = this.transform.position + new Vector3(0f, 0f,0);
		yield return new WaitForSeconds (0.5f);

		Destroy (boom);

		GameObject boom1 = (GameObject)Instantiate (Resources.Load ("boom"));
		boom1.transform.position = this.transform.position + new Vector3(0.5f, 0.5f,0);
		yield return new WaitForSeconds (0.5f);

		Destroy (boom1);

		GameObject boom2 = (GameObject)Instantiate (Resources.Load ("bigboom"));
		boom2.transform.position = this.transform.position + new Vector3(0f, 0f,0);
		yield return new WaitForSeconds (0.5f);

		Destroy (boom2);

		gameObject.GetComponent<SpriteRenderer> ().sprite = unlocked;
		foreach (Collider2D collider in gameObject.GetComponents<Collider2D>()) {
			collider.enabled = true; // Disable all colliders attatched to this gameObject. Note: might want to only disable triggers, we'll see.
		}
		wasActive = true;

		audioSource.PlayOneShot (activateSound); // play activate or deactivate sound
	}
}
