using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] projectile;
    public int projectileIndex;
    public bool canShoot;
    [SerializeField]
    private float arrowCooldown;
    public float arrowCooldownMultiplier;
    private float currentTimer;
    private PlayerManager player;
    move playerMove;

    public Transform firePoint;
    public Transform shootPoint;
    public GameObject bullet;
    public float bulletSpeed = 50;
    Vector2 lookDirection;
    float lookAngle;

    bool fireIsUnlocked;
    bool iceIsUnlocked;
    bool poisonIsUnlocked;
    public Image fireImage;
    public Image iceImage;
    public Image poisonImage;

    public SpriteRenderer muzzleFlash;


    void Start()
   {
        fireIsUnlocked = false;
        iceIsUnlocked = false;
        poisonIsUnlocked = false;
        player = GetComponent<PlayerManager>();
        playerMove = GetComponent<move>();
        canShoot = true;
        currentTimer = 0;
        arrowCooldownMultiplier = 1;

        muzzleFlash.enabled = false;
   }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<move>().mode == "Dead" || GetComponent<move>().gameIsPaused)
        { 
            return; 
        }

        lookDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        lookDirection = lookDirection.normalized;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, lookAngle);

        
        if (playerMove.gameIsPaused == false && playerMove.isMelee == false && Input.GetMouseButtonDown(0) && canShoot)
        {
            GameObject instantiatedObject = Instantiate(projectile[0]);
            instantiatedObject.transform.position = shootPoint.transform.position;
            instantiatedObject.transform.rotation = Quaternion.Euler(0, 0, lookAngle);
            instantiatedObject.GetComponent<Rigidbody2D>().velocity = lookDirection * bulletSpeed;
            canShoot = false;
            GetComponent<move>().SetAnimation("Magic", 0.25f, true);
            StartCoroutine(MuzzleFlash());
        }
        if (!canShoot)
        {
            currentTimer += Time.deltaTime;

            if(currentTimer >= arrowCooldown * arrowCooldownMultiplier)
            {
                canShoot = true;
                currentTimer = 0;
            }
        }
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.enabled = false;
    }
}
