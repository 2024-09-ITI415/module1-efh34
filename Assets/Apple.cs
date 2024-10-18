using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Header("Set in Inspector")] // Added header for the inspector
    public static float bottomY = -20f; // Static variable to define the bottom limit

    void Update()
    {
        // Destroy the apple if it falls below the bottomY limit
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject); // Destroy this Apple object

            // **Get a reference to the ApplePicker script from the main Camera**
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();

            // **Call the public AppleDestroyed() method in ApplePicker**
            apScript.AppleDestroyed();
        }
    }
}
