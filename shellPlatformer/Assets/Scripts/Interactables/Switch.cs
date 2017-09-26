using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {
	[SerializeField] private List<GameObject> interactables;
    [SerializeField] private float switchCooldown = 2f;
    private float timeStamp = 0f;

    // Checks if colliding objects is not a trigger and if current time
    // is larger than timeStamp. timeStamp is set to current time plus
    // "switchCooldown"
	private void OnTriggerEnter2D(Collider2D collision) {
        if (Time.time >= timeStamp && !collision.isTrigger) {
            foreach (GameObject i in interactables) {
                i.SendMessage("Interact");
            }
            timeStamp = Time.time + switchCooldown;
        }
	} 
}
