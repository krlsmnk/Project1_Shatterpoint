using UnityEngine;
using System.Collections;

public class CanvasFadeOut : MonoBehaviour
{
    // Reference to the CanvasGroup component
    private CanvasGroup canvasGroup;

    // Duration to wait before starting the fade
    public float delay = 2f;
    // Duration of the fade-out effect
    public float fadeDuration = 1f;

    void Start()
    {
        // Get the CanvasGroup component attached to the Canvas
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            // Start the fade-out coroutine after the specified delay
            StartCoroutine(FadeOutCanvas());
        }
        else
        {
            Debug.LogError("No CanvasGroup component found. Please add a CanvasGroup component to the Canvas.");
        }
    }

    IEnumerator FadeOutCanvas()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(delay);

        // Gradually reduce the alpha over time
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the alpha is set to 0 at the end
        canvasGroup.alpha = 0f;
    }
}
