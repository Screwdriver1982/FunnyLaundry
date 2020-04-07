using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    public int moneyBonus;
    public int powerBonus;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddPickupInList(gameObject);

    }

    private void ApplyEffect()
    {
        gameManager.AddMoney(moneyBonus);
        gameManager.AddPower(powerBonus);
    }
    public void DestroyPickup()
    {
        gameManager.RemovePickupFromList(gameObject);
        Destroy(gameObject);
        gameObject.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Basket"))
        {
            ApplyEffect();
            DestroyPickup();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bottom"))
        {
            gameManager.LosePickup();
            DestroyPickup();
        }
    }


}
