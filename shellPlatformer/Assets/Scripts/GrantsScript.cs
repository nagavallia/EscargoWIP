using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, LevelFinish); //create message listener, pick function to call in response
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, LevelFinish);
    }

    private void LevelFinish() //fill in code for finishing level 
    {
        Debug.Log("u did it");
    }
}
