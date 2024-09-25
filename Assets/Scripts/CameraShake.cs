using UnityEngine;
using System.Collections;
using UnityEngine.XR;

public class VRShake : MonoBehaviour
{
    public float shakeMagnitude = 0.2f;
    private Vector3 originalPos;
    private Coroutine shakeCoroutine;

    private float shakeDuration = 0.5f;


    // Reference to the XR Origin (self)
    private Transform xrOrigin = null;

    private void OnEnable()
    {
        EventManager.Instance.AddListener("camera.shake", ShakeVR);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener("camera.shake", ShakeVR);
    }

    void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = transform; // Assume this script is attached to the XR Origin
        }
        originalPos = xrOrigin.position;
    }

    public void ShakeVR(AudioClip clip, float duration)
    {

        shakeDuration = duration;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake());
    }

    //added overload method that doesn't require unused clip - Karl
    public void ShakeVR(float duration)
    {

        shakeDuration = duration;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            xrOrigin.position = originalPos + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }
        xrOrigin.position = originalPos;
        shakeCoroutine = null;
    }

    // For testing
    void Update()
    {
    }
}