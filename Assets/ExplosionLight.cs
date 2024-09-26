using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLight : MonoBehaviour
{

    private float fadeDuration = 1f;
    public float shortMaxIntensity = 1000f;
    public float longMaxIntensity = 10000f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener("explosion.flash.short", BeginShortFlash);
        EventManager.Instance.AddListener("explosion.flash.long", BeginLongFlash);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("explosion.flash.short", BeginShortFlash);
        EventManager.Instance.RemoveListener("explosion.flash.long", BeginLongFlash);
    }

    void BeginShortFlash(AudioClip clip, float duration)
    {
        StartCoroutine(FlashLightShort());
    }

    void BeginLongFlash(AudioClip clip, float duration)
    {
        StartCoroutine(FlashLightLong());
    }

    IEnumerator FlashLightLong()
    {
        GetComponent<Light>().intensity = longMaxIntensity;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Light>().intensity = Mathf.Lerp(longMaxIntensity, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FlashLightShort()
    {
        GetComponent<Light>().intensity = shortMaxIntensity;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Light>().intensity = Mathf.Lerp(shortMaxIntensity, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
