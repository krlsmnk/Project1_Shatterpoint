using UnityEngine;

public class ELK_Siren : MonoBehaviour
{
    public GameObject spotlight;


    private void OnEnable()
    {
        EventManagerKarl.OnEventTriggered += EventHandler;  // Subscribe to the event
    }

    private void OnDisable()
    {
        EventManagerKarl.OnEventTriggered -= EventHandler;  // Unsubscribe from the event
    }
    private void Start()
    {
        // Get the Light component from the GameObject (assuming it is a spotlight)
        //spotlight = GetComponent<Light>();

        // Ensure the spotlight is initially disabled
        if (spotlight != null)
        {
            spotlight.SetActive(false);
        }
    }

    // Event handler that gets triggered by EventManagerKarl
    public void EventHandler(string eventName)
    {
        if (eventName == "EarsRinging")
        {
            // Enable the spotlight component
            if (spotlight != null)
            {
                spotlight.SetActive(true);
                Debug.Log("Spotlight enabled due to EarsRinging event");
            }
        }

    }
}
