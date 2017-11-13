using UnityEngine;
using System.Collections;

public class ShellSpawner : MonoBehaviour
{
	private Transform shell;
	private BoxCollider2D spawnerHitbox;
	private GameObject player;

	private Sprite current;
	private Sprite green;

    [SerializeField] private bool shouldSpawnNewShell = false;
    [SerializeField] private int maxShells = 1;
    private int shellsSpawned = 0;

    // Use this for initialization
    void Start ()
	{
		player  = GameObject.Find ("snail");
        shell   = player.transform.Find ("Shell");
		Messenger.AddListener(GameEvent.SHELL_DESTROYED, SpawnShell);

		// set the sprites
		current = this.gameObject.GetComponent<SpriteRenderer>().sprite;
		green =  Resources.Load <Sprite> ("shellSpawnerGreen");

	}

	void Update()
	{
		if (player.transform.Find ("Shell") == null) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = green;

		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = current;
		}
			
	}
	
	// Update is called once per frame
	private void TriggerInteraction(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && shellsSpawned <= maxShells && player.transform.Find("Shell") == null)
		{
            if (shouldSpawnNewShell)
                SpawnShell();
            else
                ReturnShell();
		}
	}

    private void SpawnShell()
    {
        GameObject newShell = (GameObject)Instantiate(Resources.Load("Shell"));
		newShell.transform.position = this.transform.position;
		newShell.name = "Shell";
        shellsSpawned++;

        //log that a shell spawner has been used and the position of the player
  
        Managers.logging.RecordEvent(4, "" + player.transform.position);
    }

    private void ReturnShell()
    {
		if (player.transform.Find ("Shell") == null) {
			
			shell.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			shell.position = this.transform.position + new Vector3 (0, -0.6f, 0);
		}
    }

	public void switchTrigger(){
		
		if (shouldSpawnNewShell)
			SpawnShell ();
		else {
			ReturnShell ();
		}
		
	}
		
}

