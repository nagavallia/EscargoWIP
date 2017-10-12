using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActivationMode : MonoBehaviour {
    public bool ENABLED;
    [SerializeField] private GameObject[] activators;
    private Dictionary<int, bool> activatorIds;
    private bool hasBeenActivated;

    private void Start() {
        foreach (GameObject activator in activators) {
            Button buttonComp = activator.GetComponent<Button>();
            Switch switchComp = activator.GetComponent<Switch>();
            if (buttonComp != null) activatorIds.Add(buttonComp.id, false);
            else if (switchComp != null) activatorIds.Add(switchComp.id, false);
        }
    }

    private void Interact(int id) {
        if (ENABLED) {
            if (!hasBeenActivated) {
                activatorIds[id] = !activatorIds[id];
                bool activate = true;
                foreach (KeyValuePair<int, bool> pair in activatorIds) {
                    activate = activate && pair.Value;
                }
                if (activate) gameObject.SendMessage("Interact");
                hasBeenActivated = true;
            } else {
                activatorIds[id] = !activatorIds[id];
                gameObject.SendMessage("Interact");
                hasBeenActivated = false;
            }
        } else {
            gameObject.SendMessage("Interact");
        }
    }
}
