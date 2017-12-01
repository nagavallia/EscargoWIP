using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, GameManager {
    public int curSceneId;
    [SerializeField] private GameObject complete;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private int maxLevel = 11;
	public int challengeLevels = 3;
	public int levelsCompleted { get; private set; }
	public int challengeCompleted { get; private set; }

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

        if (Managers.logging.isDebugging) // load levels completed from local, else set to 0
            levelsCompleted = maxLevel + challengeLevels;
        else
            levelsCompleted = PlayerPrefs.GetInt(GameEvent.LEVELS_FINISHED, 0);
    }

    private void OnDestroy() {
        if (Managers.logging.isDebugging) PlayerPrefs.SetInt(GameEvent.LEVELS_FINISHED, 0);
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
        if (Input.GetButtonDown("Menu") && Managers.logging.isDebugging) { LoadLevel(MAIN_SCENE_ID); }

        if (Input.GetButtonDown("Pause"))
        {
			if (CanPause())
            	Pause();
			else if (curSceneId == maxLevel + challengeLevels + 2 || curSceneId == maxLevel + challengeLevels + 3)
				LoadLevel (MAIN_SCENE_ID);
        }
    }
    

    private void ReloadScene() {
		// log that reset has been triggered and record the location of the snail 
		Managers.logging.RecordEvent (7, "" + GameObject.FindGameObjectWithTag ("Player").transform.position);

        Debug.Log("reloading scene " + curSceneId);
        LoadLevel(curSceneId);
    }

    public void FinishLevel() {
        if (!levelFinished) {
            Debug.Log("finish level called in scene " + SceneManager.GetActiveScene().name);
            levelFinished = true;
            Managers.logging.RecordLevelEnd();
            Managers.UnloadAll(SceneManager.GetActiveScene());

			levelsCompleted = Mathf.Max(levelsCompleted, Mathf.Min(curSceneId - 1, maxLevel + challengeLevels));

            PlayerPrefs.SetInt(GameEvent.LEVELS_FINISHED, levelsCompleted);
            PlayerPrefs.Save();

			if (curSceneId == maxLevel + 1)
				LoadLevel (maxLevel + challengeLevels + 2);
			else if (curSceneId == maxLevel + challengeLevels + 1)
				LoadLevel (maxLevel + challengeLevels + 3);
			else 
				LoadLevel(curSceneId + 1);
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

	// loads Level levelno
	public void LoadLevelByNumber(int levelno) {
		LoadLevel(levelno + 1);
	}

	public void LoadChallenge(int levelno) {
		LoadLevel (levelno + 1 + maxLevel);
	}

    public int GetNumLevels() {
		return maxLevel;
	}

    public void Pause() {
        if (isPaused) {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenu.SetActive(false);
        } else {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenu.SetActive(true);
        }
    }

    public bool CanPause() {
		return curSceneId > 1 && curSceneId < maxLevel + challengeLevels + 2;
    }
    
}
