using UnityEngine;

public class ELK_Siren : MonoBehaviour
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
        if (eventName == "EarsRinging") Debug.Log("Enable Red Light");
        else {
            Debug.Log($"{gameObject.name} received event: {eventName}");
        }

    }
}
