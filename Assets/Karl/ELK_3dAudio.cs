using UnityEngine;
using System.Collections;

public class ELK_3dAudio : MonoBehaviour
{
    private void OnEnable()
    {
        EventManagerKarl.OnEventTriggered += EventHandler;  // Subscribe to the event
    }

    private void OnDisable()
    {
        EventManagerKarl.OnEventTriggered -= EventHandler;  // Unsubscribe from the event
    }

    // Event handler that gets triggered by EventManagerKarl
    public void EventHandler(string eventName)
    {
        switch (eventName)
        {
            case "CollisionWarning":
                transform.Find("CollisionWarning").GetComponent<AudioSource>().Play();
                StartCoroutine(PlayDelayedSound("SteelStraining", 0.8f));
                break;

            case "Impact1":
                transform.Find("Impact1").GetComponent<AudioSource>().Play();
                transform.Find("Impact1_Inside").GetComponent<AudioSource>().Play();
                break;

            case "EarsRinging":
                transform.Find("EarsRinging").GetComponent<AudioSource>().Play();
                transform.Find("Heartbeat").GetComponent<AudioSource>().Play();
                break;

            case "VisAudioFadeIn":
                transform.Find("Alarm2").GetComponent<AudioSource>().Play();
                transform.Find("Alarm4").GetComponent<AudioSource>().Play();
                break;

            case "SplashScreen":
                Destroy(gameObject);
                break;
            case "DepressurizationCabin":
                transform.Find("DepressurizationCabin").GetComponent<AudioSource>().Play();
                transform.Find("Impact2").GetComponent<AudioSource>().Play();
                break;
            case "Impact2":
                //doNothing, audiop already playing
                break;
            default:
                // Default behavior: find an AudioSource with the same name as the event and play it
                var audioSource = transform.Find(eventName)?.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning($"AudioSource not found for event '{eventName}'");
                }
                break;
        }
    }

    // Coroutine to play sound after a delay
    private IEnumerator PlayDelayedSound(string soundName, float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.Find(soundName).GetComponent<AudioSource>().Play();
    }
}

