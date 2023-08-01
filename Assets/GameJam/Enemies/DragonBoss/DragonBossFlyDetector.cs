using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossFlyDetector : MonoBehaviour
{
    public DragonBossFlyAttack dragonBossFlyAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Enemy") && dragonBossFlyAttack != null)
        {
            if (dragonBossFlyAttack.flyState == DragonBossFlyAttack.FLYSTATE.START)
            {
                dragonBossFlyAttack.flyState = DragonBossFlyAttack.FLYSTATE.STOP;
            }
        }
    }
}
