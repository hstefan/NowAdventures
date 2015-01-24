using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionTrigger : MonoBehaviour {
    [SerializeField]
    private PlayerItem item;
    private Toggle toggle;
    private PlayerController player;

    void Start() {
        toggle = GetComponent<Toggle>();
        player = FindObjectOfType<PlayerController>();
    }

    public void OnToggleChanged(bool value) {
        if (value) {
            if (player.equippedItem != item) {
                player.equippedItem = item;
            }
        }
        else {
            if (player.equippedItem == item) {
                player.equippedItem = PlayerItem.None;
            }
        }
    }

    void Update() {
        toggle.isOn = player.equippedItem == item;
    }
}
