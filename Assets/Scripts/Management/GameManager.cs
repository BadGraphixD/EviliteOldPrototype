using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
    
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public Tilemap tilemap;
    public Transform decorationParent;
    public Transform entityParent;
    private SlotData currentSlot;

    void Awake() {
        GameUIManager.Instance.SetGameUIActive(false);
        MenuManager.Instance.SetAllMenusActive(true);
        MenuManager.Instance.MainMenu();
    }

    public void GoToMainMenu() {
        SaveSlot();
        GameUIManager.Instance.SetGameUIActive(false);
        MenuManager.Instance.SetAllMenusActive(true);
        MenuManager.Instance.MainMenu();

        tilemap.ClearAllTiles();
        foreach (Transform decoration in decorationParent) {
            GameObject.Destroy(decoration.gameObject);
        }
        foreach (Transform entity in entityParent) {
            GameObject.Destroy(entity.gameObject);
        }
    }

    public void PlaySlot(SlotData slotData) {
        currentSlot = slotData;
        UnityEngine.Random.InitState(slotData.seed); 
        SlotDataProcessor.DeserializeAndCreateEntities(slotData);
        SlotDataProcessor.DeserializeAndCreateMap(slotData);
        
        GameUIManager.Instance.SetGameUIActive(true);
        MenuManager.Instance.SetAllMenusActive(false);

        Player player = FindObjectOfType<Player>();

        CameraController.Instance.SetTarget(player.transform);
        PlayerController.Instance.SetPlayer(player);
    }

    public void SaveSlot() {
        currentSlot.entityData = SlotDataProcessor.SerializeEntities();
        currentSlot.mapData = SlotDataProcessor.SerializeMap();
        SlotFileManager.SaveSlotData(currentSlot);
    }

}
