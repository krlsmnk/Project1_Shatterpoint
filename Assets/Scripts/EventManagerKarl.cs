using UnityEngine;
using System.Collections;

public class EventManagerKarl : MonoBehaviour
{
    [System.Serializable]
    public struct EventData
    {
        public string eventName;
        public float delay;
    }

    // Public array of event names and their delays
    public EventData[] eventsToSchedule;

    public delegate void EventAction(string eventName);
    public static event EventAction OnEventTriggered;

    private void OnEnable()
    {
        // Schedule each event in the array
        foreach (var eventData in eventsToSchedule)
        {
            StartCoroutine(ScheduleEvent(eventData.delay, eventData.eventName));
        }
    }

    private IEnumerator ScheduleEvent(float delay, string eventName)
    {
        yield return new WaitForSeconds(delay);
        OnEventTriggered?.Invoke(eventName);  // Trigger the event
    }
}
