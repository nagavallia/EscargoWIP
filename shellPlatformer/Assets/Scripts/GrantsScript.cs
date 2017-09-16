using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantsScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    private void Awake()
    {
		//create message listener, pick function to call in response
		Messenger.AddListener(GameEvent.LEVEL_COMPLETE, LevelFinish); 
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, LevelFinish);
    }

	//TODO: fill in code for finishing level
    private void LevelFinish()  
    {
        Debug.Log("u did it");
    }
}
