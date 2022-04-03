using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public bool gameOver;

    public InputManager im;

    public CameraController cc;

    public GameObject gameOverScreen;

    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void GameOver(Vector3 pos)
    {
        if (!gameOver)
        {
            gameOver = true;
            im.GameOver();
            Time.timeScale = 0;
            cc.Converge(pos);
            gameOverScreen.SetActive(true);
        }
    }

    public void Retry()
    {
        SceneTransitionManager.Instance.LoadScene("Gameplay");
    }
    public void MainMenu()
    {
        SceneTransitionManager.Instance.LoadScene("MainMenu");
    }
}
