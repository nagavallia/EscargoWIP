using UnityEngine;
using System.Collections;

public class ShellSpawner : MonoBehaviour
{
	private GameObject shell;
	private BoxCollider2D spawnerHitbox;
	private GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find ("snail");
		Messenger.AddListener(GameEvent.SHELL_DESTROYED, SpawnShell);

	}

	void Update()
	{
	}
	
	// Update is called once per frame
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && player.transform.Find ("Shell") == null && this.transform.Find("Shell") == null) 
		{
			SpawnShell ();
		}
	}

	private void SpawnShell() {
		shell = GameObject.Find ("Shell");
		shell.transform.SetParent (this.transform);
		shell.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		shell.transform.localPosition = new Vector3 (0, 5, 0);
		shell.transform.SetParent (null);

		// log that a shell spawner has been used and the position of the player
		LoggingManager.instance.RecordEvent (4, "" + player.transform.position);
	}
}

