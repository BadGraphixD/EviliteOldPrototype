using UnityEngine;

public class PauseManager : MonoBehaviour {

    private static PauseManager instance;
    public static PauseManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<PauseManager>();
            }
            return instance;
        }
    }

    public bool Paused {
        get; private set;
    }

    private void Awake() {
        Unpause();
    }

    public void Pause() {
        Paused = true;
    }

    public void Unpause() {
        Paused = false;
    }

}
