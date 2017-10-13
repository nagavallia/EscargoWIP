using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    [SerializeField] private GameObject uiCanvas;
    private float defaultTimeScale;

    private KeyCode MENU_BUTTON = KeyCode.Escape;

    private void Start() {
        defaultTimeScale = Time.timeScale;
        uiCanvas.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(MENU_BUTTON)) {
            //if (uiCanvas.activeInHierarchy) {
            //    Time.timeScale = defaultTimeScale;
            //    uiCanvas.SetActive(false);
            //} else {
            //    Time.timeScale = 0f;
            //    uiCanvas.SetActive(true);
            //}
            LoadLevel(0);
        }
    }

    public void LoadLevel(int levelId) {
        Messenger<int>.Broadcast(GameEvent.LOAD_LEVEL, levelId);
    }
}
