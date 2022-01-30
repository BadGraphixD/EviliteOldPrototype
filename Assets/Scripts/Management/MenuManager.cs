using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MenuManager : MonoBehaviour {
    
    private static MenuManager instance;
    public static MenuManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<MenuManager>();
            }
            return instance;
        }
    }

    [SerializeField] private CanvasGroup allMenus;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup newGame;
    [SerializeField] private CanvasGroup loadGame;
    [SerializeField] private CanvasGroup pause;
    [SerializeField] private CanvasGroup options;

    [SerializeField] private TMP_InputField slotNameInput;
    [SerializeField] private Button createNewSlotButton;
    String slotName = null;

    [SerializeField] private Transform loadedSlotsView;

    [HideInInspector]
    public SlotData[] loadedSlots;

    private void Update() {
        slotName = slotNameInput.text;
        createNewSlotButton.interactable = slotName != null || slotName != "" || slotName.Length > 0;
    }

    public void SetAllMenusActive(bool active) {
        SetActive(allMenus, active);
    }

    public void MainMenu() {
        setAllInactive();
        SetActive(mainMenu, true);
    }

    public void NewGame() {
        setAllInactive();
        SetActive(newGame, true);
    }

    public void LoadGame() {
        setAllInactive();
        SetActive(loadGame, true);
        
        foreach (Transform gameSlotUI in loadedSlotsView) {
            GameObject.Destroy(gameSlotUI.gameObject);
        }

        loadedSlots = SlotFileManager.LoadAllSlotDataFiles();

        int idx = 0;
        foreach (SlotData slotData in loadedSlots) {
            GameObject gameSlotUI = Instantiate(AssetManager.Instance.gameSlotUIPrefab, Vector3.zero, Quaternion.identity, loadedSlotsView);
            gameSlotUI.GetComponent<GameSlotUI>().SetProperties(slotData.name, idx);
            idx++;
        }
    }

    public void Pause() {
        setAllInactive();
        SetActive(pause, true);
    }

    public void Options() {
        setAllInactive();
        SetActive(options, true);
    }

    public void Exit() {
        Application.Quit();
    }

    public void CreateNewSlot() {
        GameManager.Instance.PlaySlot(SlotFileManager.LoadNewLevelSlotData(slotName, 0.0f));
        GameManager.Instance.SaveSlot();
    }

    private void setAllInactive() {
        SetActive(mainMenu, false);
        SetActive(newGame, false);
        SetActive(loadGame, false);
        SetActive(pause, false);
        SetActive(options, false);
    }

    private void SetActive(CanvasGroup group, bool active) {
        group.alpha = active ? 1f : 0f;
        group.interactable = active;
        group.blocksRaycasts = active;
    }

}
