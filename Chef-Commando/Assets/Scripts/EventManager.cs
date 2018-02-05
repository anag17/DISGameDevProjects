// modified code from https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
/// <summary>
/// Event manager listens for ceratin events then triggers other functions
/// </summary>
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {


	//Dictionary declaration
	private Dictionary <Event, UnityEvent> eventDictionary;
	// EventManager object
	private static EventManager eventManager;

	// eventmanager instance
	public static EventManager instance
	{
		get
		{
			if (!eventManager)
			{
				eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

				// tell user eventmanager needs to be in scene
				if (!eventManager)
				{
					Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
				}
				else
				{
					eventManager.Init (); 
				}
			}

			return eventManager;
		}
	}

	// Create dictionary of events
	void Init ()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<Event, UnityEvent>();
		}
	}

	//Start listening  for an event
	public static void StartListening (Event eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.AddListener (listener);
		} 
		else
		{
			thisEvent = new UnityEvent ();
			thisEvent.AddListener (listener);
			instance.eventDictionary.Add (eventName, thisEvent);
		}
	}

	public static void StopListening (Event eventName, UnityAction listener)
	{
		if (eventManager == null) return;
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.RemoveListener (listener);
		}
	}


	// Trigger an event
	public static void TriggerEvent (Event eventName)
	{
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent))
		{
			thisEvent.Invoke ();
		}
	}
}

// look for strings
public static class Events {
    public static Event enemyHit = new Event();
    public static Event enemyKill = new Event();
    public static Event playerHit = new Event();
	public static Event playerDead = new Event ();
}

// look for eventname
public class Event {

    public Event() {
    }
}
