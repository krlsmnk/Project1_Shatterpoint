using UnityEngine;

public class FadeInEffect : MonoBehaviour
{
    public float fadeDuration = 2.0f; // Duration for the fade-in effect in seconds
    private CanvasGroup canvasGroup;
    private float fadeElapsedTime = 0f;
    private bool fadingIn = true;

    void Start()
    {
        // Get the CanvasGroup component
        canvasGroup = GetComponent<CanvasGroup>();

        // Start with a fully transparent canvas
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (fadingIn)
        {
            // Increment elapsed time
            fadeElapsedTime += Time.deltaTime;

            // Calculate the new alpha value
            canvasGroup.alpha = Mathf.Clamp01(fadeElapsedTime / fadeDuration);

            // Check if the fade-in is complete
            if (canvasGroup.alpha >= 1f)
            {
                fadingIn = false;
            }
        }
    }
}
