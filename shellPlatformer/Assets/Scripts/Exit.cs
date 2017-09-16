using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		//get playerControl component from colliding object
        playerControl player = collision.gameObject.GetComponent<playerControl>();         
		//if playerControl existed, ie actually collided with player
		if (player != null) 
        {
			//broadcast level complete message
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE); 
        }
    }
}
