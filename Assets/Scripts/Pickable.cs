using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {
    public PlayerItem Item;

    void OnTriggerEnter2D(Collider2D coll) {
        var ps = FindObjectsOfType<PlayerController>();
        foreach (var p in ps) {
            if (p.controllable) {
                p.addItem(Item);
            }
        }
        Destroy(gameObject);
    }
}
