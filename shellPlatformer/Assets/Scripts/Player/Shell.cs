using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private GameObject shellSpawner;
	private int maxWaterLevel;
	public int waterLevel;
	private Sprite fullShell;

	void Start()
	{
		shellSpawner = GameObject.FindWithTag ("ShellSpawner");
		waterLevel = 0;
		maxWaterLevel = 1;

		// load the fullShell sprite
		fullShell = Resources.Load<Sprite>("fullShell");
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        collision.gameObject.SendMessage("ShellCollide", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    private void ShellDestroy() {
        // do code here for when the shell should be destroyed
        // probably broadcast a SHELL_DESTROYED message that will be intercepted by a shell spawner
        Messenger.Broadcast(GameEvent.SHELL_DESTROYED, MessengerMode.DONT_REQUIRE_LISTENER);
        Destroy(this.gameObject);
    }

	public void FillShell() {
		Debug.Log ("Filling Shell");
		if (waterLevel < maxWaterLevel) {
			waterLevel++;
			this.GetComponent<SpriteRenderer> ().sprite = fullShell;
		}
	}

	public void EmptyShell()
	{
		waterLevel = 0;
	}
}
