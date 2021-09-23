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

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject newGameCanvas;
    [SerializeField] private GameObject loadGameCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject aboutCanvas;

    [SerializeField] private TMP_InputField slotNameInput;
    [SerializeField] private Button createNewSlotButton;
    String slotName = null;

    [SerializeField] private Transform loadedSlotsView;

    [HideInInspector]
    public SlotData[] loadedSlots;

    private void Update() {
        name = slotNameInput.text;
        createNewSlotButton.interactable = name != null || name != "" || name.Length > 0;
    }

    public void NewGame() {
        setAllInactive();
        newGameCanvas.SetActive(true);
    }

    public void LoadGame() {
        setAllInactive();
        loadGameCanvas.SetActive(true);

        loadedSlots = SlotFileManager.LoadAllSlotDataFiles();

        int idx = 0;
        foreach (SlotData slotData in loadedSlots) {
            GameObject gameSlotUI = Instantiate(AssetManager.Instance.gameSlotUIPrefab);
            gameSlotUI.transform.parent = loadedSlotsView;
            gameSlotUI.GetComponent<GameSlotUI>().SetProperties(slotData.name, slotData.progress, idx);
            idx++;
        }
    }

    public void Options() {
        setAllInactive();
        optionsCanvas.SetActive(true);
    }

    public void About() {
        setAllInactive();
        aboutCanvas.SetActive(true);
    }

    public void Exit() {
        Application.Quit();
    }

    public void MainMenu() {
        setAllInactive();
        mainMenuCanvas.SetActive(true);
    }

    public void CreateNewSlot() {
        GameManager.Instance.PlaySlot(SlotFileManager.LoadNewLevelSlotData(
            GameManager.Level.OVERWORLD, name, 0.0f
        ));
        GameManager.Instance.SaveSlot();
    }

    private void setAllInactive() {
        mainMenuCanvas.SetActive(false);
        newGameCanvas.SetActive(false);
        loadGameCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        aboutCanvas.SetActive(false);
    }

}
