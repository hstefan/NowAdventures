using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TiledCharacter))]
public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float moveDuration;

    private TiledCharacter tiled;

    void Awake()
    {
        tiled = GetComponent<TiledCharacter>();
    }

    void Update()
    {
        var hits = Physics2D.RaycastAll(tiled.GetRealPosition(), transform.right);
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i].collider == collider2D) { continue; }
            if (!hits[i].collider.isTrigger) { break; }
            if (hits[i].collider.CompareTag("player")) { ActivateEffect(); }
        }
    }

    public void ActivateEffect()
    {
        StartCoroutine(movementLoop());
    }

    IEnumerator movementLoop()
    {
        while (true)
        {
            int x = (int)transform.right.x;
            int y = (int)transform.right.y;
            tiled.MoveToTile(tiled.TileX + x, tiled.TileY + y, moveDuration);
            yield return new WaitForSeconds(moveDuration);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            Destroy(gameObject);
        }
    }
}
