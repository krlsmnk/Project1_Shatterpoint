using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionLight : MonoBehaviour
{

    private float fadeDuration = 1f;
    public float maxIntensity = 1000f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener("explosion.flash", BeginShortFlash);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("explosion.flash", BeginShortFlash);
    }

    void BeginShortFlash(AudioClip clip, float duration)
    {
        StartCoroutine(FlashLight());
    }

    IEnumerator FlashLight()
    {
        GetComponent<Light>().intensity = maxIntensity;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Light>().intensity = Mathf.Lerp(maxIntensity, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
