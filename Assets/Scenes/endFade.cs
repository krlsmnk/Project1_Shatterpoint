using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Make sure to include this for UI components

public class CanvasFadeToBlack : MonoBehaviour
{
    // Reference to the RawImage component (used to create the black fade)
    public RawImage blackScreen;

    // Duration to wait before starting the fade
    public float delay = 2f;
    // Duration of the fade-out effect
    public float fadeDuration = 1f;

    void Start()
    {
        if (blackScreen != null)
        {
            // Set the image to fully transparent black
            blackScreen.color = new Color(0f, 0f, 0f, 0f);

            // Start the fade-out coroutine after the specified delay
            StartCoroutine(FadeToBlack());
        }
        else
        {
            Debug.LogError("No RawImage assigned. Please assign a RawImage in the inspector.");
        }
    }

    IEnumerator FadeToBlack()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(delay);

        // Gradually increase the alpha over time to fade to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            blackScreen.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Ensure the alpha is set to 1 at the end
        blackScreen.color = new Color(0f, 0f, 0f, 1f);
    }
}
