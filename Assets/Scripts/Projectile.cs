using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : EnemyProjectile
{
    public float speed;
    public int damage;
    public Rigidbody2D rb;

    public Transform target;
    public Vector2 bulletDir;
    public float totalLifespan = -1;
    float remainingLifespan;

    bool hit = false;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb.velocity = speed * bulletDir;

        remainingLifespan = totalLifespan;
    }

    public override void SetDirection(Vector2 direction)
    {
        //DO NOTHING
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        if (!hit)
        {
            if(hitInfo.tag == "Player")
            {
                hitInfo.GetComponent<HealthManager>().damage(damage,0.25f);
                Destroy(gameObject);
                hit = true;
            }
            else if(hitInfo.name == "Walls" || hitInfo.name == "Layout Walls")
            {
                Destroy(gameObject);
                hit = true;
            }
        }
    }

    virtual protected void Update()
    {
        if (totalLifespan != -1)
        {
            remainingLifespan -= Time.deltaTime;
            if (remainingLifespan <= 0)
            {
                remainingLifespan = 0;
                Destroy(gameObject);
                hit = true;
            }
            
            float remainingColor = 0.5f + 0.5f * (remainingLifespan / totalLifespan);
            sr.color = new Color(1f, 1f, 1f, remainingColor);
        }
    }
    
}
