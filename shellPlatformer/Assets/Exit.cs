using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerControl player = collision.gameObject.GetComponent<playerControl>(); //get playerControl component from colliding object
        if (player != null) //if playerControl existed, ie actually collided with player
        {
            Messenger.Broadcast("LEVEL_COMPLETE"); //broadcast level complete message
        }
    }
}
