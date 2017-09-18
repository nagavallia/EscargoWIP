using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float moveDist;
    [SerializeField] private float speed;
    private float curMove;
    private float direction;
    private Vector3 startPos;

	// Use this for initialization
	void Start () 
	{
        curMove = 0f;
        direction = -1f;
        startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
        float movement = direction * speed * Time.deltaTime;
        this.gameObject.transform.Translate(0f, movement, 0f);
        curMove += movement;
        if (curMove < -1f*moveDist) 
		{ 
			this.gameObject.transform.localPosition = startPos + new Vector3(0f, -1f*moveDist, 0f); 
			direction = 1f; 
			curMove = -1f * moveDist; 
		}
        else if (curMove > moveDist) 
		{ 
			this.gameObject.transform.localPosition = startPos + new Vector3(0f, moveDist, 0f); 
			direction = -1f; 
			curMove = moveDist; 
		}

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("EnemyCollide", SendMessageOptions.DontRequireReceiver);
    }

    private void ShellCollide(GameObject shell) {
        Debug.Log("enemy hit by shell");
        shell.SendMessage("ShellDestroy");
        Destroy(this.gameObject);
    }
}
