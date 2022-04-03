using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tut1;

    public GameObject tut2;

    public Button leftArrow;

    public Button rightArrow;


    public void BackMainMenu()
    {
        SceneTransitionManager.Instance.LoadScene("MainMenu");
    }

    public void Show1()
    {
        leftArrow.interactable = false;
        rightArrow.interactable = true;
        tut1.transform.localScale = Vector3.one;
        tut2.transform.localScale = Vector3.zero;
    }

    public void Show2()
    {
        leftArrow.interactable = true;
        rightArrow.interactable = false;
        tut2.transform.localScale = Vector3.one;
        tut1.transform.localScale = Vector3.zero;
    }

}
