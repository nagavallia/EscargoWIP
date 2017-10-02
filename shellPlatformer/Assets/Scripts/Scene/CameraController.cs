using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player; // Reference to the player gameobjectt

	private Vector3 offset; // Private variable for the offset of the distance between camera and player


	// Use this for initialization
	void Start () {
		// Set the offset of the camera by calculating the distance 
		offset = transform.position - player.transform.position;
	}
	
	// LateUpdate is called after Update each frame
	void Update () {

		// Set the camera position to the player position with the calculated offset distance
		transform.position = player.transform.position + offset;

		
	}
}
