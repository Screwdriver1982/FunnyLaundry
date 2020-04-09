using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyPickup))]
public class ChangeLifePickup : MonoBehaviour
{
    public int lifeBonus;
    DestroyPickup dstrPickup;

    GameManager gameManager;

    private void Start()
    {
        dstrPickup = gameObject.GetComponent<DestroyPickup>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddPickupInList(gameObject);


    }

    private void ApplyEffect()
    {
        gameManager.ChangeLife(lifeBonus);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Basket"))
        {
            ApplyEffect();
            dstrPickup.DeletePickup();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bottom"))
        {
            dstrPickup.DeletePickup();
        }
    }

}
