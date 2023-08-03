using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectyle : MonoBehaviour //Player melee projectile
{
    protected float timeActive = 0;
    [SerializeField]
    protected float totalTime = 3; //Stay active for x seconds
    [SerializeField]
    protected int damage = 3;
    [SerializeField]
    protected float stuntime = 0.5f;
    protected Rigidbody2D rb;
    [SerializeField]
    protected int travelSpeed = 6;
    [SerializeField]
    protected bool destroyOnHit = false;
    [SerializeField]
    private bool isPlayerAttack;

    private PlayerStats stats;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioClip[] panWoosh;
    public AudioClip[] panHit;

    public GameObject bloodPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int panWooshIndex = Random.Range(0, panWoosh.Length);
        audioSource1.clip = panWoosh[panWooshIndex];
        audioSource1.Play();
        stats = FindObjectOfType<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();

        if (isPlayerAttack)
        {
            damage = stats.getMeleeDamage();
        }

        SetDirection();
    }

    public virtual void SetDirection() {
        Vector2 travelAngle = new Vector2(Mathf.Cos(transform.localEulerAngles.z * (Mathf.PI / 180)), Mathf.Sin(transform.localEulerAngles.z * (Mathf.PI / 180)));
        Debug.Log("Travel At : " + travelAngle + "\n" + (Mathf.Cos(transform.localEulerAngles.z * (Mathf.PI / 180))) + ", " + (Mathf.Sin(transform.localEulerAngles.z * (Mathf.PI / 180))) );
        rb.velocity = travelAngle * travelSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeActive >= totalTime) {
            Destroy(gameObject);
        }

        timeActive += Time.deltaTime;
    }    

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            //Enemy HP Down
            //Enemy Stun slight
            collision.GetComponent<EnemyManager>().Damage(damage , stuntime);
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
            int panHitIndex = Random.Range(0, panHit.Length);
            audioSource2.clip = panHit[panHitIndex];
            audioSource2.Play();
        }
        if (collision.gameObject.tag == "Chest") {
            collision.GetComponent<ChestManager>().Damage();
            if (destroyOnHit)
            {
                Destroy(gameObject);
                int panHitIndex = Random.Range(0, panHit.Length);
                audioSource2.clip = panHit[panHitIndex];
                audioSource2.Play();
            }
        }
        if (collision.name == "Walls") {
            if (destroyOnHit) {
                Destroy(gameObject);
            }
        }
        if (collision.name == "Totem")
        {
            collision.GetComponent<TotemController>().Damage(damage);
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
