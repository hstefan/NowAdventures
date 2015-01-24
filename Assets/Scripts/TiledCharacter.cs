using UnityEngine;
using System.Collections;
using Tiled2Unity;

[ExecuteInEditMode]
public class TiledCharacter : MonoBehaviour {
    [SerializeField]
    private TiledMap mapData;
    [SerializeField]
    private int TileX;
    [SerializeField]
    private int TileY;
    [SerializeField]
    private Vector3 BaseOffset;

    void Update() {
        transform.position = mapData.transform.position + BaseOffset + 
                             new Vector3(TileX, TileY, 0f);

        if (Input.GetKeyDown(KeyCode.UpArrow) && !CollideWithBlocker(Vector2.up)) {
            ++TileY;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !CollideWithBlocker(-Vector2.up)) {
            --TileY;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !CollideWithBlocker(-Vector2.right)) {
            --TileX;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !CollideWithBlocker(Vector2.right)) {
            ++TileX;
        }
    }

    private bool CollideWithBlocker(Vector2 direction) {
        var hits = Physics2D.RaycastAll(transform.position, direction, 1.0f);
        for (int i = 0; i < hits.Length; ++i) {
            if (hits[i].collider.gameObject.layer == 9) {
                return true;
            }
        }
        return false;
    }
}