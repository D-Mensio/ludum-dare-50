using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private int totalAnimationStates;
    private int currentAnimState;

    [SerializeField]
    private Button leftArrow;
    [SerializeField]
    private Button rightArrow;
    [SerializeField]
    private Animator tutAnim;

    public void BackMainMenu()
    {
        SceneTransitionManager.Instance.LoadScene("MainMenu");
    }

    public void Forward()
    {
        if (currentAnimState == 0)
            leftArrow.interactable = true;
        if (currentAnimState == totalAnimationStates - 2)
            rightArrow.interactable = false;
        currentAnimState++;
        tutAnim.SetTrigger("Forward");
    }

    public void Backwards()
    {
        if (currentAnimState == totalAnimationStates - 1)
            rightArrow.interactable = true;
        if (currentAnimState == 1)
            leftArrow.interactable = false;
        currentAnimState--;
        tutAnim.SetTrigger("Backwards");
    }

}
