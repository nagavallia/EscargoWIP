using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour {
    public static LoggingManager logging { get; private set; }
    public static SceneController scene { get; private set; }
    public static AudioController audio { get; private set; }

    private static List<GameManager> managerList;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        logging = GetComponent<LoggingManager>();
        scene = GetComponent<SceneController>();
        audio = GetComponent<AudioController>();

        managerList = new List<GameManager>();
        managerList.Add(logging);
        managerList.Add(scene);
        managerList.Add(audio);

        StartCoroutine(StartManagers());

        
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += LoadAll;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= LoadAll;
    }

    private IEnumerator StartManagers() {
        foreach (GameManager manager in managerList) manager.Startup();

        yield return null;

        scene.FinishLevel();
    }

    public static void LoadAll(Scene scene, LoadSceneMode mode) {
        foreach(GameManager manager in managerList) {
            manager.Load(scene, mode);
        }
    }

    public static void UnloadAll(Scene scene) {
        foreach(GameManager manager in managerList) {
            manager.Unload(scene);
        }
    }
    
}
