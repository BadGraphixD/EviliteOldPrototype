using UnityEngine;
using System;
using TMPro;

public class GameSlotUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private TextMeshProUGUI progressTextField;

    private int slotIdx;

    public void SetProperties(String name, float progress, int slotIdx) {
        nameTextField.text = name;
        progressTextField.text = progress + "%";
        this.slotIdx = slotIdx;
    }

    public void Play() {
        GameManager.Instance.PlaySlot(MenuManager.Instance.loadedSlots[slotIdx]);
    }
    
}