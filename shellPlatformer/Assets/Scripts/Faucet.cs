﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D (Collision2D collision)
	{
		collision.gameObject.SendMessage("FillShell", SendMessageOptions.DontRequireReceiver);
	}
		
}
