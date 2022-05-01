using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private bool gameOver;

    [SerializeField]
    private InputManager im;
    [SerializeField]
    private CameraController cc;
    [SerializeField]
    private Animator gameOverAC;
    [SerializeField]
    private Animator bottomUIAC;
    [SerializeField]
    private GameObject confirmPanel;

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
            gameOverAC.SetTrigger("GameOver");
            bottomUIAC.SetTrigger("GameOver");
            HideConfirmPanel();
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

    public void ShowConfirmPanel()
    {
        if (!gameOver)
        {
            confirmPanel.transform.localScale = Vector3.one;
        }
    }

    public void HideConfirmPanel()
    {
        confirmPanel.transform.localScale = Vector3.zero;
    }
}
