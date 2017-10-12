using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
    [SerializeField] private float switchCooldown = 2f;
    public int id = 0;
    private float timeStamp = 0f;

	[SerializeField] private Sprite current;
	[SerializeField] private Sprite change;

    // Checks if colliding objects is not a trigger and if current time
    // is larger than timeStamp. timeStamp is set to current time plus
    // "switchCooldown"
	private void OnTriggerEnter2D(Collider2D collision) {
        if (Time.time >= timeStamp && !collision.isTrigger) {
            foreach (GameObject i in interactables) {
                i.SendMessage("TriggerInteraction", id);
            }

			// Change the image to reflect the interaction
			if (gameObject.GetComponent<SpriteRenderer> ().sprite == current) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = change;
			} else {
				gameObject.GetComponent<SpriteRenderer> ().sprite = current;
			}

            timeStamp = Time.time + switchCooldown;

			// log that the switch has been used and the location
			Managers.logging.RecordEvent(5, "" + gameObject.transform.position);
        }
			
	} 
}
