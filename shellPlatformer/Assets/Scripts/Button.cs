using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
	[SerializeField] private GameObject exit;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter2D(Collider2D collision) {  
		Instantiate(exit, new Vector3 (5f, 0f, 0f), transform.rotation);
	}  
}
