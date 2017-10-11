using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoggingLevelStart : MonoBehaviour {

	private int level = 0;

	// Use this for initialization
	void Start () {
		Scene scene = SceneManager.GetActiveScene();


		if (scene.name == "easy_level") {
			level = 1;

		} else if (scene.name == "medium_level") {
			level = 2;

		} else if (scene.name == "hard_level"){
			level = 3;
		}

		if (!LoggingManager.lvlStart) {
			
			// Logging record the start of the level with number level
			LoggingManager.instance.RecordLevelStart (level);

			LoggingManager.lvlStart = true; // set the boolean for level start to true
		}
	}
}
