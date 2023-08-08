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
        ATKCounter.text = playerStats.bonusATK.ToString();
        DEXCounter.text = playerStats.bonusDEX.ToString();
        VITCounter.text = playerStats.bonusVIT.ToString();
        StatPointsCounter.text = playerStats.unusedStatPoints.ToString();

        HPCounter.text = healthManager.getHealth() + " / " + healthManager.getMaxHealth();
        XPCounter.text = playerStats.currentExperiencePoints.ToString();
        LVCounter.text = playerStats.currentLevel.ToString();
    }
}
