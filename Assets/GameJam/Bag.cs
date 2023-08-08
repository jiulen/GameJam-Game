using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] HealthManager healthManager;

    public Text ATKCounter, DEXCounter, VITCounter, StatPointsCounter, HPCounter, XPCounter, LVCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Stats counters
        ATKCounter.text = "" + playerStats.bonusATK;
        DEXCounter.text = "" + playerStats.bonusDEX;
        VITCounter.text = "" + playerStats.bonusVIT;
        StatPointsCounter.text = "" + playerStats.unusedStatPoints;

        HPCounter.text = "" + healthManager.getHealth() + " / " + healthManager.getMaxHealth();
        XPCounter.text = "" + playerStats.currentExperiencePoints;
        LVCounter.text = "" + playerStats.currentLevel;
    }
}
