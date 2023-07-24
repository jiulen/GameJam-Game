using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    public float maxSpeed;
    public float homingForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        rb.velocity = speed * transform.up;
    }

    // Update is called once per frame
    override protected void Update()
    {
        //Push missile towards player
        rb.AddForce(homingForce * (Vector2)(target.transform.position-transform.position).normalized);

        //Limit speed
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        //Rotate missile to face player
        if (rb.velocity.sqrMagnitude > 0)
        {
            Vector3 direction = rb.velocity.normalized;
            transform.right = direction;
        }
    }
}
