using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour {

	[SerializeField]
	public GameObject player;

	void OnTriggerEnter2D(Collider2D collision) {
		Debug.Log (collision.name);
		player.SendMessage ("groundCheck", true);
	}

	void OnTriggerStay2D(Collider2D collision) {
		player.SendMessage ("groundCheck", true);
	}

	void OnTriggerExit2D(Collider2D collision) {
		player.SendMessage ("groundCheck", false);
	}
}
