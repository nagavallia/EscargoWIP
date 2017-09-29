using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private GameObject shellSpawner;
	private bool isFull;

	void Start()
	{
		shellSpawner = GameObject.FindWithTag ("ShellSpawner");
		//isFull = false;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		/*
		if (collision.gameObject.tag == "Faucet") {
			isFull = true;
			Debug.Log ("Fill Her Up!");
		}
		*/
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        collision.gameObject.SendMessage("ShellCollide", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    private void ShellDestroy() {
        // do code here for when the shell should be destroyed
        // probably broadcast a SHELL_DESTROYED message that will be intercepted by a shell spawner
        Messenger.Broadcast(GameEvent.SHELL_DESTROYED, MessengerMode.DONT_REQUIRE_LISTENER);
        Destroy(this.gameObject);
    }
}
