using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {
    public static GameControl Instance;

    public CanvasRenderer GameOverScreen;

    public void Awake() {
        Instance = this;
    }

    public void GameOver() {
        GameOverScreen.gameObject.SetActive(true);
    }
    public void RestartGame() {
        Application.LoadLevel(Application.loadedLevel);
        GameOverScreen.gameObject.SetActive(false);
    }
}
