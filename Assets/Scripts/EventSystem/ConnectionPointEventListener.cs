using UnityEngine;
using UnityEngine.Events;

public class ConnectionPointEventListener : MonoBehaviour
{
    public ConnectionPointEvent Event;
    public UnityEvent<ConnectionPoint> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(ConnectionPoint cp)
    {
        Response.Invoke(cp);
    }
}
