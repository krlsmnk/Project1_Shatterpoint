using UnityEngine;
using System.Collections;  // For coroutines

public class ELK_Depressurization : MonoBehaviour
{
    public GameObject airParticle;
   // public float DefrostDuration = 5f;  // Time to keep the GameObject enabled

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
        if (eventName == "DepressurizationCabin")
        {
            // Enable the GameObject
            airParticle.SetActive(true);
        }
        else if (eventName == "GlassShatter") airParticle.SetActive(false);
    }
}
