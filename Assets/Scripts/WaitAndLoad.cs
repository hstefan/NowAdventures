using UnityEngine;
using System.Collections;

public class WaitAndLoad : MonoBehaviour {
    public string scene;
    public float delay;

    // Use this for initialization
    void Start () {
        StartCoroutine(WaitLoad());
    }

    public IEnumerator WaitLoad() {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(scene);
    }
    
}
