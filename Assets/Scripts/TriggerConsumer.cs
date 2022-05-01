using UnityEngine;

public class TriggerConsumer : MonoBehaviour
{
    [SerializeField]
    private Animator ac;

    public void ResetTrigger(string name)
    {
        ac.ResetTrigger(name);
    }
}
