using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) 
	{
		collision.gameObject.SendMessage("KillPlayer", this.gameObject, SendMessageOptions.DontRequireReceiver);
	}
}
