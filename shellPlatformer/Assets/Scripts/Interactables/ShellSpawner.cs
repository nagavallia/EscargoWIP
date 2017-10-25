using UnityEngine;
using System.Collections;

public class ShellSpawner : MonoBehaviour
{
	private GameObject shell;
	private BoxCollider2D spawnerHitbox;
	private GameObject player;

	[SerializeField] private int maxShells = 1;
	private int shellsSpawned = 0;

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
		if (collision.gameObject.tag == "Player" && shellsSpawned < maxShells)
		{
			SpawnShell ();
		}
	}

	private void SpawnShell() {
		shell = GameObject.Find ("Shell");
		GameObject newShell = Instantiate (shell);
		newShell.transform.position = this.transform.position;
		shellsSpawned++;

		// log that a shell spawner has been used and the position of the player
		Managers.logging.RecordEvent (4, "" + player.transform.position);
	}
}

