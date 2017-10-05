using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    Scene curScene;
    [SerializeField] private string nextScene;
    [SerializeField] private GameObject complete;
	[SerializeField] private Dropdown throwSelector;

    private float defaultTimeScale;

    private void Awake() {
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.AddListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.AddListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
        defaultTimeScale = Time.timeScale;
    }

    // Use this for initialization
    void Start () {
        curScene = SceneManager.GetActiveScene();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Movement Hitbox"));
	}

    private void OnDestroy() {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.RemoveListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.RemoveListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.R)) { ReloadScene(); }
	}

    private void ReloadScene() {
        SceneManager.LoadScene(curScene.name);
    }

    private void FinishLevel() {
        if (nextScene != "") { SceneManager.LoadScene(nextScene); }
        else { complete.SetActive(true); }
    }

    private void ShellDestroyed() {
        Debug.Log("shell was destroyed");
    }

    public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
        Time.timeScale = defaultTimeScale;
    }
}
