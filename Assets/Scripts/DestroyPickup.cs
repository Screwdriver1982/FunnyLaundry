using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPickup : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void DeletePickup()
    {
        gameManager.RemovePickupFromList(gameObject);
        Destroy(gameObject);
    }
}
