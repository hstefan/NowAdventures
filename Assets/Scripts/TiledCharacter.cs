using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TiledCharacter : MonoBehaviour {
    [SerializeField]
    private Transform mapOrigin;
    public int TileX;
    public int TileY;
    [SerializeField]
    private Vector3 BaseOffset;
    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            enabled = false;
        }
        else
        {
            transform.position = getPositionForTile(TileX, TileY);
        }
    }
#endif

    public Vector3 GetRealPosition()
    {
        return getPositionForTile(TileX, TileY);
    }

    private Vector3 getPositionForTile(int x, int y)
    {
        return mapOrigin.transform.position + BaseOffset + new Vector3(x, y, 0f);
    }

    public void MoveToTile(int x, int y, float duration)
    {
        StartCoroutine(MoveToTileCo(x, y, duration));
    }

    public IEnumerator MoveToTileCo(int x, int y, float duration)
    {
        var speed = new Vector2(x - TileX, y - TileY);
        TileX = x;
        TileY = y;

        Vector3 prev_pos = transform.position;
        Vector3 new_pos = getPositionForTile(x, y);
        if (anim != null) {
            anim.SetInteger("SpeedX", Mathf.RoundToInt(speed.x));
            anim.SetInteger("SpeedY", Mathf.RoundToInt(speed.y));
        }
        
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(prev_pos, new_pos, t / duration);
            yield return null;
        }
        rigidbody2D.MovePosition(new_pos);
    }

    public Collider2D CollideWithBlocker(Vector2 direction, Collider2D skip = null) {
        var hits = Physics2D.RaycastAll(GetRealPosition(), direction, 1.0f);
        for (int i = 0; i < hits.Length; ++i) {
            if (hits[i].collider == skip) { continue; }
            if (!hits[i].collider.isTrigger) { return hits[i].collider; }
        }
        return null;
    }

    public static Vector2 Vector2FromPlayerDirection(PlayerDirection direction)
    {
        switch (direction)
        {
            case PlayerDirection.Up:
                return Vector2.up;
            case PlayerDirection.Down:
                return -Vector2.up;
            case PlayerDirection.Left:
                return -Vector2.right;
            case PlayerDirection.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }
}
