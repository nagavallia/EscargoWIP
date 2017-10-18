﻿using System.Collections;
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

    private int MAIN_SCENE_ID = 1;

    public void Startup() {
        defaultTimeScale = Time.timeScale;

        curSceneId = 0;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += Loaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= Loaded;
    }

    // Use this for initialization
    private void Loaded (Scene scene, LoadSceneMode mode) {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Movement Hitbox"));
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.AddListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.AddListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
        Messenger<int>.AddListener(GameEvent.LOAD_LEVEL, LoadLevel);

        Camera camera = Camera.main;
        camera.orthographicSize = 4.5f;
        Screen.SetResolution(1024, 576, false);

        levelFinished = false;
    }

    private void Unload() {
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, FinishLevel);
        Messenger.RemoveListener(GameEvent.RELOAD_LEVEL, ReloadScene);
        Messenger.RemoveListener(GameEvent.SHELL_DESTROYED, ShellDestroyed);
        Messenger<int>.RemoveListener(GameEvent.LOAD_LEVEL, LoadLevel);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Restart")) { ReloadScene(); }
        if (Input.GetButtonDown("Menu")) { LoadLevel(MAIN_SCENE_ID); }
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
            if (curSceneId <= maxLevel) {
                Unload();
                LoadLevel(curSceneId + 1);
            } else { complete.SetActive(true); }
        }
    }

    private void ShellDestroyed() {
        Debug.Log("shell was destroyed");
    }

    public void LoadLevel(int id) {
        if ((id == 1 && curSceneId != 0) || curSceneId == 1) {
            Unload();
        }
        curSceneId = id;
        SceneManager.LoadScene(curSceneId);
        Time.timeScale = defaultTimeScale;
    }
}
