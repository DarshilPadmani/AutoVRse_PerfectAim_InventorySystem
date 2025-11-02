using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float velocityMultiplier = 1000f;  // Adjust to control the throw force
    public float trackingTimeWindow = 0.15f;  // Time window to track hand movement (in seconds)
    private List<Vector3> trackingPositions = new List<Vector3>();  // Positions tracked to calculate throw direction

    private bool pickedUp = false;  // Whether the object is currently held
    private GameObject parentHand;  // Reference to the hand holding the object
    private Rigidbody rb;  // Reference to the object's Rigidbody

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Initialize Rigidbody
    }

    private void Update()
    {
        if (pickedUp)
        {
            // Disable gravity and kinematic mode when holding the object
            rb.useGravity = false;
            rb.isKinematic = true;

            // Align the object with the hand's position and rotation
            transform.position = parentHand.transform.position;
            transform.rotation = parentHand.transform.rotation;

            // Track hand positions over time to calculate the throw direction
            if (trackingPositions.Count > 0 && Time.time - trackingPositions[0].z > trackingTimeWindow)
            {
                trackingPositions.RemoveAt(0);  // Remove old positions outside the tracking window
            }

            trackingPositions.Add(new Vector3(transform.position.x, transform.position.y, Time.time));  // Add current hand position with timestamp

            // Use OVRInput.Get to check if the trigger is pressed deeply enough
            if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) < 0.1f)  // Release the object when the trigger is below a threshold (near 0)
            {
                ThrowObject();  // Call the function to throw the object
            }
        }
    }

    // Function to throw the object with proper physics
    private void ThrowObject()
    {
        pickedUp = false;
        rb.isKinematic = false;  // Enable physics simulation
        rb.useGravity = true;  // Enable gravity

        if (trackingPositions.Count > 1)
        {
            // Calculate the throw direction using the first and last tracked positions
            Vector3 direction = (trackingPositions[trackingPositions.Count - 1] - trackingPositions[0]).normalized;

            // Apply a force to the object in the calculated direction
            rb.AddForce(direction * velocityMultiplier, ForceMode.VelocityChange);
        }

        // Clear tracking positions after the object is thrown
        trackingPositions.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the object is picked up by a hand
        if (other.CompareTag("hand") && OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.9f)  // Trigger deeply pressed to pick up
        {
            pickedUp = true;
            parentHand = other.gameObject;  // Reference to the hand picking up the object
        }
    }
}
