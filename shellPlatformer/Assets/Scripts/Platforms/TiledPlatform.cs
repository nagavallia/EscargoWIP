using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        BoxCollider2D hitbox = GetComponent<BoxCollider2D>();

        hitbox.size = sprite.size;
        
	}
}
