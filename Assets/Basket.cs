using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // This line enables access to UI elements

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text scoreGT; // Reference to the score UI text

    void Start()
    {
        // Find a reference to the ScoreCounter
        GameObject scoreGO = GameObject.Find("ScoreCounter"); // Ensure this GameObject name matches your hierarchy
        // Get the Text Component of that GameObject
        scoreGT = scoreGO.GetComponent<Text>();
        // Set the starting number of points to 0
        scoreGT.text = "0";
    }

    void Update()
    {
        // Get the current screen position of the mouse
        Vector3 mousePos2D = Input.mousePosition;

        // The Camera's z position sets how far the basket is from the camera
        mousePos2D.z = -Camera.main.transform.position.z; // Set the z position to the camera's z

        // Convert the point from 2D screen space to 3D world space
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Move the x position of this Basket to the mouse's x position
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x; // Update only the x position
        this.transform.position = pos; // Set the new position of the basket
    }

    void OnCollisionEnter(Collision coll)
    {
        // Find out what hit this basket
        GameObject collidedWith = coll.gameObject;

        // Check if the collided object has the tag "Apple"
        if (collidedWith.CompareTag("Apple"))
        {
            Destroy(collidedWith); // Destroy the apple object

            // Parse the text of the scoreGT into an integer
            int score = int.Parse(scoreGT.text);
            // Add points for catching the apple
            score += 100;
            // Convert the score back to a string and update the UI
            scoreGT.text = score.ToString();

            // Track the high score
            if (score > HighScore.score)
            {
                HighScore.score = score; // Update the high score
                PlayerPrefs.SetInt("HighScore", HighScore.score); // Save the high score to PlayerPrefs
            }
        }
    }
}
