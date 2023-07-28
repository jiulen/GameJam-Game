using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField]
    protected float stunTime = 0.25f;
    [SerializeField]
    protected int contactDamage = 4;

    virtual protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().damage(contactDamage, stunTime);
        }
    }
}
