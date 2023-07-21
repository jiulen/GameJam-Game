using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] Transform explosionTransform;
    [SerializeField] Vector3 explosionOffset;

    [SerializeField] float scaleTime;
    float scaleTimer = 0;
    [SerializeField] Vector3 minScale, maxScale;
    bool scaling = true;

    public Vector2 futureVelo;

    public ChickenManager chickenManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.localScale = minScale;
    }

    private void Update()
    {
        if (scaling)
        {
            scaleTimer += Time.deltaTime;

            if (scaleTimer >= scaleTime)
            {
                transform.localScale = maxScale;

                scaling = false;

                chickenManager.Launch();
                rb.velocity = futureVelo;
            }
            else
            {
                float percentage = scaleTimer / scaleTime;

                transform.localScale = (maxScale - minScale) * percentage + minScale;
            }
        }

        //Rotate egg to face player
        if (futureVelo.sqrMagnitude > 0)
        {
            Vector3 direction = futureVelo.normalized;
            transform.right = direction;
        }
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        if(hitInfo.tag == "Player" || hitInfo.name == "Walls" || hitInfo.name == "Layout Walls")
        {
            //Spawn explosion
            Instantiate(explosionPrefab, explosionTransform.position + explosionOffset, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
