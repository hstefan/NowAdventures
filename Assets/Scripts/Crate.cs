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

    public bool CanMove(PlayerDirection dir)
    {
        if (locked) { return false; }
        Collider2D coll = tiled.CollideWithBlocker(TiledCharacter.Vector2FromPlayerDirection(dir), collider2D);
        return coll == null;
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
        }
    }
}
