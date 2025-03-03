using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamageOnce : EnemyContactDamage
{
    [SerializeField] float dmgTime = -1;
    float dmgTimer = 0;
    public bool hit = false;
    
    void Update()
    {
        dmgTimer += Time.deltaTime;
        if (dmgTimer >= dmgTime && dmgTime != -1) //if dmgTime -1 then means will always dmg
        {
            hit = true;
        }
    }

    override protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !hit)
        {
            hit = true;

            if (!collision.gameObject.GetComponent<HealthManager>().isInvincible() && bloodPrefab != null)
            {
                Instantiate(bloodPrefab, collision.bounds.ClosestPoint(contactCollider.transform.position + (Vector3)contactCollider.offset), Quaternion.identity);
            }

            collision.gameObject.GetComponent<HealthManager>().damage(contactDamage, stunTime);
            
            if (enemyManager != null)
            {
                enemyManager.hitPlayer = true;
            }
        }
    }
}
