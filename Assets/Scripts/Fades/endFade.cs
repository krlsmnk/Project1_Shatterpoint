using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasFadeToImage : MonoBehaviour
{
    // Reference to the RawImage component (used to create the fade effect)
    public RawImage customImageFade;

    // Duration to wait before starting the fade
    // public float delay = 2f;

    // Duration of the fade-out effect
    private float fadeDuration = 1f;


    private void OnEnable()
    {
        EventManager.Instance.AddListener("outro.begin", BeginFadeOut);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("outro.begin", BeginFadeOut);
    }

    void Start()
    {
        if (customImageFade != null)
        {
            // Ensure the custom image starts fully transparent
            customImageFade.color = new Color(1f, 1f, 1f, 0f);

        }
        else
        {
            Debug.LogError("No RawImage assigned. Please assign a RawImage in the inspector.");
        }
    }

    void BeginFadeOut(AudioClip clip, float duration)
    {
        fadeDuration = duration;
        StartCoroutine(FadeToCustomImage());
    }

    IEnumerator FadeToCustomImage()
    {
        // Wait for the initial delay
        // yield return new WaitForSeconds(delay);

        // Gradually increase the alpha over time to fade in the custom image
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            customImageFade.color = new Color(1f, 1f, 1f, alpha); // Keep RGB at 1 to maintain your image color
            yield return null;
        }

        // Ensure the alpha is set to 1 at the end
        customImageFade.color = new Color(1f, 1f, 1f, 1f);
    }
}
