using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

	public ParticleSystem particleLauncher;
	public int intervalSecs = 2;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("emitSalt", 0, intervalSecs);
		InvokeRepeating ("emitSalt", 0.1f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.2f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.3f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.4f, intervalSecs);
		InvokeRepeating ("emitSalt", 0, intervalSecs);
		InvokeRepeating ("emitSalt", 0.1f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.2f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.3f, intervalSecs);
		InvokeRepeating ("emitSalt", 0.4f, intervalSecs);
	}


	void OnParticleCollision(GameObject other){
<<<<<<< HEAD

		if (other.tag == "Player") {
			
			other.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
		}
=======
        other.SendMessage("KillPlayer", SendMessageOptions.DontRequireReceiver);
>>>>>>> b8d0e7f23a5899fd6027ce15da565461fc8c01cf
	}

	void emitSalt(){
		
		particleLauncher.Emit (1);
	}
}
