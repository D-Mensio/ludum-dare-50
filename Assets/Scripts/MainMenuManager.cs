using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Image cupFill;

    private float progress = 0.5f;

    private float changeDelay;

    private bool rising = true;

    public void Play()
    {
        SceneTransitionManager.Instance.LoadScene("Gameplay");
    }

    public void Tutorial()
    {
        SceneTransitionManager.Instance.LoadScene("Tutorial");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        changeDelay -= Time.deltaTime;
        if (changeDelay <= 0)
        {
            changeDelay = Random.Range(5f, 10f);
            rising = !rising;
        }
        if (rising)
            progress += 0.1f * Time.deltaTime;
        else
            progress -= 0.5f * Time.deltaTime;
        progress = Mathf.Clamp(progress, 0.5f, 1);
        cupFill.fillAmount = progress;
    }
}
