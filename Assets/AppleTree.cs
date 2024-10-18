using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject applePrefab;

    public float speed = 1f;
    public float leftAndRightEdge = 20f;
    // Chance that the AppleTree will change directions
    public float chanceToChangeDirection = 0.2f;
    // Rate at which Apples will be instantiated
    public float secondsBetweenAppleDrop = 1.5f;

    void Start()
    {
        // Dropping apples every second
        Invoke("DropApple", 2f);
    }

    void DropApple()
    {
        GameObject apple = Instantiate(applePrefab); // Instantiate the apple prefab
        apple.transform.position = transform.position; // Set the position of the apple
        Invoke("DropApple", secondsBetweenAppleDrop); // Schedule the next apple drop
    }

    void Update()
    {
        // Basic Movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Changing Direction
        if (pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed); // Move right
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed); // Move left
        }

        // Randomly change direction
        if (Random.value < chanceToChangeDirection)
        {
            speed *= -1; // Change direction randomly
        }
    }
}

