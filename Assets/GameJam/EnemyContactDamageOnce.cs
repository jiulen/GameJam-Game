using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamageOnce : EnemyContactDamage
{
    [SerializeField] float dmgTime = -1;
    float dmgTimer = 0;
    bool hit = false;
    
    void Update()
    {
        dmgTimer += Time.deltaTime;
        if (dmgTimer >= dmgTime && dmgTime != -1) //if dmgTime -1 then means will always dmg
        {
            hit = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !hit)
        {
            hit = true;
            collision.gameObject.GetComponent<HealthManager>().damage(contactDamage, stunTime);
        }
    }
}
