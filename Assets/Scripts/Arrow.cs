using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TiledCharacter))]
public class Arrow : MonoBehaviour
{
    public float MoveDuration;
    public bool Respawning;

    private TiledCharacter tiled;
    private Vector2 original_position;
    private Coroutine movement_coroutine;

    void Awake()
    {
        tiled = GetComponent<TiledCharacter>();
    }

    void Start()
    {
        original_position.x = tiled.TileX;
        original_position.y = tiled.TileY;
        movement_coroutine = null;
    }

    void Update()
    {
        if (movement_coroutine == null)
        {
            var hits = Physics2D.RaycastAll(tiled.GetRealPosition(), transform.right);
            for (int i = 0; i < hits.Length; ++i)
            {
                if (hits[i].collider == collider2D) { continue; }
                if (!hits[i].collider.isTrigger) { break; }
                if (hits[i].collider.CompareTag("player")) { ActivateEffect(); }
            }
        }
    }

    public void ResetPosition()
    {
        if (!Respawning)
        {
            Destroy(gameObject);
            return;
        }

        if (movement_coroutine != null)
        {
            StopCoroutine(movement_coroutine);
            movement_coroutine = null;
        }
        tiled.TeleportToTile((int)original_position.x, (int)original_position.y);
    }

    public void ActivateEffect()
    {
        if (movement_coroutine == null)
        {
            movement_coroutine = StartCoroutine(movementLoop());
        }
    }

    IEnumerator movementLoop()
    {
        while (true)
        {
            int x = (int)transform.right.x;
            int y = (int)transform.right.y;
            tiled.MoveToTile(tiled.TileX + x, tiled.TileY + y, MoveDuration);
            yield return new WaitForSeconds(MoveDuration);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            ResetPosition();
        }
    }
}
