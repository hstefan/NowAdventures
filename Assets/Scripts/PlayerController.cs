﻿using UnityEngine;
using System.Collections;

public enum PlayerDirection
{
    None, Up, Down, Left, Right
}

enum PlayerItem
{
    None = 0,
    Sword,
    Boots,
    PowerGloves
}

[RequireComponent(typeof(TiledCharacter))]
public class PlayerController : MonoBehaviour
{
    private TiledCharacter tiled;

    [SerializeField]
    private float moveDuration;
    [SerializeField]
    private float moveInterval;

    [SerializeField, HideInInspector]
    private PlayerDirection direction;
    private PlayerDirection new_direction;
    [SerializeField, HideInInspector]
    private PlayerItem equippedItem;

    private bool controllable;

    void Awake()
    {
        tiled = GetComponent<TiledCharacter>();
        direction = PlayerDirection.None;
        new_direction = PlayerDirection.None;
        equippedItem = PlayerItem.None;
    }

    void Start()
    {
        bool first_player = FindObjectsOfType(typeof(PlayerController)).Length <= 1;
        Debug.Log(FindObjectsOfType(typeof(PlayerController)).Length);
        controllable = first_player;

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

            if (Input.GetKeyDown(KeyCode.Alpha1)) { equippedItem = PlayerItem.Sword; }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { equippedItem = PlayerItem.Boots; }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { equippedItem = PlayerItem.PowerGloves; }
        }
    }

    private IEnumerator movementLoop() {
        while (true)
        {
            if (direction != new_direction)
            {
                if (direction != PlayerDirection.None)
                {
                    // Split player
                    GameObject new_player_go = (GameObject)Instantiate(gameObject);
                    var player_controller = new_player_go.GetComponent<PlayerController>();
                    player_controller.new_direction = direction;
                    player_controller.direction = PlayerDirection.None;
                }
            }
            direction = new_direction;
            if (direction != PlayerDirection.None)
            {
                int x = tiled.TileX;
                int y = tiled.TileY;
                Vector2 dir = Vector2.zero;
                switch (direction)
                {
                    case PlayerDirection.Up:
                        y += 1;
                        dir = Vector2.up;
                        break;
                    case PlayerDirection.Down:
                        y -= 1;
                        dir = -Vector2.up;
                        break;
                    case PlayerDirection.Left:
                        x -= 1;
                        dir = -Vector2.right;
                        break;
                    case PlayerDirection.Right:
                        x += 1;
                        dir = Vector2.right;
                        break;
                    default: break;
                }

                Collider2D coll = tiled.CollideWithBlocker(dir);
                if (coll != null)
                {
                    if (coll.CompareTag("crate"))
                    {
                        Debug.Log("hi crate!");
                        Crate crate = coll.GetComponent<Crate>();
                        if (equippedItem == PlayerItem.PowerGloves && crate.CanMove(direction))
                        {
                            Debug.Log("PUSHIN' MAH CRATES");
                            crate.Push(direction, moveDuration * 0.75f);
                        }
                        else
                        {
                            direction = PlayerDirection.None;
                        }
                    }
                    else
                    {
                        direction = PlayerDirection.None;
                    }
                }
                else
                {
                    tiled.MoveToTile(x, y, moveDuration);
                }
            }

            yield return new WaitForSeconds(moveInterval);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("slime"))
        {
            if (equippedItem == PlayerItem.Sword)
            {
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("exit_door"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("hole"))
        {
            Destroy(gameObject);
        }
    }
}