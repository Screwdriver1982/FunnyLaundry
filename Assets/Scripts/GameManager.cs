using System.Collections;
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
    public int money;
    public Text moneyTxt;
    public float life;
    public float maxLife;
    public GameObject lifeBar;
    public float power;
    
    public GameObject powerBar;
    public List<GameObject> pickUpsList;
    public int losePickupLife;
    public float safeModeTime;
    public Vector3 pickupGenerator;
    public Text  loseFinalMoney;
    public Image loseGameWindow;
    public Text winFinalMoney;
    public Image winLevelWindow;
    


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

        levelPickups = levelVar.pickupTable;
        levelStage = 1;
                
        Invoke("ConsilerySay", 0.5f);
        Invoke("BossSay", 3.5f);
        StageDrop(1);

    }

    void ConsilerySay()
    {
        consileryImage.gameObject.SetActive(true);
        consileryText.text = levelVar.consileryPhrases[consPhraseI];
        Invoke("ConsileryOff", 3f);

    }

    void BossSay()
    {
        bossImage.gameObject.SetActive(true);
        bossText.text = levelVar.bossPhrases[bossPhraseI];
        Invoke("BossOff", 3f);

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
    }

    public void ChangeLife(float bonusLife)
    {
        life += bonusLife;
        lifeBar.transform.localScale = new Vector3(1, life / maxLife, 1);
        if (life <= 0)
        {
            LoseGame();
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

    public void LosePickup()
    {
        if (!safeMode)
        {
            ChangeLife(losePickupLife);
        }
        
    }

    public void SafeModeOn()
    {
        CancelInvoke("SafeModeOff");
        safeMode = true;
        Invoke("SafeModeOff", safeModeTime);
                
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
        winLevelWindow.gameObject.SetActive(true);

        ClearPickups();
        //Time.timeScale = 1;
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

        CancelInvoke("PickupDrop");
        InvokeRepeating("PickupDrop", delay, pickupCreatePeriod);

    }

    private void PickupDrop()
    {
        float dice = Random.Range(0f, 1f);

        Debug.Log(dice);

        float baseProbability = 0f;
        float directionAngel = Random.Range(Mathf.PI, Mathf.PI * 2);

        Debug.Log(directionAngel);

        for (int i = 0; i < stagePobability.Length; i++)
        {
            baseProbability += stagePobability[i];
            if (baseProbability >= dice)
            {
                chosenNumber = i;
                Debug.Log(baseProbability);
                break;
            }
        
        }

        GameObject newPickup = Instantiate(levelPickups[chosenNumber], pickupGenerator, Quaternion.identity);


        Rigidbody2D rb = newPickup.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(Mathf.Cos(directionAngel), Mathf.Sin(directionAngel), 0) * 0.5f;
        rb.gravityScale = Random.Range(0.5f,1.5f)*gravity;
        Debug.Log(rb.velocity);

    
    
    }

    public void NextLevel()
    {
        winLevelWindow.gameObject.SetActive(false);
        currentLevel += 1;
        LoadLevel(currentLevel, true);
    }


}
