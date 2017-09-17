using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    Scene curScene;

	// Use this for initialization
	void Start () {
        curScene = SceneManager.GetActiveScene();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(curScene.name); }
	}
}
