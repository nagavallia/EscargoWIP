using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    Scene curScene;
    [SerializeField] private string nextScene;

    private void Awake() {
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.AddListener(GameEvent.RELOAD_LEVEL, ReloadScene);
    }

    // Use this for initialization
    void Start () {
        curScene = SceneManager.GetActiveScene();
	}

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.RemoveListener(GameEvent.RELOAD_LEVEL, ReloadScene);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.R)) { ReloadScene(); }
	}

    private void ReloadScene() {
        SceneManager.LoadScene(curScene.name);
    }

    private void FinishLevel() {
        if (nextScene != null) { SceneManager.LoadScene(nextScene); }
    }
}
