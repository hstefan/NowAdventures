﻿using UnityEngine;
using System.Collections;

public enum PlayerDirection
{
    None, Up, Down, Left, Right
}

public enum PlayerItem
{
    None = 0,
    Sword,
    Boots,
    PowerGloves
}

[System.Serializable]
public class ItemUses {
    public PlayerItem Item;
    public int Uses;
}

[RequireComponent(typeof(TiledCharacter))]
public class PlayerController : MonoBehaviour
{
    private TiledCharacter tiled;
    private Animator anim;

    [SerializeField]
    private float moveDuration;
    [SerializeField]
    private float moveInterval;

    [System.NonSerialized]
    public PlayerDirection direction;
    private PlayerDirection new_direction;
    [SerializeField, HideInInspector]
    public PlayerItem equippedItem;
    [SerializeField]
    public ItemUses[] usesPerItem;
    [SerializeField]
    private bool canClone = true;
    [System.NonSerialized]
    public bool controllable;

    void Awake()
    {
        tiled = GetComponent<TiledCharacter>();
        anim = GetComponent<Animator>();

        direction = PlayerDirection.None;
        new_direction = PlayerDirection.None;
        equippedItem = PlayerItem.None;
        controllable = true;

        anim.SetFloat("DirX", 0f);
        anim.SetFloat("DirY", -1f);
        anim.SetFloat("Speed", 0f);
    }

    void Start()
    {
        if (!controllable)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color col = renderer.material.color;
            col.a = 0.5f;
            renderer.material.color = col;
        }

