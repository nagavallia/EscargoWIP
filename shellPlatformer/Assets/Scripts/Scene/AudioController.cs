using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour, GameManager {
    [SerializeField] private AudioClip bgm;
    private AudioSource bgmSource; // AudioSource object for bgm, created at startup
    private bool levelLoaded;

    public void Startup() {
        levelLoaded = false;

        LoadBGM();
    }

    public void Load(Scene scnee, LoadSceneMode mode) {
        if (!levelLoaded) {
            bgmSource.Play();

            levelLoaded = true;
        }
    }

    public void Unload(Scene scene) {
        bgmSource.Stop();

        levelLoaded = false;
    }

    private void LoadBGM() {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgm; // for final: bgmSource.clip = Resources.Load(bgm) as AudioClip;
        bgmSource.loop = true;
    }
	
}
