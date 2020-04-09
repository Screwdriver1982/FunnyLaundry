﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DestroyPickup))]
public class MoneyPickup : MonoBehaviour
{
    public int moneyBonus;
    public int powerBonus;
    public int loseLifeBottom;
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
        gameManager.AddMoney(moneyBonus);
        gameManager.AddPower(powerBonus);
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
            gameManager.LosePickup(loseLifeBottom);
            dstrPickup.DeletePickup();
        }
    }


}
