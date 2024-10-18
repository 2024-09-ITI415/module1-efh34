using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab; // Prefab for the basket
    public int numBaskets = 3; // Number of baskets to instantiate
    public float basketBottomY = -14f; // Bottom position of the baskets
    public float basketSpacingY = 2f; // Space between the baskets
    public List<GameObject> basketList; // List to store basket GameObjects

    void Start()
    {
        basketList = new List<GameObject>(); // Initialize the basket list
        for (int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate(basketPrefab); // Instantiate a new basket
            Vector3 pos = Vector3.zero; // Initialize position
            pos.y = basketBottomY + (basketSpacingY * i); // Calculate basket position
            tBasketGO.transform.position = pos; // Set the position of the basket

            basketList.Add(tBasketGO); // Add the basket to the list
        }
    }

    // Method to destroy all of the falling apples
    public void AppleDestroyed()
    {
        // Find all GameObjects tagged as "Apple"
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tGO in tAppleArray)
        {
            Destroy(tGO); // Destroy each apple GameObject
        }
        DestroyBasket();
    }

    // Method to destroy one of the baskets
    public void DestroyBasket()
    {
        // Get the index of the last Basket in the list
        int basketIndex = basketList.Count - 1;

        // Check if there are any baskets left to destroy
        if (basketIndex >= 0)
        {
            // Get a reference to that Basket GameObject
            GameObject tBasketGO = basketList[basketIndex];

            // Remove the Basket from the list and destroy it
            basketList.RemoveAt(basketIndex);
            Destroy(tBasketGO);

            // If there are no Baskets left, restart the scene
            if (basketList.Count == 0)
            {
                SceneManager.LoadScene("Main-ApplePicker"); // Load the ApplePicker scene
            }
        }
        else
        {
            Debug.LogWarning("No baskets left to destroy!");
        }
    }
}
