using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

	//public ParticleSystem particleLauncher;
	public float intervalSecs = 2.1f;

	public Vector3 offset = new Vector3(0,0,0);

	public float waitSecs = 0.1f; 

	public string particleName = "saltParticle";

	public float halfWidth = 0.1f;

	private bool interactCheck = true;

	[SerializeField]
	private bool startEnabled = true;

	// Use this for initialization
	void Start () {
		if (startEnabled) {
			StartCoroutine (emitParticleRoutine ());
		}

		interactCheck = startEnabled;
	}


//	void OnParticleCollision(GameObject other){
//		if (other.tag == "Player") {
//			Debug.Log ("salt collided with player");
//			
//			other.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
//		}
//	}

	void emitParticle(){

		// random number between 0 and 2
		int randomNum = Random.Range (0, 2);

		// if 0, do this function, else go to emitSalt2
		if (randomNum == 0) {
			GameObject particle = (GameObject)Instantiate (Resources.Load (particleName));
			Vector3 curPosition = transform.position;
			particle.transform.position = curPosition + offset + new Vector3 (-halfWidth, 0, 0);
			particle.transform.rotation = Random.rotation; // add a random rotation to each salt particle
			particle.transform.rotation = new Quaternion(0, 0, particle.transform.rotation.z, particle.transform.rotation.w);
		} else {
			emitParticle2 ();
		}
			
	}
	void emitParticle2(){

		// random number between 0 and 2
		int randomNum = Random.Range (0, 2);

		// if 0, do this function, else go to emitSalt2
		if (randomNum == 0) {
			GameObject particle = (GameObject) Instantiate(Resources.Load(particleName));
			Vector3 curPosition = transform.position;
			particle.transform.position = curPosition + offset + new Vector3 (halfWidth, 0, 0);
			particle.transform.rotation = Random.rotation; // add a random rotation to each salt particle
			particle.transform.rotation = new Quaternion(0, 0, particle.transform.rotation.z, particle.transform.rotation.w);
		} else {
			emitParticle();
		}
	}

	IEnumerator emitParticleRoutine(){
		yield return new WaitForSeconds (waitSecs);

		InvokeRepeating ("emitParticle", 0, intervalSecs);
		InvokeRepeating ("emitParticle", 0.1f, intervalSecs);
		InvokeRepeating ("emitParticle", 0.2f, intervalSecs);
		InvokeRepeating ("emitParticle", 0.3f, intervalSecs);
		InvokeRepeating ("emitParticle", 0.4f, intervalSecs);
		InvokeRepeating ("emitParticle2", 0.05f, intervalSecs);
		InvokeRepeating ("emitParticle2", 0.15f, intervalSecs);
		InvokeRepeating ("emitParticle2", 0.25f, intervalSecs);
		InvokeRepeating ("emitParticle2", 0.35f, intervalSecs);
		InvokeRepeating ("emitParticle2", 0.45f, intervalSecs);


	}

	public void Interact(){

		CancelInvoke ();

		if (!interactCheck) {
			StartCoroutine (emitParticleRoutine ());
		}

		interactCheck = !interactCheck;
	
	}
}
