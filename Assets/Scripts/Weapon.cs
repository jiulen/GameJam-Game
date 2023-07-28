using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject projectile;
    public EnemyManager test;
    public Transform target;
    public int minRange;
    public float fireRate;
    private float timer = 0;
    float bulletSpeed = 10f;
    Animator enemyAnim;
    [SerializeField] int bulletNum;
    SpriteRenderer sr;
    EnemyControl enemyControl;

    float attackTimer = 0;
    [SerializeField] float attackTime;
   void Start()
   {
        enemyAnim = GetComponent<Animator>();
        test = GetComponent<EnemyManager>();
        target = test.target;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        enemyControl = GetComponent<EnemyControl>();
   }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) < minRange && fireRate <= timer && !test.stunned)
        {
            Shoot();
            timer = 0;
        }
        timer += Time.deltaTime;

        if (enemyControl.isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                enemyControl.isAttacking = false;
                attackTimer = 0;
            }
        }
    }

    void Shoot()
    {
        enemyControl.isAttacking = true;
        enemyAnim.Play("EnemyAttack", sr.sortingLayerID, 0);
        StartCoroutine(DelayShooting());
    }
    IEnumerator DelayShooting()
    {
        yield return new WaitForSeconds(0.7f);
        
        for (int i = 0; i < bulletNum; ++i)
        {
            Debug.Log("Bullet " + i);
            float randRotation = Random.Range(0f, 360f);

            GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.Euler(0, 0, randRotation));
            bullet.GetComponent<Projectile>().manager = test;

            float randScale = Random.Range(1f, 1.5f);
            bullet.transform.localScale = new Vector3(randScale, randScale, 1);

            Projectile bulletScript = bullet.GetComponent<Projectile>();

            float randSpeed = Random.Range(1f, 3f);
            bulletScript.speed = randSpeed;
            bulletScript.bulletDir = bullet.transform.right;

            yield return new WaitForSeconds(0.1f);
        }
    }
}

