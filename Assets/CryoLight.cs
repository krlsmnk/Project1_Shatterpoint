using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoLight : MonoBehaviour
{

    private float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener("cryo.light.fade", BeginFadeOut);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("cryo.light.fade", BeginFadeOut);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BeginFadeOut(AudioClip clip, float duration)
    {
        fadeDuration = duration;
        StartCoroutine(FadeOutCanvas());
    }

    IEnumerator FadeOutCanvas()
    {

        // Turn intensity slowly to zero over duration
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            GetComponent<Light>().intensity = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
