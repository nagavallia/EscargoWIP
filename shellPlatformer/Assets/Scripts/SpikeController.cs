using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D collision) 
	{
		collision.gameObject.SendMessage("KillPlayer", this.gameObject, SendMessageOptions.DontRequireReceiver);
	}
}
