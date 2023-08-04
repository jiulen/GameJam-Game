using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField]
    protected float stunTime = 0.25f;
    [SerializeField]
    protected int contactDamage = 4;
    public EnemyManager enemyManager;
    public GameObject bloodPrefab;
    public Collider2D contactCollider;
    public bool playerCenterBlood = false;

    virtual protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<HealthManager>().isInvincible() && bloodPrefab != null)
            {
                if (playerCenterBlood)
                {
                    Instantiate(bloodPrefab, collision.transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(bloodPrefab, collision.bounds.ClosestPoint(contactCollider.transform.position + (Vector3)contactCollider.offset), Quaternion.identity);
                }
            }

            collision.gameObject.GetComponent<HealthManager>().damage(contactDamage, stunTime);

            if (enemyManager != null)
            {
                enemyManager.hitPlayer = true;
            }
        }
    }
}
