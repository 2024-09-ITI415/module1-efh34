using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    // Singleton instance
    static private Slingshot S;

    // Fields set in the Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile; // Prefab for the projectile
    public float velocityMult = 8f; // Multiplier for projectile velocity

    // Fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint; // Reference to the launch point
    public Vector3 launchPos; // Position where the projectile will launch from
    public GameObject projectile; // Reference to the instantiated projectile
    public bool aimingMode; // Indicates if the player is aiming
    private Rigidbody projectileRigidbody; // Rigidbody of the instantiated projectile

    // Static property to get the launch position
    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    private void Awake()
    {
        S = this; // Set the singleton instance
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // Hide the launch point initially
        launchPos = launchPointTrans.position; // Set the launch position
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true); // Show the launch point when the mouse enters
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false); // Hide the launch point when the mouse exits
    }

    void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true; // Set aiming mode to true

        // Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject; // Create the projectile
        projectile.transform.position = launchPos; // Start it at the launch point

        // Get the Rigidbody component and set it to isKinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true; // Make the projectile kinematic
    }

    void Update()
    {
        // If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode)
            return; // Exit if not aiming

        // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition; // Get mouse position in 2D
        mousePos2D.z = -Camera.main.transform.position.z; // Set the z-coordinate to the camera's position

        // Convert the 2D mouse position to a 3D position
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        // Limit mouseDelta to the radius of the Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius; // Get the radius of the SphereCollider
        if (mouseDelta.magnitude > maxMagnitude)
        { // If mouseDelta exceeds the max radius
            mouseDelta.Normalize(); // Normalize the vector to keep it within the radius
            mouseDelta *= maxMagnitude;
        }
        Vector3 proPos = launchPos + mouseDelta;
        projectile.transform.position = proPos;

        // Check if the mouse button has been released
        if (Input.GetMouseButtonUp(0))
        {
            // The mouse has been released
            aimingMode = false; // Exit aiming mode
            projectileRigidbody.isKinematic = false; // Make the projectile dynamic
            projectileRigidbody.velocity = -mouseDelta * velocityMult; // Set projectile velocity

            FollowCam.POI = projectile; // Set the FollowCam's point of interest to the projectile
            MissionDemolition.ShotFired(); // Call the ShotFired method from MissionDemolition
            ProjectileLine.S.poi = projectile; // Set the point of interest for ProjectileLine
            projectile = null; // Clear the reference to the projectile
        }
    }
}
