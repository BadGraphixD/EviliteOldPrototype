using UnityEngine;
using System;
using TMPro;

public class GameSlotUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI nameTextField;

    private int slotIdx;

    public void SetProperties(String name, int slotIdx) {
        nameTextField.text = name;
        this.slotIdx = slotIdx;
    }

    public void Play() {
        GameManager.Instance.PlaySlot(MenuManager.Instance.loadedSlots[slotIdx]);
    }
    
}