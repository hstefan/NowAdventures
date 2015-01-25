using UnityEngine;
using System.Collections;

public class MovieAutoplay : MonoBehaviour {
    public MovieTexture movie;

    // Use this for initialization
    void Start () {
        renderer.material.mainTexture = movie;
        movie.Play();
    }
    
    // Update is called once per frame
    void Update () {
    }
}
