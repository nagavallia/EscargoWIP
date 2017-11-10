using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPopUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showControls(){
		GameObject pp = (GameObject)Instantiate (Resources.Load ("ControlsPopUp"));
		pp.transform.SetParent (transform.parent);
		pp.transform.position = transform.parent.position;
	}

	public void destroyControls(){
		Destroy (this.gameObject);
	}
}
