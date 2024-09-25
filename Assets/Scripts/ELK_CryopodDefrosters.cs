using UnityEngine;
using System.Collections;  // For coroutines

public class ELK_CryopodDefrosters : MonoBehaviour
{
    public GameObject frostParticle;
    public float DefrostDuration = 5f;  // Time to keep the GameObject enabled

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
        if (eventName == "CrypodOpens")
        {
            StartCoroutine(EnableForDuration(DefrostDuration));  // Start the coroutine
        }
    }

    // Coroutine to enable the GameObject for a specific duration and then disable it
    private IEnumerator EnableForDuration(float duration)
    {
        // Enable the GameObject
        frostParticle.SetActive(true);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Disable the GameObject
        frostParticle.SetActive(false);
    }
}
