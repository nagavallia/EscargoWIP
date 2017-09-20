using UnityEngine;
using System.Collections;

public class ShellSpawner : MonoBehaviour
{
	private GameObject shell;
	private BoxCollider2D spawnerHitbox;
	private PlayerController player;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && this.transform.Find ("Shell") != null) {
			shell = this.transform.Find("Shell").gameObject;
			shell.GetComponent<ShellThrower>().PickUpShell();
		}
	}	
}

