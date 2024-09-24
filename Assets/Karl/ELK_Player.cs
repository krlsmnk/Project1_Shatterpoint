using UnityEngine;
using UnityEngine.UI;  // For RawImage
using System.Collections;

public class ELK_Player : MonoBehaviour
{
    // Public properties you can configure from the Inspector
    public float defrostTime = 5f;  // Time to fully defrost the visor
    public float shakeDuration = 1f;  // Default camera shake duration
    public float fadeOutTime = 2f;  // Time for screen fade out
    public float fadeInTime = 2f;  // Time for screen fade in

    public Transform Visor, helmet, suckLocation;
    public RawImage screenFadeImage; // Reference to the RawImage used for fading the screen
    private Material visorMaterial;  // Store the material of the visor
    private Camera playerCamera;  // Reference to the player's camera

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
        // Find and cache the visor material and player camera
        visorMaterial = Visor.GetComponent<Renderer>().material;
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Event handler that gets triggered by EventManagerKarl
    public void EventHandler(string eventName)
    {
        switch (eventName)
        {
            case "HelmetDefrosts":
                StartCoroutine(AdjustVisorTransparencyAndDistortion(1, 0, defrostTime));
                break;
            case "Impact1":
                GetComponent<VRShake>().ShakeVR(shakeDuration);
                break;
            case "EarsRinging":
                StartCoroutine(FadeScreenToAlpha(1, fadeOutTime)); // Fade to fully white
                break;
            case "VisAudioFadeIn":
                StartCoroutine(FadeScreenToAlpha(0, fadeInTime)); // Fade back to transparent
                break;
            case "DepressurizationCabin":
                StartCoroutine(AdjustVisorTransparencyAndDistortion(0,0.05f, defrostTime));
                break;
            case "Impact2":
                GetComponent<VRShake>().ShakeVR(shakeDuration);
                break;
            case "AirRush":
                StartCoroutine(AdjustVisorTransparencyAndDistortion(0.05f, 1f, defrostTime));
                //Suck the player 
                StartCoroutine(MoveObject(gameObject.transform, suckLocation, 5.7f, false));
                //Suck the helmet
                //StartCoroutine(MoveObject(helmet.transform, suckLocation, 5.7f, true));
                break;
        }
    }

    // Coroutine to adjust the transparency and distortion of the visor over time
    private IEnumerator AdjustVisorTransparencyAndDistortion(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);

            // Update the visor material's properties
            visorMaterial.SetFloat("_Transparency", newValue);
            visorMaterial.SetFloat("_Distortion", newValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set correctly
        visorMaterial.SetFloat("_Transparency", endValue);
        visorMaterial.SetFloat("_Distortion", endValue);
    }

    // Coroutine to fade the screen to a specific alpha over time using RawImage
    private IEnumerator FadeScreenToAlpha(float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color currentColor = screenFadeImage.color;  // Get the current color of the RawImage
        float initialAlpha = currentColor.a;  // Store the initial alpha value

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
            screenFadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);  // Set the new alpha value
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is set correctly
        screenFadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }

    private IEnumerator MoveObject(Transform objectToMove, Transform endLocation, float duration, bool tumble)
    {
       // Random axis of rotation
        Vector3 randomRotationAxis = Random.onUnitSphere;  // A random axis
        float elapsedTime = 0f;

        // Initial position
        Vector3 initialPosition = objectToMove.position;
        Vector3 targetPosition = endLocation.position;

        while (elapsedTime < duration)
        {
            // Move asteroid towards target over time
            float t = elapsedTime / duration;
            objectToMove.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Apply random rotation over time
            if(tumble) objectToMove.transform.Rotate(randomRotationAxis, 360 * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the objectToMove reaches the exact target position
        objectToMove.transform.position = targetPosition;
    }

}
