using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Tiled2Unity;

[CustomTiledImporter]
public class ObjectImporter : ICustomTiledImporter {
    private Transform mapRoot;

    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props) {
        Debug.Log("Custom properties!");
        var map = gameObject.GetComponentInParent<TiledMap>();
        if (props.ContainsKey("AddComp")) {
            var pos = gameObject.transform.position;
            //gameObject.AddComponent(props["AddComp"]);

            string prefab = string.Format("Assets/Prefabs/{0}.prefab", props["AddComp"]);
            var go = AssetDatabase.LoadAssetAtPath(prefab, typeof(GameObject));
            if (go != null) {
                var inst = (GameObject)GameObject.Instantiate(go);
                inst.name = go.name;
                inst.transform.parent = gameObject.transform;
                inst.transform.localPosition = Vector3.zero;
                inst.transform.localScale = new Vector3(map.TileWidth, map.TileHeight, 1f);

                var tiledComp = inst.GetComponent<TiledCharacter>();
                if (tiledComp != null) {
                    tiledComp.TileX = Mathf.RoundToInt(pos.x / map.TileWidth);
                    tiledComp.TileY = Mathf.RoundToInt(pos.y / map.TileHeight);
                }
            }
        }
    }

    public void CustomizePrefab(GameObject prefab) {
    }
}
