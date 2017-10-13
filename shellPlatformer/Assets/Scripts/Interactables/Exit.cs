﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
		//get playerControl component from colliding object
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();         
		//if playerControl existed, ie actually collided with player
		if (player != null) 
        {
            // record that the level has ended
            //Managers.logging.RecordLevelEnd();

            //broadcast level complete message. listeners handle level completion functionality
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);

			// set the boolean for level start to false
			LoggingManager.lvlStart = false;
        }
    }
}
