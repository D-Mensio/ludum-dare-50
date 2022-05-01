using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]
    private Animator transitionAnimator;

    private bool loadingScene = false;

    public static SceneTransitionManager Instance { get; private set; }

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

    private void Start()
    {
        LoadScene("MainMenu");
    }

    public void LoadScene(string s)
    {
        if (!loadingScene)
        {
            StartCoroutine(TransitionAndLoadScene(s));
        }
    }

    private IEnumerator TransitionAndLoadScene(string s)
    {
        loadingScene = true;
        transitionAnimator.SetBool("Transition", true);
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(s);
        loadSceneAsync.allowSceneActivation = false;
        yield return TimeTransition();
        while (!(loadSceneAsync.isDone))
        {
            if (loadSceneAsync.progress >= 0.9f)
                loadSceneAsync.allowSceneActivation = true;
            yield return null;
        }
        transitionAnimator.SetBool("Transition", false);
        yield return TimeTransition();
        Time.timeScale = 1;
        loadingScene = false;
    }

    private IEnumerator TimeTransition()
    {
        yield return new WaitForSecondsRealtime(0.5f);
    }


}
