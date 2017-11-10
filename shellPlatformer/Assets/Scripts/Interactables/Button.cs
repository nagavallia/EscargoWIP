using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
    public int id = 0;
    private List<GameObject> touching; // List of gameObjects colliding with button

	[SerializeField] private ColorCode ButtonColor = ColorCode.GREEN;
    [SerializeField] private bool ColorAttachedInteractables = true;

	private Sprite current;
	private Sprite change;

    [SerializeField] private AudioClip ActivateSound, DeactivateSound;
    private AudioSource audioSource;

	enum ColorCode
	{
		BLUE,
		RED,
		YELLOW,
		GREEN
	}

    private void Start() {
        touching = new List<GameObject>();

        audioSource = gameObject.AddComponent<AudioSource>();

		Color objColor = Color.white;

		switch (ButtonColor) { //change color of button and attached interactables based on field
		case ColorCode.BLUE:
			current = Resources.Load<Sprite> ("ButtonUpBlue");
			change = Resources.Load<Sprite> ("ButtonDownBlue");
			objColor = Color.blue;
			break;
		case ColorCode.GREEN:
			current = Resources.Load<Sprite> ("ButtonUpGreen");
			change = Resources.Load<Sprite> ("ButtonDownGreen");
			objColor = Color.green;
			break;
		case ColorCode.RED:
			current = Resources.Load<Sprite> ("ButtonUpRed");
			change = Resources.Load<Sprite> ("ButtonDownRed");
			objColor = Color.red;
			break;
		case ColorCode.YELLOW:
			current = Resources.Load<Sprite> ("ButtonUpYellow");
			change = Resources.Load<Sprite> ("ButtonDownYellow");
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
			if (Managers.logging != null)
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
