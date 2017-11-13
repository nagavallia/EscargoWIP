using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
    [SerializeField] private float switchCooldown = 2f;
    public int id = 0;
    private float timeStamp = 0f;

	[SerializeField] private ColorCode SwitchColor = ColorCode.GREEN;
    [SerializeField] private bool ColorAttachedInteractables = true;

	private Sprite current;
	private Sprite change;

	enum ColorCode
	{
		BLUE,
		RED,
		YELLOW,
		GREEN
	}

    [SerializeField] private AudioClip ActivateSound;
    private AudioSource audioSource;

	private GameObject popUp;

    private void Start() {
		Color objColor = Color.white;

		switch (SwitchColor) { //change color of switch and attached interactables based on field
		case ColorCode.BLUE:
			current = Resources.Load<Sprite> ("LeverRightBlue");
			change = Resources.Load<Sprite> ("LeverLeftBlue");
			objColor = Color.blue;
			break;
		case ColorCode.GREEN:
			current = Resources.Load<Sprite> ("LeverRightGreen");
			change = Resources.Load<Sprite> ("LeverLeftGreen");
			objColor = Color.green;
			break;
		case ColorCode.RED:
			current = Resources.Load<Sprite> ("LeverRightRed");
			change = Resources.Load<Sprite> ("LeverLeftRed");
			objColor = Color.red;
			break;
		case ColorCode.YELLOW:
			current = Resources.Load<Sprite> ("LeverRightYellow");
			change = Resources.Load<Sprite> ("LeverLeftYellow");
			objColor = Color.yellow;
			break;
		}

        if (!ColorAttachedInteractables)
            objColor = Color.white;

		gameObject.GetComponent<SpriteRenderer> ().sprite = current;
		foreach (GameObject go in interactables) {
			var spriterendScript = go.GetComponent<SpriteRenderer> ();
			var buttonScript = go.GetComponent<Button> ();
			var switchScript = go.GetComponent<Switch> ();

			if (spriterendScript != null && buttonScript == null && switchScript == null) {
				spriterendScript.color = objColor;
			}
		}

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ActivateSound;
		popUp = (GameObject) Instantiate(Resources.Load("interactPopup"), transform);
		popUp.transform.position = new Vector3 (transform.position.x, transform.position.y + 1f, 0);
		popUp.SetActive (false);
    }

    // Checks if colliding objects is not a trigger and if current time
    // is larger than timeStamp. timeStamp is set to current time plus
    // "switchCooldown"
    private void OnTriggerStay2D(Collider2D collision) {
			
		bool use = Input.GetButtonDown ("Use");

		if (use && collision.attachedRigidbody.tag == "Player") {
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

				timeStamp = Time.time + switchCooldown/10f;

				audioSource.Play (); // play activate sound

				// log that the switch has been used and the location
				Managers.logging.RecordEvent (5, "" + gameObject.transform.position);
			}
		}
			
	} 

	// same as onTriggerStay2D but with only enter for the shell and the npc snail
	private void OnTriggerEnter2D(Collider2D collision) {

		// load Interact popup if snail enters
		if (collision.tag == "Player") {
			popUp.SetActive (true);
		}

		if (collision.tag == "Shell" && collision.gameObject.transform.parent == null || collision.gameObject.tag == "NPC") {
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
			}
		}

	} 
	private void OnTriggerExit2D(Collider2D collision){
		popUp.SetActive (false);
	}
		
}
