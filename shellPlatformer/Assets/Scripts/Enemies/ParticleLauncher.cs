using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

	//public ParticleSystem particleLauncher;
	public int intervalSecs = 2;

	public Vector3 offset = new Vector3(0,0,0);

	public int waitSecs = 0; 

	// Use this for initialization
	void Start () {
		StartCoroutine (emitSaltRoutine());
	}


//	void OnParticleCollision(GameObject other){
//		if (other.tag == "Player") {
//			Debug.Log ("salt collided with player");
//			
//			other.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
//		}
//	}

	void emitSalt(){

		// random number between 0 and 2
		int randomNum = Random.Range (0, 2);

		// if 0, do this function, else go to emitSalt2
		if (randomNum == 0) {
			GameObject salt = (GameObject)Instantiate (Resources.Load ("saltParticle"));
			Vector3 curPosition = transform.position;
			salt.transform.position = curPosition + offset + new Vector3 (-0.1f, 0, 0);
			salt.transform.rotation = Random.rotation; // add a random rotation to each salt particle
			salt.transform.rotation = new Quaternion(0, 0, salt.transform.rotation.z, salt.transform.rotation.w);
		} else {
			emitSalt2 ();
		}
			
	}
	void emitSalt2(){

		// random number between 0 and 2
		int randomNum = Random.Range (0, 2);

		// if 0, do this function, else go to emitSalt2
		if (randomNum == 0) {
			GameObject salt = (GameObject) Instantiate(Resources.Load("saltParticle"));
			Vector3 curPosition = transform.position;
			salt.transform.position = curPosition + offset + new Vector3 (0.1f, 0, 0);
			salt.transform.rotation = Random.rotation; // add a random rotation to each salt particle
			salt.transform.rotation = new Quaternion(0, 0, salt.transform.rotation.z, salt.transform.rotation.w);
		} else {
			emitSalt ();
		}
	}

	IEnumerator emitSaltRoutine(){
		yield return new WaitForSeconds (waitSecs);

		InvokeRepeating ("emitSalt", 0, intervalSecs);
		InvokeRepeating ("emitSalt", 0.1f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.2f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.3f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.4f, intervalSecs);
		InvokeRepeating ("emitSalt2", 0.05f, intervalSecs);
		InvokeRepeating ("emitSalt2", 0.15f, intervalSecs);
		InvokeRepeating ("emitSalt2", 0.25f, intervalSecs);
		InvokeRepeating ("emitSalt2", 0.35f, intervalSecs);
		InvokeRepeating ("emitSalt2", 0.45f, intervalSecs);

	}
}
