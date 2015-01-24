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
                             new Vector3(TileX * 2f * BaseOffset.x, TileY * 2f * BaseOffset.y, 0f);

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            --TileY;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            ++TileY;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            --TileX;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            ++TileX;
        }
    }
}
