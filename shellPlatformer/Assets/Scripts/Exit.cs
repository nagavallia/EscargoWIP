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
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();         
		//if playerControl existed, ie actually collided with player
		if (player != null) 
        {
			//broadcast level complete message
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE); 
			player.gameObject.transform.SetPositionAndRotation (new Vector3 (-4.84f, -2.748991f, 0f), transform.rotation);
        }
    }
}
