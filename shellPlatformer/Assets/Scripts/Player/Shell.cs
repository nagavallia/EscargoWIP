using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private GameObject shellSpawner;
	public int maxWaterLevel;
	public int waterLevel;

    private Sprite normalShell;
	[SerializeField] private Sprite fullShell;

    [SerializeField] private AudioClip CollideSound, FillSound;
    private AudioSource audioSource;

	void Start()
	{
		shellSpawner = GameObject.FindWithTag ("ShellSpawner");
		waterLevel = 0;
		maxWaterLevel = 20;

        // load the fullShell sprite
        normalShell = this.GetComponent<SpriteRenderer>().sprite;

        audioSource = gameObject.AddComponent<AudioSource>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		// Shell will keep velocity if colliding with Salt
		if (collision.gameObject.tag != "Salt" && collision.gameObject.tag != "Water") {
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}
		if (collision.gameObject.tag != "Water") {
			audioSource.PlayOneShot (CollideSound);
		} else {
			if (!audioSource.isPlaying)
			audioSource.PlayOneShot (FillSound);
		}

        collision.gameObject.SendMessage("ShellCollide", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }

	private void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.tag != "Salt" && collision.gameObject.tag != "Water") {
			Vector3 vel = gameObject.GetComponent<Rigidbody2D> ().velocity;
			gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector3(0f, vel.y, 0f);
		}
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
			waterLevel=maxWaterLevel;
			this.GetComponent<SpriteRenderer> ().sprite = fullShell;

            //audioSource.PlayOneShot(FillSound);
		}
	}

	public void EmptyShell()
	{
		waterLevel = 0;
        this.GetComponent<SpriteRenderer>().sprite = normalShell;
	}
}
