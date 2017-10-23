using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour, GameManager {
    [SerializeField] private AudioSource bgm;
    private bool levelLoaded;

    public void Startup() {
        levelLoaded = false;
    }

    public void Load(Scene scnee, LoadSceneMode mode) {
        if (!levelLoaded) {
            bgm.loop = true;
            bgm.Play();

            levelLoaded = true;
        }
    }

    public void Unload(Scene scene) {
        bgm.Stop();

        levelLoaded = false;
    }
	
}
