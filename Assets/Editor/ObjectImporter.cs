using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Tiled2Unity;
using System;

[CustomTiledImporter]
public class ObjectImporter : ICustomTiledImporter {
    private Transform mapRoot;
    private Dictionary<string, GameObject> objectIds = new Dictionary<string, GameObject>();
    private Dictionary<string, List<Predicate<GameObject>>> objectIdBackpatches = new Dictionary<string, List<Predicate<GameObject>>>();

    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        var map = gameObject.GetComponentInParent<TiledMap>();
        GameObject inst = null;

        if (props.ContainsKey("AddComp"))
        {
            string prefab_name = props["AddComp"];
            var pos = gameObject.transform.position;

            string prefab = string.Format("Assets/Prefabs/{0}.prefab", prefab_name);
            var go = AssetDatabase.LoadAssetAtPath(prefab, typeof(GameObject));
            if (go != null) {
                inst = (GameObject)GameObject.Instantiate(go);
                inst.name = go.name;
                inst.transform.parent = gameObject.transform;
                inst.transform.localPosition = Vector3.zero;
                inst.transform.localScale = new Vector3(map.TileWidth, map.TileHeight, 1f);

                var tiledComp = inst.GetComponent<TiledCharacter>();
                if (tiledComp != null) {
                    tiledComp.TileX = Mathf.RoundToInt(pos.x / map.TileWidth);
                    tiledComp.TileY = Mathf.RoundToInt(pos.y / map.TileHeight);
                }

                if (prefab_name == "Arrow") {
                    var arrow = inst.GetComponent<Arrow>();
                    if (props.ContainsKey("MoveDuration")) {
                        arrow.MoveDuration = float.Parse(props["MoveDuration"]);
                    }
                    if (props.ContainsKey("Respawning")) {
                        arrow.Respawning = props["Respawning"] == "true";
                    }
                    if (props.ContainsKey("Facing")) {
                        var facing = props["Facing"];
                        switch (facing) {
                        case "Up":
                            inst.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                            break;
                        case "Down":
                            inst.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                            break;
                        case "Left":
                            inst.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                            break;
                        case "Right":
                            inst.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            break;
                        }
                    }
                }
                else if (prefab_name == "Touchplate")
                {
                    var touchplate = inst.GetComponent<Touchplate>();
                    if (props.ContainsKey("Target"))
                    {
                        WithIdReference(props["Target"], target => touchplate.Target = target);
                    }
                }
            }
        }

        if (props.ContainsKey("ObjId"))
        {
            string obj_id = props["ObjId"];
            if (objectIds.ContainsKey(obj_id)) { Debug.Log(obj_id); }
            objectIds.Add(obj_id, inst);
            List<Predicate<GameObject>> patch_list;
            if (objectIdBackpatches.TryGetValue(obj_id, out patch_list))
            {
                foreach (Predicate<GameObject> pred in patch_list)
                {
                    pred(inst);
                }
                objectIdBackpatches.Remove(obj_id);
            }
        }
    }

    public void CustomizePrefab(GameObject prefab) {
    }

    private void WithIdReference(string objId, Predicate<GameObject> pred)
    {
        GameObject go;
        if (objectIds.TryGetValue(objId, out go))
        {
            pred(go);
        }
        else
        {
            List<Predicate<GameObject>> pred_list;
            if (!objectIdBackpatches.TryGetValue(objId, out pred_list))
            {
                pred_list = new List<Predicate<GameObject>>();
                objectIdBackpatches.Add(objId, pred_list);
            }
            pred_list.Add(pred);
        }
    }
}
