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
        other.SendMessage("KillPlayer", SendMessageOptions.DontRequireReceiver);
	}

	void emitSalt(){
		
		particleLauncher.Emit (1);
	}
}
