using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	private Sprite current;
	private Sprite change;

	public bool everlastingFlames = false;

	public float waitSecs = 0.4f;

	void Start(){
		current = this.GetComponent<SpriteRenderer> ().sprite;
		change = Resources.Load <Sprite> ("fire_01");
		InvokeRepeating ("fireAnimation", 0, waitSecs);
	}

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (!everlastingFlames) {
			
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC" || collision.gameObject.tag == "Water") {
				gameObject.SetActive (false);
			} else if (collision.gameObject.tag == "Shell") {
				if (collision.gameObject.GetComponent<Shell> ().isFull()) {
					gameObject.SetActive (false);
					//collision.gameObject.SendMessage ("ShellDestroy", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	void fireAnimation(){
		if (this.GetComponent<SpriteRenderer> ().sprite == change) {
			this.GetComponent<SpriteRenderer> ().sprite = current;
		} else {
			this.GetComponent<SpriteRenderer> ().sprite = change;
		}
	}
}