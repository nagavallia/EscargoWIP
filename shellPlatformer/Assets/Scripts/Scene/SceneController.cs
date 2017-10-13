using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, GameManager {
    public int curSceneId;
    [SerializeField] private GameObject complete;
    [SerializeField] private int maxLevel = 11;

    private float defaultTimeScale;
    private bool levelFinished;

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
        Messenger<int>.AddListener(GameEvent.LOAD_LEVEL, LoadLevel);

        Camera camera = Camera.main;
        camera.orthographicSize = 4.6875f;
        Screen.SetResolution(800, 600, false);

        levelFinished = false;
    }

    private void Unload() {
        Debug.Log("scene controller unloaded scene");
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.RemoveListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.RemoveListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
        Messenger<int>.RemoveListener(GameEvent.LOAD_LEVEL, LoadLevel);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.R)) { ReloadScene(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { LoadLevel(0); }
    }

    private void ReloadScene() {
        Unload();
        LoadLevel(curSceneId);
    }

    public void FinishLevel() {
        if (!levelFinished) {
            Debug.Log("finish level called in scene " + SceneManager.GetActiveScene().name);
            levelFinished = true;
            Managers.logging.RecordLevelEnd();
            Unload();
            curSceneId++;
            if (curSceneId <= maxLevel) { LoadLevel(curSceneId); } else { complete.SetActive(true); }
        }
    }

    private void ShellDestroyed() {
        Debug.Log("shell was destroyed");
    }

    public void LoadLevel(int id) {
        string name = "level_" + id;
        if (id == 0) name = "main";
        curSceneId = id;
        SceneManager.LoadScene(name);
        Time.timeScale = defaultTimeScale;
    }
}
