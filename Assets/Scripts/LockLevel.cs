using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockLevel : MonoBehaviour
{
    public Image lockImg;
    public int levelNum;

    int playerMaxLvl;
    ScenesLoader scenesLoaderVar;
    Transporter transporterVar;
    
    
    // Start is called before the first frame update
    void Start()
    {
        scenesLoaderVar = FindObjectOfType<ScenesLoader>();
        transporterVar = FindObjectOfType<Transporter>();
        playerMaxLvl = PlayerPrefs.GetInt("PlayerMaxLvl", 0);
        if (levelNum > playerMaxLvl)
        {
            lockImg.gameObject.SetActive(true);
        }
        else
        {
            lockImg.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    public void clickOnLevel()
    {
        if (playerMaxLvl >= levelNum)
        {
            transporterVar.ChooseLevel(levelNum);
            scenesLoaderVar.LoadGameScene();

        }
    }
}
