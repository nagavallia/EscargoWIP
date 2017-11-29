using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {
	public int attachedLevel = 0;
	public Text levelText;
	public Sprite locked;
	public Sprite unlocked;
	public bool isLocked;
	public bool isChallenge;

	public void LoadAttachedLevel() {
		if (!isLocked) {
			if (!isChallenge)
				Managers.scene.LoadLevelByNumber (attachedLevel);
			else
				Managers.scene.LoadChallenge (attachedLevel);
		}
	}

	public void Lock() {
		Image image = GetComponent<Image> ();
		image.sprite = locked;
		levelText.text = "";
		isLocked = true;
	}

	public void Unlock() {
		Image image = GetComponent<Image> ();
		image.sprite = unlocked;
		levelText.text = "" + attachedLevel;
		isLocked = false;
	}
}
