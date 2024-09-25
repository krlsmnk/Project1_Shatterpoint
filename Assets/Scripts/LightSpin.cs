using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpin : MonoBehaviour
{
    // Speed of the rotation (can be modified in the Unity inspector)
    public float rotationSpeed = 50f;

    // Reference to the specific child that you want to rotate
    public Transform targetChild;

    // Start is called before the first frame update
    void Start()
    {

        // If the child isn't found, show a warning
        if (targetChild == null)
        {
            Debug.LogWarning("Child with the specified name not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the specific child if it exists
        if (targetChild != null)
        {
            targetChild.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}

