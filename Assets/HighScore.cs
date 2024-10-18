using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For accessing UI elements

public class HighScore : MonoBehaviour
{
    // Static variable to hold the high score
    public static int score = 0; // Default score value

    // UI Text to display the high score
    private Text highScoreText;

    void Awake()
    {
        // If the PlayerPrefs HighScore already exists, retrieve it
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore"); // Retrieve the high score from PlayerPrefs
        }
    }

    void Start()
    {
        // Find the Text component for displaying high scores
        highScoreText = GetComponent<Text>();
        UpdateHighScoreText(); // Display the high score at start
    }

    void Update()
    {
        // Update the displayed high score text every frame
        UpdateHighScoreText();
    }

    // Method to update the displayed high score text
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0); // Display the high score
        }
    }
}
