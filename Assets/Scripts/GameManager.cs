﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Basket basketVar;
    public SpriteRenderer backGroundSprite;
    public Image bossImage;
    public Image consileryImage;
    public Text bossText;
    public Text consileryText;
    public Level[] levels;
    public float startX;
    public Text moneyTxt;
    public float maxLife;
    public GameObject lifeBar;
    public int alarmMaxClock;
    public float alarmBlinkTime;
    public int winGold;
    public Color alarmColor;
    public Color baseMoneyColor;
    public Color baseBorderColor;
    public Color alarmBorderColor;
    public SpriteRenderer rightBorder;
    public SpriteRenderer leftBorder;
    public SpriteRenderer bottomBorder;

    public GameObject powerBar;
    public List<GameObject> pickUpsList;
    public float safeModeTime;
    public Vector3 pickupGenerator;
    public Vector3 bossGenerator;
    public Vector3 consileryGenerator;
    public Text  loseFinalMoney;
    public Image loseGameWindow;
    public Image goldImg;
    public Image winLevelWindow;
    public Text winFinalMoney;
    public Text winFinalGold;

    int money;
    int gold;
    float life;
    float power;
    float maxPower;
    int currentLevel;
    int consPhraseI;
    int bossPhraseI;
    bool safeMode = false;
    Level levelVar;
    int levelStage;
    GameObject[] levelPickups;
    float[] stagePobability;
    float gravity;
    float pickupCreatePeriod;
    float delay;
    int chosenNumber;
    bool pauseActive = false;
    bool alarmOn = false;
    int alarmClock;
    int consileryStage = 0;





    // Start is called before the first frame update

    void Start()
    {

        // грузим левел и устанавливаем жизнь в максимум
        LoadLevel(currentLevel, true);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseActive)
            {
                //выключить паузу
                Time.timeScale = 1;
                pauseActive = false;
                if (alarmOn)
                {
                    Alarm();
                }
            }
            else
            {
                //включить паузу
                Time.timeScale = 0;
                pauseActive = true;
            }

        }
    }

    void LoadLevel(int levelIndex, bool setMaxLife)
    {
        currentLevel = levelIndex;
        levelVar = levels[levelIndex];
        basketVar.ActiveBasket();
        backGroundSprite.sprite = levelVar.backGroundSprite;
        bossImage.sprite = levelVar.bossSprite;
        consileryImage.sprite = levelVar.consilerySprite;

        // обнуляем силу
        maxPower = levelVar.targetPower;
        AddPower(-power);
        //ставим жизнь в максимум или сохраняем
        if (setMaxLife)
        {
            ChangeLife(maxLife - life);
        }

        //зануляем деньги
        money = 0;

        //выключаем аларм если он был
        alarmOn = false;

        levelPickups = levelVar.pickupTable;
        levelStage = 1;
                
        Invoke(nameof(ConsilerySay), 0.5f);
        Invoke(nameof(BossSay), 3.5f);
        StageDrop(1);

    }

    void ConsilerySay()
    {
        consileryImage.gameObject.SetActive(true);
        consileryText.text = levelVar.consileryPhrases[consPhraseI];
        Invoke(nameof(ConsileryOff), 3f);

    }

    void BossSay()
    {
        bossImage.gameObject.SetActive(true);
        bossText.text = levelVar.bossPhrases[bossPhraseI];
        Invoke(nameof(BossOff), 3f);

    }


    void ConsileryOff()
    {
        consileryImage.gameObject.SetActive(false);
    }

    void BossOff()
    {
        bossImage.gameObject.SetActive(false);
    }

    public void AddMoney(int bonusMoney)
    {
        money += bonusMoney;
        moneyTxt.text = "" + money;

        if (money < 0 && !alarmOn)
        {
            alarmOn = true;
            Alarm();
        }
        else if(money >=0 && alarmOn)
        {
            alarmOn = false;
        }

    }

    public void ChangeLife(float bonusLife)
    {
        life += bonusLife;
        lifeBar.transform.localScale = new Vector3(1, life / maxLife, 1);

        if (life <= 0)
        {
            LoseGame();
        }

        if (levelVar.healthHelp.Length > consileryStage)
        {
            if (life / maxLife <= levelVar.healthHelp[consileryStage])
            {
                ConsileryHealthDrop(consileryStage);
                consileryStage += 1;
            }

        }
        
        
    }

    

    public void AddPower(float bonusPower)
    {
        power += bonusPower;
        powerBar.transform.localScale = new Vector3(1, power / maxPower, 1);

        if (power >= maxPower)
        {
            WinLevel();
        }
        else if (power >= 0.6 * maxPower && levelStage < 4)
        {
            SetBossStage(4);
        }
        else if (power >= 0.4 * maxPower && levelStage < 3)
        {
            SetBossStage(3);
        }
        else if (power >= 0.2 * maxPower && levelStage < 2)
        {
            SetBossStage(2);
        }
    }


    public void AddPickupInList(GameObject pickup)
    {
        pickUpsList.Add(pickup);
    }

    public void RemovePickupFromList(GameObject pickup)
    {
        pickUpsList.Remove(pickup);
    }

    public void LosePickup(int losePickupLife)
    {
        if (!safeMode)
        {
            ChangeLife(losePickupLife);
        }
        
    }

    public void SafeModeOn()
    {
        CancelInvoke(nameof(SafeModeOff));
        safeMode = true;
        Invoke(nameof(SafeModeOff), safeModeTime);
                
    }

    public void SafeModeOff()
    {
        safeMode = false;
    }

    private void LoseGame()
    {
        Time.timeScale = 0;
        CancelInvoke();
        basketVar.StopBasket();
        loseFinalMoney.text = "" + money;
        ExchangeMoney(money, 0);
        loseGameWindow.gameObject.SetActive(true);

        ClearPickups();
        Time.timeScale = 1;

    }

    private void WinLevel()
    {
        Time.timeScale = 0;
        CancelInvoke();
        basketVar.StopBasket();
        winFinalMoney.text = "" + money;
        ExchangeMoney(money, 1);
        
        winLevelWindow.gameObject.SetActive(true);

        ClearPickups();
        Time.timeScale = 1;
    }

    private void ExchangeMoney(int money, int winOrLose)
    {
        gold = winOrLose * winGold;

        if (money >= 0)
        {
            gold += Mathf.FloorToInt(money / 100);
        }

        winFinalGold.text = "" + gold;
        goldImg.gameObject.SetActive(true);

        int playerGold = PlayerPrefs.GetInt("PlayerGold",0);
        playerGold += gold;
        PlayerPrefs.SetInt("PlayerGold", playerGold);

    }

    private void ClearPickups()
    {
        while (pickUpsList.Count>0)
        {
            pickUpsList[0].GetComponent<DestroyPickup>().DeletePickup();

        }
    }

    private void SetBossStage(int stage)
    {
        levelStage = stage;
        bossPhraseI = stage-1;
        BossSay();
        StageDrop(stage);

        BossBombDrop(stage-1);

    }

    private void StageDrop(int stage)
    {
        float wheightSum = 0f;
        

        if (stage == 1)
        {
            stagePobability = levelVar.pickupProbStage1;
            gravity = levelVar.gravityForce[0];
            pickupCreatePeriod = levelVar.pickupCreatePeriod[0];
            delay = 4f;
        }
        else if(stage == 2)
        {
            stagePobability = levelVar.pickupProbStage2;
            gravity = levelVar.gravityForce[1];
            pickupCreatePeriod = levelVar.pickupCreatePeriod[1];
            delay = 0.5f;
        }
        else if (stage == 3)
        {
            stagePobability = levelVar.pickupProbStage3;
            gravity = levelVar.gravityForce[2];
            pickupCreatePeriod = levelVar.pickupCreatePeriod[2];
            delay = 0.5f;
        }
        else if (stage == 4)
        {
            stagePobability = levelVar.pickupProbStage4;
            gravity = levelVar.gravityForce[3];
            pickupCreatePeriod = levelVar.pickupCreatePeriod[3];
            delay = 0.5f;
        }

        for (int i = 0; i < stagePobability.Length; i++)
        {
            wheightSum = wheightSum + stagePobability[i];
        }

        for (int j = 0; j < stagePobability.Length; j++)
        {
            stagePobability[j] = stagePobability[j] / wheightSum;
        }

        CancelInvoke(nameof(PickupDrop));
        InvokeRepeating(nameof(PickupDrop), delay, pickupCreatePeriod);

    }

    private void PickupDrop()
    {
        float dice = Random.value;

        float baseProbability = 0f;
        float directionAngel = Random.Range(Mathf.PI, Mathf.PI * 2);

       

        for (int i = 0; i < stagePobability.Length; i++)
        {
            baseProbability += stagePobability[i];
            if (baseProbability >= dice)
            {
                chosenNumber = i;
                break;
            }
        
        }

        GameObject newPickup = Instantiate(levelPickups[chosenNumber], pickupGenerator, Quaternion.identity);


        Rigidbody2D rb = newPickup.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(Mathf.Cos(directionAngel), Mathf.Sin(directionAngel), 0) * 0.5f;
        rb.gravityScale = Random.Range(0.5f,1.5f)*gravity;
     
    }


    private void BossBombDrop(int stage)
    {
        for (int i = 0; i < levelVar.bossBombNumber[stage]; i++)
        {
            int dropNumber = Random.Range(0,levelVar.bossBomb.Length-1);
            float directionAngel = Random.Range(Mathf.PI, 7 * Mathf.PI / 6);
            GameObject newBomb = Instantiate(levelVar.bossBomb[dropNumber], bossGenerator, Quaternion.identity);
            Rigidbody2D rb = newBomb.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(Mathf.Cos(directionAngel), Mathf.Sin(directionAngel), 0)*2;
            
        }
        
    }

    private void ConsileryHealthDrop(int helpNum)
    {
        for (int i = 0; i < levelVar.consileryHealthNumber[helpNum]; i++)
        {
            int dropNumber = Random.Range(0, levelVar.ConsileryDrop.Length - 1);
            float directionAngel = Random.Range(7 * Mathf.PI / 6, 2 * Mathf.PI);
            GameObject newDrop = Instantiate(levelVar.ConsileryDrop[dropNumber], consileryGenerator, Quaternion.identity);
            Rigidbody2D rb = newDrop.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(Mathf.Cos(directionAngel), Mathf.Sin(directionAngel), 0) * 2;

        }

    }



    public void NextLevel()
    {
        winLevelWindow.gameObject.SetActive(false);
        loseGameWindow.gameObject.SetActive(false);
        goldImg.gameObject.SetActive(false);
        currentLevel += 1;
        LoadLevel(currentLevel, true);
    }

    public void Alarm()
    {

        if (alarmOn)
        {
            if (!pauseActive)
            {
                
                alarmClock += 1;

                if (moneyTxt.color == alarmColor)
                {
                    moneyTxt.color = baseMoneyColor;
                    rightBorder.color = baseBorderColor;
                    leftBorder.color = baseBorderColor;
                    bottomBorder.color = baseBorderColor;
                }
                else
                {
                    moneyTxt.color = alarmColor;
                    rightBorder.color = alarmBorderColor;
                    leftBorder.color = alarmBorderColor;
                    bottomBorder.color = alarmBorderColor;
                }
                

                if (alarmClock >= alarmMaxClock)
                {
                    LoseGame();
                }
                else
                {
                    Invoke(nameof(Alarm), alarmBlinkTime);
                }
            }
            else
            {
                CancelInvoke(nameof(Alarm));
            }

        }
        else
        {
            alarmClock = 0;
            moneyTxt.color = baseMoneyColor;
        }
        
    }
}
