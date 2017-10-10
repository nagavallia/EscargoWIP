using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactKill : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D collision) 
	{
		collision.gameObject.SendMessage("Kill", this.gameObject, SendMessageOptions.DontRequireReceiver);
	}
}
