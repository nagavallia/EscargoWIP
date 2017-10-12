using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, GameManager {
    public int curSceneId;
    [SerializeField] private GameObject complete;
    [SerializeField] private int maxLevel = 6;

    private float defaultTimeScale;

    public void Startup() {
        defaultTimeScale = Time.timeScale;

        curSceneId = -1;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += Loaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= Loaded;
    }

    // Use this for initialization
    private void Loaded (Scene scene, LoadSceneMode mode) {
        Debug.Log("scene controller loaded scene: " + scene.name);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Movement Hitbox"));
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.AddListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.AddListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
    }

    private void Unload() {
        Debug.Log("scene manager unloaded scene");
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.RemoveListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.RemoveListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.R)) { ReloadScene(); }
	}

    private void ReloadScene() {
        Unload();
        LoadLevel(curSceneId);
    }

    public void FinishLevel() {
        Unload();
        curSceneId++;
        if (curSceneId <= maxLevel) { LoadLevel(curSceneId); }
        else { complete.SetActive(true); }
    }

    private void ShellDestroyed() {
        Debug.Log("shell was destroyed");
    }

    public void LoadLevel(int id) {
        string name = "level_" + id;
        if (id == 0) name = "easy_level";
        curSceneId = id;
        SceneManager.LoadScene(name);
        Time.timeScale = defaultTimeScale;
    }
}
