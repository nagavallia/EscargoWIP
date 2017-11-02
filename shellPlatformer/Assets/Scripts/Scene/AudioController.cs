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
        Messenger<float>.AddListener(GameEvent.BGM_SCALE, SetBGMScale);

        LoadBGM();
    }

    private void OnDestroy() {
        Messenger<float>.RemoveListener(GameEvent.BGM_SCALE, SetBGMScale);
    }

    public void Load(Scene scnee, LoadSceneMode mode) {
        if (Managers.scene.curSceneId == 1 && !levelLoaded) {
            bgmSource.Play();
            levelLoaded = true;
        } else if (Managers.scene.curSceneId == 1) {
            bgmSource.time = 0f;
        }
    }

    public void Unload(Scene scene) {
        //bgmSource.Stop();

        //levelLoaded = false;
    }

    private void LoadBGM() {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgm; // for final: bgmSource.clip = Resources.Load(bgm) as AudioClip;
        bgmSource.loop = true;
    }

    public void SetBGMScale(float scale) {
        bgmSource.volume = scale;
    }
	
}
