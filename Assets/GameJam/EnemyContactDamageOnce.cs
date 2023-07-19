using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamageOnce : EnemyContactDamage
{
    bool hit = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !hit)
        {
            hit = true;
            collision.gameObject.GetComponent<HealthManager>().damage(contactDamage, stunTime);
        }
    }
}
