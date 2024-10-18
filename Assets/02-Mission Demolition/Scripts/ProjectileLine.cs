using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    // Singleton instance
    static public ProjectileLine S;

    // Fields set in Inspector
    [Header("Set in Inspector")]
    public float minDist = 0.1f; // Minimum distance between points
    private LineRenderer line; // LineRenderer component
    private GameObject _poi; // Point of interest
    public List<Vector3> points; // List of points for the line

    void Awake()
    {
        S = this; // Set the singleton
        line = GetComponent<LineRenderer>();
        line.enabled = false; // Disable the LineRenderer until it's needed
        points = new List<Vector3>(); // Initialize the points List
    }

    // Property for getting and setting the point of interest (poi)
    public GameObject poi
    {
        get { return _poi; }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false; // Reset everything when poi is set
                points = new List<Vector3>();
                AddPoint(); // Add the first point
            }
        }
    }

    // This can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    // Add a point to the line
    public void AddPoint()
    {
        // If no point of interest (poi) exists, return
        if (_poi == null) return;

        Vector3 pt = _poi.transform.position;

        // If the point is not far enough from the last point, return
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) return;

        // Add points to the line
        if (points.Count == 0)
        {
            // If it's the launch point, add an extra bit of line for aiming
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Assumes Slingshot.LAUNCH_POS is defined elsewhere
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // Enables the LineRenderer
            line.enabled = true;
        }
        else
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint); // Update the last point on the line
            line.enabled = true; // Enable the LineRenderer
        }
    }

    // Returns the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null || points.Count == 0)
                return Vector3.zero; // If there are no points, return Vector3.zero
            return points[points.Count - 1];
        }
    }

    void FixedUpdate()
    {
        // If no point of interest exists, look for one
        if (_poi == null)
        {
            if (FollowCam.POI != null && FollowCam.POI.tag == "Projectile")
            {
                poi = FollowCam.POI; // Assign the projectile as the poi
            }
            else
            {
                return; // If no poi, return
            }
        }

        // Add the current position of the poi every FixedUpdate
        AddPoint();

        // Once the poi (projectile) is no longer active, clear the reference
        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }
}
