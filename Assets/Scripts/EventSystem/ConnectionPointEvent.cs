using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConnectionPointEvent : ScriptableObject
{
	private List<ConnectionPointEventListener> listeners =	new List<ConnectionPointEventListener>();

	public void Raise(ConnectionPoint cp)
	{
		for (int i = 0; i < listeners.Count; i++)
		{
			listeners[i].OnEventRaised(cp);
		}
	}

	public void RegisterListener(ConnectionPointEventListener listener)
	{ 
		listeners.Add(listener); 
	}

	public void UnregisterListener(ConnectionPointEventListener listener)
	{ 
		listeners.Remove(listener); 
	}
}