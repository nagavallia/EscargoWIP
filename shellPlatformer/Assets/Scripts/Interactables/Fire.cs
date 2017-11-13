using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {


	public bool everlastingFlames = false;

	void OnTriggerEnter2D (Collider2D collision)
	{
		if (!everlastingFlames) {
			
			if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC" || collision.gameObject.tag == "Water") {
				gameObject.SetActive (false);
			} else if (collision.gameObject.tag == "Shell") {
				if (collision.gameObject.GetComponent<Shell> ().waterLevel > 0) {
					gameObject.SetActive (false);
					//collision.gameObject.SendMessage ("ShellDestroy", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}