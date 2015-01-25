using UnityEngine;
using System.Collections;

public class LoadNextLevel : MonoBehaviour {
    public float delay;
    public string nextScene;

    public void OnTriggerEnter2D(Collider2D coll) {
        if (coll.GetComponent<PlayerController>() != null) StartCoroutine(WaitAndLoad());
    }

    public IEnumerator WaitAndLoad() {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(nextScene);
    }
}
