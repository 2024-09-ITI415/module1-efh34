using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f; // Easing factor for smooth camera movement
    public Vector2 minXY = Vector2.zero; // Minimum X and Y values for camera position

    [Header("Set Dynamically")]
    public float camZ; // The desired Z position of the camera

    void Awake()
    {
        camZ = this.transform.position.z; // Set camZ to the current camera's Z position
    }

    void FixedUpdate()
    {
        Vector3 destination; // Declare destination variable

        // If there is no point of interest, return to the default position
        if (POI == null)
        {
            destination = Vector3.zero; // Default position
        }
        else
        {
            // Get the position of the point of interest
            destination = POI.transform.position;

            // If POI is a Projectile, check to see if it's at rest
            if (POI.tag == "Projectile")
            {
                // If it is sleeping (that is, not moving), set POI to null
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null; // Clear POI if the projectile is at rest
                }
            }
        }

        // Limit the X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x); // Enforce minimum X value
        destination.y = Mathf.Max(minXY.y, destination.y); // Enforce minimum Y value

        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing); // Smoothly move towards the destination

        // Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;

        // Set the camera to the destination
        transform.position = destination;

        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10; // Adjust orthographic size based on destination.y
    }
}
