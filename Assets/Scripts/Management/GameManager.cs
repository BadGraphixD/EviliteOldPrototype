using UnityEngine;
using UnityEngine.Tilemaps;
using System;

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

    public enum Level {
        OVERWORLD = 0,
        CATACOMBS = 1,
        TEMPLE = 2
    }

    public Tilemap tilemap;
    private SlotData currentSlot;

    public void PlaySlot(SlotData slotData) {
        currentSlot = slotData;
        UnityEngine.Random.seed = 42; 
        SlotDataProcessor.DeserializeAndCreateEntities(slotData);
        SlotDataProcessor.DeserializeAndCreateMap(slotData);
    }

    public void SaveSlot() {
        currentSlot.entityData = SlotDataProcessor.SerializeEntities();
        currentSlot.mapData = SlotDataProcessor.SerializeMap();
        SlotFileManager.SaveSlotData(currentSlot);
    }

}
