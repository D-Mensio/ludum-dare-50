using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton
{
    [SerializeField]
    Animator transitionAnimator;

    public bool loadingScene = false;
    private bool transitionPlaying = false;


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
        yield return TimeTransition();
        while (!(loadSceneAsync.isDone && !transitionPlaying))
        {
            yield return null;
        }
        transitionAnimator.SetBool("Transition", false);
        yield return TimeTransition();
        loadingScene = false;
    }

    private IEnumerator TimeTransition()
    {
        yield return new WaitForSeconds(0.34f);
    }
}
