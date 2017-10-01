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
		player = GameObject.Find ("Snail");
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == player) {
			shell = GameObject.Find ("Shell");
			shell.transform.SetParent (player.transform);
			shell.transform.localPosition = new Vector2 (0, 0);
		}
	}	
}

