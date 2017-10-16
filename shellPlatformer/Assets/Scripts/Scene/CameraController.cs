using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Camera cam; // this camera
	private Transform player; // Reference to the player gameobject
    [SerializeField] private Collider2D bbox; // bounding box game object

	[SerializeField] private float offsetX = 3f; // Unity unit size of deadzone
    [SerializeField] private float offsetY = 2f; //

    private float camHeight; // Unity unit distance from camera center to top
    private float camWidth; // Unity unit distance from camera center to side

    private Vector3 min, max; // min/max positions of the camera center

	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponent<Camera>();

        player = GameObject.FindWithTag("Player").transform;

        camHeight = cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;

        min = bbox.bounds.min + new Vector3(camWidth, camHeight, 0f);
        max = bbox.bounds.max - new Vector3(camWidth, camHeight, 0f);

        if (min.x > max.x) {
            min = new Vector3(bbox.bounds.center.x, min.y, min.z);
            max = new Vector3(bbox.bounds.center.x, max.y, max.z);
        } 
        if (min.y > max.y) {
            min = new Vector3(min.x, bbox.bounds.center.y, min.z);
            max = new Vector3(max.x, bbox.bounds.center.y, max.z);
        }
    }

	void Update () {
        float newX, newY;
        Vector3 playerPos = player.position;
        Vector3 camPos = transform.position;

        if (playerPos.x > camPos.x + offsetX) newX = playerPos.x - offsetX;
        else if (playerPos.x < camPos.x - offsetX) newX = playerPos.x + offsetX;
        else newX = camPos.x;

        if (playerPos.y > camPos.y + offsetY) newY = playerPos.y - offsetY;
        else if (playerPos.y < camPos.y - offsetY) newY = playerPos.y + offsetY;
        else newY = camPos.y;

        newX = Mathf.Clamp(newX, min.x, max.x);
        newY = Mathf.Clamp(newY, min.y, max.y);

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