        StartCoroutine(movementLoop());
    }

    void Update()
    {
        if (controllable)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { new_direction = PlayerDirection.Up; }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { new_direction = PlayerDirection.Down; }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { new_direction = PlayerDirection.Left; }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { new_direction = PlayerDirection.Right; }

            if (Input.GetKeyDown(KeyCode.Alpha1)) { equipItem(PlayerItem.Sword); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { equipItem(PlayerItem.Boots); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { equipItem(PlayerItem.PowerGloves); }

            if (Input.GetKeyDown(KeyCode.R)) { GameControl.Instance.RestartGame(); }
        }
    }

    static PlayerDirection ReverseDirection(PlayerDirection dir)
    {
        switch (dir)
        {
            case PlayerDirection.None: return PlayerDirection.None;
            case PlayerDirection.Left: return PlayerDirection.Right;
            case PlayerDirection.Right: return PlayerDirection.Left;
            case PlayerDirection.Up: return PlayerDirection.Down;
            case PlayerDirection.Down: return PlayerDirection.Up;
            default: return PlayerDirection.None;
        }
    }

    private void equipItem(PlayerItem item) {
        var usedata = System.Array.Find<ItemUses>(usesPerItem, i => i.Item == item);
        if (usedata.Uses > 0) {
            usedata.Uses--;
            equippedItem = item;
        }
    }

    public void addItem(PlayerItem item) {
        var usedata = System.Array.Find<ItemUses>(usesPerItem, i => i.Item == item);
        usedata.Uses++;
        Debug.Log(usedata.Uses);
    }

    private IEnumerator movementLoop() {
        while (true)
        {
            if (new_direction == ReverseDirection(direction))
            {
                new_direction = PlayerDirection.None;
            }

            float move_interval = equippedItem == PlayerItem.Boots ? moveInterval * 0.5f : moveInterval;
            float move_duration = equippedItem == PlayerItem.Boots ? move_interval : moveDuration;

            int new_x = -1;
            int new_y = -1;
            bool moved = false;
            bool blocked = false;

            if (new_direction != PlayerDirection.None)
            {
                TryToMoveInDirection(new_direction, move_duration, out blocked, out moved, out new_x, out new_y);

                if (blocked)
                {
                    // Ignore keypress
                    new_direction = PlayerDirection.None;
                }
                else if (moved)
                {
                    if (direction != PlayerDirection.None && canClone)
                    {
                        // Split player
                        GameObject new_player_go = (GameObject)Instantiate(gameObject);
                        var player_controller = new_player_go.GetComponent<PlayerController>();
                        player_controller.new_direction = direction;
                        player_controller.direction = PlayerDirection.None;
                        player_controller.equippedItem = equippedItem;
                        player_controller.controllable = false;
                    }
                }
            }

            if (new_direction == PlayerDirection.None && direction != PlayerDirection.None)
            {
                TryToMoveInDirection(direction, move_duration, out blocked, out moved, out new_x, out new_y);
            }

            if (blocked)
            {
                direction = PlayerDirection.None;
                GameControl.Instance.CheckGameOver();
                anim.SetFloat("Speed", 0f);
            }
            else if (moved)
            {
                if (anim != null) {
                    anim.SetFloat("DirX", new_x - tiled.TileX);
                    anim.SetFloat("DirY", new_y - tiled.TileY);
                    var spd = 1f;
                    if (equippedItem == PlayerItem.Boots) {
                        spd = spd * 2f;
                    }
                    anim.SetFloat("Speed", spd);
                }
                tiled.MoveToTile(new_x, new_y, move_duration);
            }

            if (new_direction != PlayerDirection.None)
            {
                direction = new_direction;
                new_direction = PlayerDirection.None;
            }

            yield return new WaitForSeconds(move_interval);
        }
    }

    private void TryToMoveInDirection(PlayerDirection new_dir, float move_duration, out bool blocked, out bool moved, out int out_x, out int out_y)
    {
        int x = tiled.TileX;
        int y = tiled.TileY;
        Vector2 vdir = Vector2.zero;
        switch (new_dir)
        {
            case PlayerDirection.Up:
                y += 1;
                vdir = Vector2.up;
                break;
            case PlayerDirection.Down:
                y -= 1;
                vdir = -Vector2.up;
                break;
            case PlayerDirection.Left:
                x -= 1;
                vdir = -Vector2.right;
                break;
            case PlayerDirection.Right:
                x += 1;
                vdir = Vector2.right;
                break;
            default: break;
        }

        anim.SetBool("Pushing", false);
        Collider2D coll = tiled.CollideWithBlocker(vdir);
        if (coll != null)
        {
            if (coll.CompareTag("crate"))
            {
                Crate crate = coll.GetComponent<Crate>();
                if (equippedItem == PlayerItem.PowerGloves && crate.CanMove(new_direction))
                {
                    crate.Push(new_dir, move_duration * 0.75f);
                    out_x = out_y = -1;
                    moved = false;
                    blocked = false;
                    anim.SetBool("Pushing", true);
                }
                else
                {
                    out_x = out_y = -1;
                    moved = false;
                    blocked = true;
                }
            }
            else
            {
                out_x = out_y = -1;
                moved = false;
                blocked = true;
            }
        }
        else
        {
            out_x = x;
            out_y = y;
            moved = true;
            blocked = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("slime"))
        {
            if (equippedItem == PlayerItem.Sword)
            {
                anim.SetTrigger("Attacking");
                Destroy(other.gameObject);
            }
            else
            {
                anim.SetFloat("DeathType", 0f);
                die();
            }
        }
        else if (other.CompareTag("exit_door"))
        {
            die();
        }
        else if (other.CompareTag("hole"))
        {
            anim.SetFloat("DeathType", 1f);
            die();
        }
        else if (other.CompareTag("arrow"))
        {
            anim.SetFloat("DeathType", 0f);
            die();
            other.SendMessage("ResetPosition");
        }
    }

    private void die()
    {
        controllable = false;
        new_direction = PlayerDirection.None;
        direction = PlayerDirection.None;
        anim.SetTrigger("Dying");
        gameObject.tag = null;
        collider2D.enabled = false;
        StartCoroutine(WaitAndDie(2f));
    }

    private IEnumerator WaitAndDie(float t) {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
        GameControl.Instance.CheckGameOver();
    }
}
