using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Sprite backGroundSprite;
    public Sprite bossSprite;
    public Sprite consilerySprite;
    public int targetPower;
    public string[] bossPhrases;
    public string[] consileryPhrases;

    public float[] gravityForce;
    public float[] pickupCreatePeriod;
    public GameObject[] pickupTable;
    public float[] pickupProbStage1;
    public float[] pickupProbStage2;
    public float[] pickupProbStage3;
    public float[] pickupProbStage4;
    public int[] bossBombNumber;
    public GameObject[] bossBomb;
    public int[] consileryHealthNumber;
    public GameObject[] ConsileryDrop;
    public float[] healthHelp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
