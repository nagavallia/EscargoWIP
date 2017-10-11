using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingIntializer : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// only do this once in the game, so when gameStart boolean is false
		if (!LoggingManager.gameStart) {

			// Initialize the logging
			LoggingManager.instance.Initialize (889, 0, false);

			// Start the Game Logging
			LoggingManager.instance.RecordPageLoad ();

			// set the boolean to true and never change it back
			LoggingManager.gameStart = true;
		}
	}
}
