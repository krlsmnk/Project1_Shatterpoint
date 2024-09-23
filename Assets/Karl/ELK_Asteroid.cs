using UnityEngine;
using System.Collections;

public class ELK_Asteroid : MonoBehaviour
{
    public GameObject asteroid;
    public Transform startLocation1, startLocation2, impactLocation, endLocation1, endLocation2;
    public float timeToImpact1, timeToImpact2;

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
        if (eventName == "CollisionWarning")
        {
            StartCoroutine(MoveAsteroid(startLocation1, impactLocation, timeToImpact1));
        }
        else if (eventName == "GlassCrack")
        {
            StartCoroutine(MoveAsteroid(startLocation2, impactLocation, timeToImpact2));
        }
        else if (eventName == "Impact1")
        {
            StartCoroutine(MoveAsteroid(impactLocation, endLocation1, timeToImpact2));
        }
        else if (eventName == "Impact2")
        {
            StartCoroutine(MoveAsteroid(impactLocation, endLocation2, timeToImpact2));
        }
    }

    // Coroutine to move the asteroid from a start location to an end location
    private IEnumerator MoveAsteroid(Transform startLocation, Transform endLocation, float duration)
    {
        // Teleport asteroid to start location
        asteroid.transform.position = startLocation.position;

        // Random axis of rotation
        Vector3 randomRotationAxis = Random.onUnitSphere;  // A random axis
        float elapsedTime = 0f;

        // Initial position
        Vector3 initialPosition = startLocation.position;
        Vector3 targetPosition = endLocation.position;

        while (elapsedTime < duration)
        {
            // Move asteroid towards target over time
            float t = elapsedTime / duration;
            asteroid.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Apply random rotation over time
            asteroid.transform.Rotate(randomRotationAxis, 360 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the asteroid reaches the exact target position
        asteroid.transform.position = targetPosition;
    }
}
