using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExteriorLight : MonoBehaviour
{

    private float fadeDuration = 1f;

    public float maxIntensity = 0.35f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener("exterior.light.fade", BeginFadeIn);
        EventManager.Instance.AddListener("exterior.light.break", BeginBreakLight);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("exterior.light.fade", BeginFadeIn);
        EventManager.Instance.RemoveListener("exterior.light.break", BeginBreakLight);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BeginFadeIn(AudioClip clip, float duration)
    {
        fadeDuration = duration;
        StartCoroutine(FadeInLight());
    }

    void BeginBreakLight(AudioClip clip, float duration)
    {
        GetComponent<Light>().intensity = 0f;
    }

    IEnumerator FadeInLight()
    {

        // Turn up intensity to max over duration
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Light>().intensity = Mathf.Lerp(0f, maxIntensity, elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
