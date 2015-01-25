using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {
    public static GameControl Instance;

    public CanvasRenderer GameOverScreen;

    public void Awake() {
        Instance = this;
    }

    public void CheckGameOver(PlayerController dying_player)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        bool active_player = false;
        foreach (PlayerController p in players) {
            if (p == dying_player) continue;
            if (p.controllable || p.direction != PlayerDirection.None)
            {
                active_player = true;
                break;
            }
        }

        if (!active_player)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
    }

    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
        GameOverScreen.gameObject.SetActive(false);
    }
}
