using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltController : MonoBehaviour {

	private bool canKill = true;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag != "TempShell" && canKill)
			collision.gameObject.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);

		if (collision.collider.tag == "Shell") {
			canKill = false;
		}

		if (collision.gameObject.tag == "Ground" || collision.gameObject.layer == LayerMask.NameToLayer("Background") || collision.collider.tag == "Player" && !canKill) {
			Destroy (this.gameObject);
		}
	}
}
