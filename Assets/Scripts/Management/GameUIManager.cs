using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class GameUIManager : MonoBehaviour {
    
    private static GameUIManager instance;
    public static GameUIManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameUIManager>();
            }
            return instance;
        }
    }
    
    [SerializeField] private CanvasGroup gameUI;

    public void SetGameUIActive(bool active) {
        gameUI.alpha = active ? 1f : 0f;
        gameUI.interactable = active;
        gameUI.blocksRaycasts = active;
    }

}
