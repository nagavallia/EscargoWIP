using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour {
    //[SerializeField] private GameObject head;
    [SerializeField] private GameObject water;
    [SerializeField] private bool startEnabled = false;

	// Use this for initialization
	void Start () {
        if (startEnabled) water.SetActive(true);
        else water.SetActive(false);
	}

	private void Interact() {
		water.SetActive (!water.activeInHierarchy);
	}

    private void OnTriggerEnter2D (Collider2D collision)
	{
        collision.gameObject.SendMessage("FillShell", SendMessageOptions.DontRequireReceiver);
	}
		
}
