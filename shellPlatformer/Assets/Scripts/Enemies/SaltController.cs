using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltController : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D collision)
	{
		collision.gameObject.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);

		if (collision.gameObject.tag == "Ground" || collision.gameObject.layer == LayerMask.NameToLayer("Background")) {
			Destroy (this.gameObject);
		}
	}
}
