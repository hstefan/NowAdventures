using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionTrigger : MonoBehaviour {
    [SerializeField]
    private PlayerItem item;
    private Toggle toggle;
    private PlayerController player;
    private Text text;

    void Start() {
        toggle = GetComponent<Toggle>();
        player = FindObjectOfType<PlayerController>();
        text = GetComponentInChildren<Text>();
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
        text.text = string.Format("{0}", System.Array.Find<ItemUses>(player.usesPerItem, i => i.Item == item).Uses);
    }
}
