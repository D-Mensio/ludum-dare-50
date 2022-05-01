using UnityEngine;
using UnityEngine.Events;

public class ToggleTrueButton : MonoBehaviour
{
    [SerializeField]
    private UnityEvent unityEvent;
    
    public void OnChange(bool value)
    {
        if (value)
        {
            unityEvent.Invoke();
        }
    }
}
