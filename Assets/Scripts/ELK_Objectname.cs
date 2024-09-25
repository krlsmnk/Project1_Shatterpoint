using UnityEngine;

public class ELK_Objectname : MonoBehaviour
{
    private void OnEnable()
    {
        EventManagerKarl.OnEventTriggered += EventHandler;  // Subscribe to the event
    }

    private void OnDisable()
    {
        EventManagerKarl.OnEventTriggered -= EventHandler;  // Unsubscribe from the event
    }

    // Event handler that gets triggered by EventManagerKarl
    public void EventHandler(string eventName)
    {
        Debug.Log($"{gameObject.name} received event: {eventName}");
    }
}
