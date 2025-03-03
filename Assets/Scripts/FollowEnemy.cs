﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 5f;
    public float fasterSpeed = 5f;
    public float normalSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;


    private Animator anim;                                  //Modified      Add an "Attacking" parameter to the enemy animator
    public float attackDistance;                            //Modified
    private bool attacking;                                 //Modified

    [SerializeField] float attackCooldown;
    float cooldownTimer = 0;
    [SerializeField] float attackTime;
    float attackTimer = 0;
    Vector2 chargeVelo;
    EnemyManager enemyManager;

    bool readying = false;
    [SerializeField] float readyTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        rb = this.GetComponent<Rigidbody2D>();
        player = FindObjectOfType<HealthManager>().gameObject.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        movement = direction.normalized;

        if (!attacking)
        {
            cooldownTimer += Time.deltaTime;

            if(Vector2.Distance(transform.position, player.position) <= attackDistance && cooldownTimer >= attackCooldown && !readying)
            {
                readying = true;
                moveSpeed = normalSpeed * 0.25f;
                anim.speed = 0.25f;
            }

            if (readying && cooldownTimer >= attackCooldown + readyTime)
            {
                readying = false;
                attacking = true;
                attackTimer = 0;
                anim.SetBool("Attacking", true);
                anim.speed = 2;
                moveSpeed = fasterSpeed;
                chargeVelo = movement * moveSpeed;
            }
        }
        else
        {
            //attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime) //attack stops when boar hit wall
            {
                attacking = false;
                cooldownTimer = 0;
                anim.SetBool("Attacking", false);
                anim.speed = 1;
                moveSpeed = normalSpeed;
            }
        }
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 dir)
    {
        //Modified code below
        if (GetComponent<EnemyManager>().stunned)
        {
            rb.velocity = Vector2.zero;
        }
        else if(!attacking)
        {
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else {
                transform.localScale = new Vector3(1, 1, 1);
            }
            rb.velocity = dir * moveSpeed * enemyManager.slowMultiplier;
        }
        else
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else {
                transform.localScale = new Vector3(1, 1, 1);
            }
            rb.velocity = chargeVelo * enemyManager.slowMultiplier;
        }              
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Walls" || collision.collider.name == "Layout Walls" || collision.collider.tag == "Door")
        {
            if (attacking)
            {
                attackTimer = attackTime;
                enemyManager.Damage(3, 0.25f);
                Debug.Log("Collided1 : " + collision.collider.name);
            }
        }
    }
}
