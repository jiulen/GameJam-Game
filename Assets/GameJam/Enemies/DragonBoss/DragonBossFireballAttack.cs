using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossFireballAttack : MonoBehaviour
{
    public float attackTime; //Time before dragon can do other attacks
    private float attackTimer = 0;
    private int attacking = 0; //0 for not started, 1 for animation stuff, 2 for raining, 3 for done
    public float delayTime; //Time before fireballs start raining down
    private float delayTimer = 0;
    public float fireballTime;
    private float fireballTimer = 0;
    public Transform fireballOrigin;

    private Transform playerTarget;

    public GameObject fireballPrefab;

    // Start is called before the first frame update
    void Start()
    {        
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking == 1)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayTime)
            {
                attacking = 2;
                fireballTimer = 0;

                StopAttack();
            }
        }
        else if (attacking == 2)
        {
            fireballTimer += Time.deltaTime;
            if (fireballTimer >= fireballTime)
            {
                Vector3 targetPos = playerTarget.position;

                //Shoot fireball
                Shoot(true, targetPos, 5);

                fireballTimer = 0;
            }
        }
    }



    public void StartAttack()
    {
        //Start attack animation (for now just shoot fireball)
        Shoot(false, Vector3.zero);

        attacking = 1;
        delayTimer = 0;
    }

    public void StopAttack()
    {        
        GetComponent<DragonBossManager>().attacking = false;
    }

    public void StopFireballs()
    {
        attacking = 3;
    }

    void Shoot(bool falling, Vector3 TargetPos, float time = 1)
    {
        GameObject fireball;
        FireballBehaviour behaviour;
        if (falling)
        {
            fireball = Instantiate(fireballPrefab, new Vector3(TargetPos.x, -250, TargetPos.z), Quaternion.identity); //hardcoded based on position of arena and camera
            behaviour = fireball.GetComponent<FireballBehaviour>();

            behaviour.falling = true;
            behaviour.targetPosY = TargetPos.y;
            behaviour.speed = Mathf.Abs(-250 - TargetPos.y) / time;
            behaviour.haveShadow = true;
        }
        else
        {
            fireball = Instantiate(fireballPrefab, fireballOrigin.position, Quaternion.identity);
            behaviour = fireball.GetComponent<FireballBehaviour>();
            
            behaviour.falling = false;
            behaviour.targetPosY = -250; //hardcoded based on position of arena and camera
            behaviour.speed = 50;
            behaviour.haveShadow = false;
        }
    }
}
