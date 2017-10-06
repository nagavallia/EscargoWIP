using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player; // Reference to the player gameobjectt

	private Vector3 offset; // Private variable for the offset of the distance between camera and player

	Vector3 velocity = Vector3.zero; // Velocity of camera

	public float smoothTime = .15f; // The time to move the camera to player position

	// Use this for initialization
	void Start () {
		// Set the offset of the camera by calculating the distance 
		offset = transform.position - player.transform.position;

		// Set the camera position to the player position with the calculated offset distance
		transform.position = player.transform.position + new Vector3(0,0,offset.z);
	}

	void FixedUpdate () {

		transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + new Vector3 (0, 0, offset.z), ref velocity , smoothTime);


//		if (Mathf.Abs(player.transform.position.x - transform.position.x) > 2
//			|| Mathf.Abs(player.transform.position.y - transform.position.y) > 2) {
//			transform.position = player.transform.position + new Vector3(0,0,offset.z);
//			transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + new Vector3 (0, 0, offset.z), ref velocity , smoothTime);
//		}

		
	}
}
