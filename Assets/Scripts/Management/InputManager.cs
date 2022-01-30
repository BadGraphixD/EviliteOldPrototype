using UnityEngine;
using System;

public class InputManager : MonoBehaviour {

    private static InputManager instance;
    public static InputManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<InputManager>();
            }
            return instance;
        }
    }

    public enum InputAction {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
        JUMP = 4,
        INVENTORY = 5,
        MAP = 6,
        DASH = 7,
        ATTACK = 8,
        RECALL = 9,
        ALTERNATE_SWORD = 10
    }

    private int inputActionAmount {
        get { return Enum.GetValues(typeof(InputAction)).Length; }
    }

    private KeyCode[] keyBinds;

    private Utils.Callback[] keyDownCallbacks;
    private Utils.Callback[] keyUpCallbacks;

    private void Awake() {
        keyDownCallbacks = new Utils.Callback[inputActionAmount];
        keyUpCallbacks = new Utils.Callback[inputActionAmount];

        SetDefaultKeyBinds(); // TODO: or load existing kexbinds
    }

    private void LateUpdate() {
        for (int i = 0; i < inputActionAmount; i++) {

            if (Input.GetKeyDown(keyBinds[i])) {
                keyDownCallbacks[i]?.Invoke();
            }
            if (Input.GetKeyUp(keyBinds[i])) {
                keyUpCallbacks[i]?.Invoke();
            }
        }
    }

    public void SetDefaultKeyBinds() {
        keyBinds = new KeyCode[inputActionAmount];

        SetKeyBind(InputAction.UP, KeyCode.W);
        SetKeyBind(InputAction.DOWN, KeyCode.S);
        SetKeyBind(InputAction.LEFT, KeyCode.A);
        SetKeyBind(InputAction.RIGHT, KeyCode.D);
        SetKeyBind(InputAction.JUMP, KeyCode.Space);
        SetKeyBind(InputAction.INVENTORY, KeyCode.E);
        SetKeyBind(InputAction.MAP, KeyCode.M);
        SetKeyBind(InputAction.DASH, KeyCode.Tab);
        SetKeyBind(InputAction.ATTACK, KeyCode.Mouse0);
        SetKeyBind(InputAction.RECALL, KeyCode.Mouse1);
        SetKeyBind(InputAction.ALTERNATE_SWORD, KeyCode.LeftShift);
    }

    public void SetKeyBind(InputAction action, KeyCode key) {
        keyBinds[(int)action] = key;
    }

    public void SubscribeKeyDownCallback(InputAction action, Utils.Callback callback) {
        keyDownCallbacks[(int)action] += callback;
    }

    public void SubscribeKeyUpCallback(InputAction action, Utils.Callback callback) {
        keyUpCallbacks[(int)action] += callback;
    }
    
    public void UnsubscribeKeyDownCallback(InputAction action, Utils.Callback callback) {
        keyDownCallbacks[(int)action] -= callback;
    }

    public void UnsubscribeKeyUpCallback(InputAction action, Utils.Callback callback) {
        keyUpCallbacks[(int)action] -= callback;
    }

    public bool GetKey(InputAction action) {
        return Input.GetKey(keyBinds[(int)action]);
    }

}
