﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private float defaultTimeScale;
	[SerializeField] private Canvas MainMenuCanvas;
	[SerializeField] private Canvas LevelSelectCanvas;
	[SerializeField] private float horizButtonStep, vertButtonStep;
	[SerializeField] private int buttonsPerRow, buttonsPerCol;

    private void Start() {
        defaultTimeScale = Time.timeScale;
        
		CreateLevelSelect ();

		MainMenuCanvas.gameObject.SetActive (true);
		LevelSelectCanvas.gameObject.SetActive (false);
    }

	public void ToggleLevelSelect() {
		MainMenuCanvas.gameObject.SetActive (!MainMenuCanvas.gameObject.activeInHierarchy);
		LevelSelectCanvas.gameObject.SetActive (!LevelSelectCanvas.gameObject.activeInHierarchy);
	}

    public void LoadLevel(int levelId) {
        Messenger<int>.Broadcast(GameEvent.LOAD_LEVEL, levelId + 1);
    }

    public void SetBGMScale(float scale) {
        Messenger<float>.Broadcast(GameEvent.BGM_SCALE, scale);
    }

	private void CreateLevelSelect() {
		int i = 0, j = 0;
		for (int k = 1; k <= Managers.scene.GetNumLevels (); k++) {
			GameObject buttonObject = Instantiate (Resources.Load ("LevelButton") as GameObject, LevelSelectCanvas.transform);
			LevelSelectButton button = buttonObject.GetComponent<LevelSelectButton> ();

			button.attachedLevel = k;
			Vector2 localPos = button.GetComponent<RectTransform> ().anchoredPosition;
			button.GetComponent<RectTransform> ().anchoredPosition = localPos + new Vector2 (horizButtonStep * i, vertButtonStep * j);

			if (k <= Managers.scene.levelsCompleted)
				button.Unlock ();
			else
				button.Lock ();

			i++;
			if (i == buttonsPerRow) {
				j++;
				i = 0;
			}
		}
	}
		
}
