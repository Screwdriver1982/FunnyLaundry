using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{

    bool basketActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (basketActive)
        {
            Vector3 mousePos = Input.mousePosition; //координаты на экране
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos); // функция приводит к координатам мира
            float posX = Mathf.Clamp(mouseWorldPos.x, -1.62f, 1.62f);

            transform.position = new Vector3(posX, transform.position.y, 0);

        }
        
    }

    public void SetBasket(float posX)
    {
        transform.position = new Vector3(posX, transform.position.y, 0);
    }

    public void StopBasket()
    {
        basketActive = false;
    }
}
