using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
    private List<GameObject> touching; // List of gameObjects colliding with button

    private void Start() {
        touching = new List<GameObject>();
    }

    // On every collision, add that game to touching.
    // If first game object collided, call Interact on all objects atatched. 
    private void OnTriggerEnter2D(Collider2D collision) {
        if (touching.Count == 0) {
            foreach (GameObject i in interactables) {
                i.SendMessage("Interact");
            }
        }
        touching.Add(collision.gameObject);
	} 

	private void OnTriggerExit2D(Collider2D collision) {
        touching.Remove(collision.gameObject);
        if (touching.Count == 0) {
            foreach (GameObject i in interactables) {
                i.SendMessage("Interact");
            }
        }
	}  
}
