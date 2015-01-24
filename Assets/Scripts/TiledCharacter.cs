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
    [SerializeField]
    private float rayScale;

    void Update() {
        transform.position = mapData.transform.position + BaseOffset + 
                             new Vector3(TileX * 2f * BaseOffset.x, TileY * 2f * BaseOffset.y, 0f);

        if (Input.GetKeyDown(KeyCode.UpArrow) && !CollideWithBlocker(-Vector2.up)) {
            --TileY;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !CollideWithBlocker(Vector2.up)) {
            ++TileY;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !CollideWithBlocker(Vector2.right)) {
            --TileX;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !CollideWithBlocker(-Vector2.right)) {
            ++TileX;
        }
        Debug.DrawLine(transform.position, transform.position + (new Vector3(0f, -1f, 0f) * BaseOffset.y * rayScale), Color.red);
    }

    private bool CollideWithBlocker(Vector2 direction) {
        var hits = Physics2D.RaycastAll(transform.position, direction, BaseOffset.y * rayScale);
        for (int i = 0; i < hits.Length; ++i) {
            if (hits[i].collider.gameObject.layer == 9) {
                return true;
            }
        }
        return false;
    }
}