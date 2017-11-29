using UnityEngine;
using System.Collections;

public class ShellSpawner : MonoBehaviour
{
	private Transform shell;
	private BoxCollider2D spawnerHitbox;
	private GameObject player;

	private Sprite current;
	private Sprite green;

	private float curTime;
	private bool isMoving;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool lockedOn;

	[SerializeField] private float moveDuration = 2.0f;
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

		endPos = this.transform.position + new Vector3 (0, -0.5f, 0);
		lockedOn = false;

	}

	void Update()
	{
		
	}
	
	// Update is called once per frame
	private void TriggerInteraction(Collider2D collision)
	{
	  	if (collision.gameObject.tag == "Player" && shellsSpawned <= maxShells && player.transform.Find("Shell") == null)
		{
			if (shouldSpawnNewShell) {
				SpawnShell ();
			} else {
				ReturnShell ();
			}
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
		if (lockedOn) {
			Debug.Log("dropping");
			shell.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1;
			lockedOn = false;
		}
		else if (player.transform.Find ("Shell") == null && !isMoving) {
			StartCoroutine("Move");
//			shell.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
//			shell.position = this.transform.position + new Vector3 (0, -0.6f, 0);
		}
    }

	private IEnumerator Move() {
		isMoving = true;
		startPos = shell.gameObject.GetComponent<Rigidbody2D> ().position;
		shell.gameObject.layer = LayerMask.NameToLayer ("No Collison");
		while (isMoving) {
			while (curTime < moveDuration) {
				var t = curTime / moveDuration;
				shell.gameObject.transform.SetPositionAndRotation(Vector3.Lerp(startPos, endPos, t),
					Quaternion.identity);
				curTime += Time.deltaTime;
				yield return null;
			}
			shell.gameObject.transform.SetPositionAndRotation(endPos, Quaternion.identity);
			shell.gameObject.layer = LayerMask.NameToLayer ("Shell");
			shell.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			shell.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
			curTime = 0f;
			isMoving = false;
			lockedOn = true;
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

