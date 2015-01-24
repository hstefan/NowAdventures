using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TiledCharacter))]
public class Slime : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
