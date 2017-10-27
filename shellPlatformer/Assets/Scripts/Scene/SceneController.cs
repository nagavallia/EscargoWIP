using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour, GameManager {
    public int curSceneId;
    [SerializeField] private GameObject complete;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private int maxLevel = 11;

    private float defaultTimeScale;
    private bool levelFinished;
    private bool levelLoaded;
    private bool isPaused = false;

    private int MAIN_SCENE_ID = 1;

    public void Startup() {
        defaultTimeScale = Time.timeScale;

        curSceneId = 0;
        levelLoaded = false;
        pauseMenu.SetActive(false);
    }

    // Use this for initialization
    public void Load (Scene scene, LoadSceneMode mode) {
        if (!levelLoaded) {
            Messenger.AddListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
            Messenger.AddListener(GameEvent.RELOAD_LEVEL, ReloadScene);
            Messenger.AddListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
            Messenger<int>.AddListener(GameEvent.LOAD_LEVEL, LoadLevel);

            pauseMenu.SetActive(false);

            levelLoaded = true;
        }

        Debug.Log("loaded scene " + curSceneId);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Movement Hitbox"));

        Camera camera = Camera.main;
        camera.orthographicSize = 4.5f;
        Screen.SetResolution(1024, 576, false);

        levelFinished = false;
    }

    public void Unload(Scene scene) {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.RemoveListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.RemoveListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
        Messenger<int>.RemoveListener(GameEvent.LOAD_LEVEL, LoadLevel);

        levelLoaded = false;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Restart")) { ReloadScene(); }
        if (Input.GetButtonDown("Menu")) { LoadLevel(MAIN_SCENE_ID); }

        if (Input.GetButtonDown("Pause") && curSceneId > 1)
        {
            if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                isPaused = true;
                pauseMenu.SetActive(true);
            }

        }
    }
    

    private void ReloadScene() {
        Debug.Log("reloading scene " + curSceneId);
        LoadLevel(curSceneId);
    }

    public void FinishLevel() {
        if (!levelFinished) {
            Debug.Log("finish level called in scene " + SceneManager.GetActiveScene().name);
            levelFinished = true;
            Managers.logging.RecordLevelEnd();
            if (curSceneId <= maxLevel) {
                Managers.UnloadAll(SceneManager.GetActiveScene());
                LoadLevel(curSceneId + 1);
            } else { SceneManager.LoadScene("VictoryScreen"); }
        }
    }

    private void ShellDestroyed() {
        Debug.Log("shell was destroyed");
    }

    public void LoadLevel(int id) {
        if ((id == 1 && curSceneId != 0) || curSceneId == 1) {
            Managers.UnloadAll(SceneManager.GetActiveScene());
        }
        curSceneId = id;
        Debug.Log(curSceneId);
        SceneManager.LoadScene(curSceneId);
        Time.timeScale = defaultTimeScale;
    }

    public int GetNumLevels() {
        return maxLevel;
    }
}
