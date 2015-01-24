using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TiledCharacter))]
public class Crate : MonoBehaviour
{
    private TiledCharacter tiled;
    private bool locked;

    void Awake()
    {
        tiled = GetComponent<TiledCharacter>();
    }

    void Start()
    {
        locked = false;
    }

    public bool CanMove(PlayerDirection dir)
    {
        if (locked) { return false; }
        Vector2 vdir = TiledCharacter.Vector2FromPlayerDirection(dir);
        var hits = Physics2D.RaycastAll(tiled.GetRealPosition(), vdir, 1.0f);
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i].collider == collider2D) { continue; }
            if (hits[i].collider.CompareTag("hole")) { continue; }
            if (!hits[i].collider.isTrigger) { return false; }
        }
        return true;
    }

    public void Push(PlayerDirection dir, float duration)
    {
        Vector2 vdir = TiledCharacter.Vector2FromPlayerDirection(dir);
        tiled.MoveToTile(tiled.TileX + (int)vdir.x, tiled.TileY + (int)vdir.y, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("hole"))
        {
            Destroy(other.gameObject);
            locked = true;
            Color col = renderer.material.color;
            col.a = 0.5f;
            renderer.material.color = col;
            Destroy(collider2D);
        }
    }
}
