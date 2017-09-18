using UnityEngine;
using System.Collections;

public class ShellSpawner : MonoBehaviour
{
	private GameObject shell;
	private BoxCollider2D spawnerHitbox;

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
		PlayerController player = collision.gameObject.GetComponent<PlayerController> ();

		if (player != null) 
		{
			shell.transform.SetParent (player.gameObject.transform);
		}
	}	
}

