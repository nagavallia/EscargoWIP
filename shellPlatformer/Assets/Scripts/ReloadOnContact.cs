using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadOnContact : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) 
	{
		if (collision.gameObject.tag == "Player") {
			Messenger.Broadcast (GameEvent.RELOAD_LEVEL);
		}
	}
}
