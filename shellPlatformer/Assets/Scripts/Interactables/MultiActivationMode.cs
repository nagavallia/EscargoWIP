using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActivationMode : MonoBehaviour {
    public bool ENABLED;
    [SerializeField] private int[] activators;
    private Dictionary<int, bool> activatorIds;
    private bool hasBeenActivated;

    private void Start() {
        activatorIds = new Dictionary<int, bool>();
        hasBeenActivated = false;

        foreach (int activator in activators) {
            activatorIds.Add(activator, false);
        }
    }

    private void TriggerInteraction(int id) {
        if (id == -1) return;
        if (ENABLED) {
            if (!hasBeenActivated) {
                activatorIds[id] = !activatorIds[id];
                bool activate = true;
                foreach (KeyValuePair<int, bool> pair in activatorIds) {
                    activate = activate && pair.Value;
                }
                if (activate) {
                    gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                    hasBeenActivated = true;
                }
            } else {
                activatorIds[id] = !activatorIds[id];
                gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                hasBeenActivated = false;
            }
        } else {
            gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }
    }
}
