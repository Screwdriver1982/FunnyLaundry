using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DestroyPickup))]
public class MoneyPickup : MonoBehaviour
{
    public int moneyBonus;
    public int powerBonus;
    public int loseLifeBottom;
    public AudioClip pickupSound;
    public AudioClip bottomSound;
    public GameObject basketDestroyFX;
    public GameObject bottomDestroyFX;

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
            gameManager.PlaySound(pickupSound);
            if (basketDestroyFX != null)
            {
                Vector3 fxPosition = transform.position;

                //запомнили созданный объект, чтобы потом его прибить
                GameObject newObject = Instantiate(basketDestroyFX, fxPosition, Quaternion.identity);

                //уничтожить через N секунд
                Destroy(newObject, 5f);
            }


            dstrPickup.DeletePickup();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bottom"))
        {
            gameManager.LosePickup(loseLifeBottom);
            
            if (bottomSound != null)
            {
                gameManager.PlaySound(bottomSound);
            }

            if (bottomDestroyFX != null)
            {
                Vector3 fxPosition = transform.position;

                //запомнили созданный объект, чтобы потом его прибить
                GameObject newObject = Instantiate(bottomDestroyFX, fxPosition, Quaternion.identity);

                //уничтожить через N секунд
                Destroy(newObject, 5f);
            }


            dstrPickup.DeletePickup();



        }
    }


}
