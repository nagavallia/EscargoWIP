using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlatformOther : MonoBehaviour {
	[SerializeField] private ScalePlatform parent;

	private void OnCollisionEnter2D(Collision2D collision) {
		collision.transform.SetParent (this.transform);
		parent.OtherWeightChange (1);
	}

	private void OnCollisionExit2D(Collision2D collision) {
		collision.transform.SetParent (null);
		parent.OtherWeightChange (-1);
	}
}
