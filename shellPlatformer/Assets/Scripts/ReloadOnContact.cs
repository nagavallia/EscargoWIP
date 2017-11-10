using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadOnContact : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) 
	{
		if (collision.gameObject.tag == "Player") {
            Debug.Log("Reloading from Pitfall");
			Messenger.Broadcast (GameEvent.RELOAD_LEVEL);
		}
        else
        {
            Debug.Log("Not a player");
        }
	}
}
