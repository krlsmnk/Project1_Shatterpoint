using UnityEngine;
using System.Collections;


public class ELK_Cryopod : MonoBehaviour
{
    public Transform cryopodDoor;  // Assign the CryopodDoor from the Inspector or find it in the code
    public float rotationDuration = 3.0f;  // Time to fully open the door
    public float targetRotationY = 90f;    // Degrees to rotate (adjust this as needed)

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
        if (eventName == "CrypodOpens")
        {
            //Debug.Log($"Cryopod Door Opening");

            // Start rotating the door slowly around the Y axis
            if (cryopodDoor != null)
            {
                StartCoroutine(RotateDoor());
            }
        }
    }

    private IEnumerator RotateDoor()
    {
        Quaternion startRotation = cryopodDoor.rotation;
        Quaternion endRotation = Quaternion.Euler(cryopodDoor.eulerAngles.x, cryopodDoor.eulerAngles.y + targetRotationY, cryopodDoor.eulerAngles.z);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            cryopodDoor.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the target
        cryopodDoor.rotation = endRotation;
    }
}
