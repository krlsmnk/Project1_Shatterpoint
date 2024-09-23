using UnityEngine;
using System.Collections;

public class ELK_GlassCracks : MonoBehaviour
{
    public GameObject glassCrackPrefab;  // Assign the GlassCrack decal prefab in the Inspector
    public Transform window;             // The window GameObject where the cracks will appear
    public float d1 = 1f;                // Delay for first crack
    public float d2 = 2f;                // Delay for second crack
    public float d3 = 3f;                // Delay for third crack
    public float d4 = 4f;                // Delay for fourth crack

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
        if (eventName == "GlassCrack")
        {
            StartCoroutine(SpawnGlassCracks());
        }
    }

    // Coroutine to spawn glass cracks at different intervals
    private IEnumerator SpawnGlassCracks()
    {
        // Spawn first crack after d1 seconds
        yield return new WaitForSeconds(d1);
        SpawnCrack(0.0f, 0.0f, 8f);  // Example values for xOffset, yOffset, scale

        // Spawn second crack after d2 seconds
        yield return new WaitForSeconds(d2);
        SpawnCrack(-4.15f, 2.9f, 2.1f);

        // Spawn third crack after d3 seconds
        yield return new WaitForSeconds(d3);
        SpawnCrack(5.05f, -5.25f, 3.6f);

        // Spawn fourth crack after d4 seconds
        yield return new WaitForSeconds(d4);
        SpawnCrack(-6.1f, -6.2f, 5.1f);
    }

    // Function to instantiate the glass crack decal with position offset and scale
    private void SpawnCrack(float xOffset, float yOffset, float scale)
    {
        // Ensure both the prefab and the window reference are valid
        if (glassCrackPrefab != null && window != null)
        {
            // Calculate the position of the crack with the given offsets
            Vector3 spawnPosition = window.position + window.right * xOffset + window.up * yOffset;

            // Instantiate the glass crack at the calculated position
            GameObject crackInstance = Instantiate(glassCrackPrefab, spawnPosition, window.rotation, window);

            // Apply the scale to the spawned crack
            crackInstance.transform.localScale = new Vector3(scale, scale, scale);
            Debug.LogWarning("Spawned");
        }
        else
        {
            Debug.LogWarning("GlassCrack prefab or Window reference is missing!");
        }
    }
}