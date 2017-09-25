using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		foreach (GameObject i in interactables) {
			i.SendMessage ("Interact");
		}
	} 

	private void OnTriggerExit2D(Collider2D collision) {
		foreach (GameObject i in interactables) {
			i.SendMessage ("Interact");
		}
	}  
}
