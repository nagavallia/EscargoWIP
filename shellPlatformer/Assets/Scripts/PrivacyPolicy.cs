using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyPolicy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void showPrivacyPolicy(){
		GameObject pp = (GameObject)Instantiate (Resources.Load ("PrivacyPolicy"));
		pp.transform.SetParent (transform.parent);
		pp.transform.position = transform.parent.position;
	}

	public void destroyPrivacyPolicy(){
		Destroy (this.gameObject);
	}
}
