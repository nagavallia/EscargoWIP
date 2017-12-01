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
	[SerializeField] private Sprite chargedMagnet;
	[SerializeField] private Sprite chargedMagnet2;
    [SerializeField] private AudioClip retrieveSound, spawnSound;
    private AudioSource audioSource;
    private int shellsSpawned = 0;
	private Sprite originalMagnet;



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

		originalMagnet = this.GetComponent<SpriteRenderer> ().sprite;

        audioSource = gameObject.AddComponent<AudioSource>();

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

		audioSource.PlayOneShot (spawnSound);

        //log that a shell spawner has been used and the position of the player
  
        Managers.logging.RecordEvent(4, "" + player.transform.position);
    }

    private void ReturnShell()
    {
		if (lockedOn) {
			Debug.Log("dropping");
			shell.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 1;
			lockedOn = false;
			StopAllCoroutines ();
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = originalMagnet;
		}
		else if (player.transform.Find ("Shell") == null && !isMoving) {
            audioSource.PlayOneShot(retrieveSound);
			StartCoroutine("Move");
			StartCoroutine ("magnetAnim");
//			shell.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
//			shell.position = this.transform.position + new Vector3 (0, -0.6f, 0);
		}
    }

	private IEnumerator Move() {
		isMoving = true;
		startPos = shell.gameObject.GetComponent<Rigidbody2D> ().position;
		shell.gameObject.layer = LayerMask.NameToLayer ("No Collison");
		float dist = Vector2.Distance ((Vector2)startPos, (Vector2)endPos);
		while (isMoving) {
			if (dist < 7.5f)
				moveDuration = 1;
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

		if (shouldSpawnNewShell) {
			if (shellsSpawned < maxShells) {
				SpawnShell ();

				// log that the switch for spawn shell has been triggered
				Managers.logging.RecordEvent (10, "" + player.transform.position);
			}
		}


		else {
			ReturnShell ();

			// log that the switch for retireve shell has been triggered
			Managers.logging.RecordEvent (11, "" + player.transform.position);
		}
		
	}

	IEnumerator magnetAnim(){
		while (true) {
			
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = chargedMagnet;
			yield return new WaitForSeconds (0.1f);
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = chargedMagnet2;
			yield return new WaitForSeconds (0.1f);
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = originalMagnet;
			yield return new WaitForSeconds (0.1f);
		}
		
	}
		
}

