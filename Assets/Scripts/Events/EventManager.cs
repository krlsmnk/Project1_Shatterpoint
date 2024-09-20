using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("EventManager");
                _instance = go.AddComponent<EventManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }

    }

    // Clean up the event manager when the game is over
    private void OnDestroy()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }

    private Dictionary<string, Action<AudioClip, float>> eventDictionary = new Dictionary<string, Action<AudioClip, float>>();

    public void AddListener(string eventName, Action<AudioClip, float> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            eventDictionary[eventName] += listener;
        }
    }

    public void RemoveListener(string eventName, Action<AudioClip, float> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName, AudioClip audioClip, float duration)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(audioClip, duration);
        }
    }

    public void TriggerEvent(string eventName, AudioClip audioClip)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(audioClip, audioClip.length);
        }
    }
}