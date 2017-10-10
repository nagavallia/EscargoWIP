using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingIntializer : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// Initialize the logging
		LoggingManager.instance.Initialize (889, 0, false);

		// Start the Game Logging
		LoggingManager.instance.RecordPageLoad ();
	}
}
