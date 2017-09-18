﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private GameObject shellSpawner;

	void Start()
	{
		shellSpawner = GameObject.FindWithTag ("ShellSpawner");
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("ShellCollide", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    private void ShellDestroy() {
        // do code here for when the shell should be destroyed
        // probably broadcast a SHELL_DESTROYED message that will be intercepted by a shell spawner
		this.gameObject.transform.SetParent(shellSpawner.transform);
    }
}
