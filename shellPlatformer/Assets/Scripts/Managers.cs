using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour {
    public static LoggingManager logging { get; private set; }
    public static SceneController scene { get; private set; }

    private List<GameManager> _startSequence;

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        logging = GetComponent<LoggingManager>();
        scene = GetComponent<SceneController>();

        _startSequence = new List<GameManager>();
        _startSequence.Add(logging);
        _startSequence.Add(scene);

        StartCoroutine(StartManagers());

        
    }

    private IEnumerator StartManagers() {
        foreach (GameManager manager in _startSequence) manager.Startup();

        yield return null;

        scene.FinishLevel();
    }
}
