using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] Transform explosionTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        if(hitInfo.tag == "Player" || hitInfo.name == "Walls" || hitInfo.name == "Layout Walls")
        {
            //Spawn explosion
            Instantiate(explosionPrefab, explosionTransform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
