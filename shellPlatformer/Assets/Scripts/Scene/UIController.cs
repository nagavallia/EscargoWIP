using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private float defaultTimeScale;
    [SerializeField] private Dropdown levelSelect;

    private void Start() {
        defaultTimeScale = Time.timeScale;
        List<string> levels = new List<string>();
        for (int i = 1; i <= Managers.scene.GetNumLevels(); i++) {
            levels.Add("Level " + i);
        }

        levelSelect.AddOptions(levels);
    }


    public void LoadLevel(int levelId) {
        Messenger<int>.Broadcast(GameEvent.LOAD_LEVEL, levelId + 1);
    }

    public void SetBGMScale(float scale) {
        Messenger<float>.Broadcast(GameEvent.BGM_SCALE, scale);
    }
}
