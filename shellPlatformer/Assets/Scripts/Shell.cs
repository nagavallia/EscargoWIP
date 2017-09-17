using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("ShellCollide", SendMessageOptions.DontRequireReceiver);
    }
}
