﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class move : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float speed = 6.5f; 
    [SerializeField]
    private float rollSpeed = 24.0f;
    [SerializeField]
    private float dodgeSpeed = 24.0f;
    private Animator animator;
    private SpriteRenderer sprite;
    private string direction = "Front";
    public string mode = "Idle";
    public float setTime = 0.0f; //Cooldown
    private bool rollStart = false;

    private float x = 0, y = 0;

    private Vector2 rollVector =  new Vector2(0, 0);

    public GameObject frontHitbox, sideHitbox, backHitbox;
    private GameObject hitbox;
    private PlayerStats stats;

    public GameObject bagUI;
    private bool bagIsOpen = false;
    [SerializeField]
    private GameObject statsPage, cookingPage;
    [SerializeField]
    Text statsToggleButtonText;
    private bool statsOpen = true; //if false then cooking open
    public GameObject pausedUI;
    public bool gameIsPaused;

    public Text boarTrotterText;
    public int boarTrotterCount;
    public Text shroomText;
    public int shroomCount;
    public Text broccoliText;
    public int broccoliCount;
    public Text chickenFeetText;
    public int chickenFeetCount;
    public Text rabbitLegText;
    public int rabbitLegCount;
    public Text shellText;
    public int shellCount;
    public Text wingText;
    public int wingCount;
    public Text yolkText;
    public int yolkCount;

    public bool isMelee;
    public GameObject fryingPan;
    public GameObject kitchenGun;

    private GameObject weaponArea;
    public Sprite gunSprite;
    public Sprite upgradeBuffSprite;
    public Sprite panSprite;
    public Sprite noEleSprite;
    public Sprite poisonGunSprite;
    public Sprite freezeGunSprite;
    public Sprite fireGunSprite;
    public Image weapon;
    public Image weaponBuff;
    private Text weaponName;
    public PlayerTest playerTest;
    public ElementalEffectChangeButton fireProjectileHolder;
    public ElementalEffectChangeButton iceProjectileHolder;
    public ElementalEffectChangeButton poisonProjectileHolder;


    public string Direction {
        get { return direction; }
    }

    public string Mode {
        get { return mode; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameIsPaused = false;
        isMelee = true;
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        weaponArea = GameObject.Find("WeaponArea");
        weaponName = weaponArea.transform.Find("WeaponName").GetComponent<Text>();
        weapon = weaponArea.transform.Find("Weapon").GetComponent<Image>();
        weaponBuff = weaponArea.transform.Find("WeaponBuff").GetComponent<Image>();

        weaponName.text = "Frying Pan";
        weapon.sprite = panSprite;
        weaponBuff.sprite = noEleSprite;

        //boarTrotterCount = 0;
        //shroomCount = 0;
        //broccoliCount = 0;
        //chickenFeetCount = 2;
        //rabbitLegCount = 2;
        //shellCount = 2;
        //wingCount = 2;
        //yolkCount = 2;
    }

    public void SetAnimation( string mode, float cooldown, bool stop = true) {
        this.mode = mode;
        setTime = cooldown;
        animator.Play("Mlafi_" + this.mode + "_" + direction);
        if (stop)
        {
            rb.velocity = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMelee)
        {
            weaponBuff.sprite = upgradeBuffSprite;

        }

        bool isAttackingOrUsingMagic = (isMelee == true && Input.GetMouseButtonDown(0) && (setTime <= (0.267f / 8.0f * 4.0f)) && mode != "Hurt")
    || (isMelee != true && Input.GetMouseButtonDown(0) && (setTime >= (0.333f * 0.6f)) && mode != "Hurt");

        bool isWalking = x != 0 || y != 0;

        if (mode == "Dead") {
            return;
        }

        x = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(x) > 0.3f)
        {
            if (x < 0)
            {
                x = -1;
            }
            else if (x > 0)
            {
                x = 1;
            }
            //Correct X
        }
        else { x = 0; }

        y = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(y) > 0.3f)
        {
            if (y < 0)
            {
                y = -1;
            }
            else if (y > 0)
            {
                y = 1;
            } //Correct Y
        }
        else { y = 0; }


        if (gameIsPaused == false && mode != "Roll" && !Input.GetButton("Fire3")) {
            rollStart = false;
        }
        
        //Roll Execute
        if (gameIsPaused == false && mode == "Roll" && setTime > 0) {
            /*if (direction == "Side")
            {
                x = rollSpeed * (transform.localScale.x * -1);
                y = 0;
            }
            else if (direction == "Back")
            {
                x = 0;
                y = rollSpeed;
            }
            else {
                x = 0;
                y = rollSpeed * -1;
            }

            rb.velocity = new Vector2(x * speed, y * speed);*/
            rb.velocity = rollVector;
            setTime -= Time.deltaTime;
            if (setTime <= 0)
            {
                mode = "Idle";
                rb.velocity = Vector2.zero;
            }
            return;
        }
      
        //Set Direction
        changeDirection();

        if (gameIsPaused == false && Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBag();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        //if (Input.GetButtonDown("Fire2") && (mode == "Idle" || mode == "Walk"))
        //{
        //    Debug.Log("Can shoot: " + GetComponent<PlayerTest>().canShoot);
        //    mode = "Magic";
        //    setTime = 0.333f;

        //    x = 0; y = 0;
        //}

        //ROLL SET
        /*        if (gameIsPaused == false && Input.GetButton("Fire3")&& !rollStart && (mode == "Idle" || mode == "Walk"))
                {
                    mode = "Roll";
                    setTime = 0.222f;

                    if (x == 0 && y == 0) {
                        if (direction == "Side")
                        {
                            if (transform.localScale.x < 0)
                            {
                                x = 1;
                            }
                            else if (transform.localScale.x > 0) {
                                x = -1;
                            }
                        }
                        else
                        {
                            if (direction == "Back")
                            {
                                y = 1;
                            }
                            else if (direction == "Front")
                            {
                                y = -1;
                            }
                            else {
                                rollStart = true; // Just in case
                                return;
                            }

                        }
                    }

                    rollVector.x = rollSpeed * x;
                    rollVector.y = rollSpeed * y ;
                    if (x != 0 && y != 0) {
                        rollVector.x *= 0.707f;
                        rollVector.y *= 0.707f;
                    }//For Diagonal

                    rollStart = true;
                    rb.velocity = rollVector;
                    animator.Play("Mlafi_" + mode + "_" + direction);
                    return;
                }*/

        //WALK
        if (gameIsPaused == false && !isAttackingOrUsingMagic)
        {
            // Code for walking
            if (isWalking)
            {
                mode = "Walk";
                rb.velocity = new Vector2(x, y).normalized * speed;
                animator.Play("Mlafi_" + mode + "_" + direction);
            }
            else
            {
                mode = "Idle";
                animator.Play("Mlafi_" + mode + "_" + direction);
            }
        }
        else if (gameIsPaused == false && isAttackingOrUsingMagic)
        {
            // Code for attacking or using magic
            if (isMelee == true && mode != "Hurt")
            {
                mode = "Attack";
                setTime = 0.267f;

                // Determine the direction based on mouse position
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 directionVector = mousePos - transform.position;
                float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;

                if (angle > -45 && angle <= 45)
                {
                    direction = "Side";
                }
                else if (angle > 45 && angle <= 135)
                {
                    direction = "Back";
                }
                else if (angle > 135 || angle <= -135)
                {
                    direction = "Side";
                }
                else
                {
                    direction = "Front";
                }

                animator.Play("Mlafi_" + mode + "_" + direction, -1, 0.0f);
                StartCoroutine(Attack());
            }
            else if (isMelee != true && mode != "Hurt")
            {
                Debug.Log("Can shoot: " + GetComponent<PlayerTest>().canShoot);
                mode = "Magic";
                setTime = 0.267f;
                animator.Play("Mlafi_" + mode + "_" + direction, -1, 0.0f);
            }
        }

        setTime -= Time.deltaTime;
        if (setTime <= 0)
        {
            mode = "Idle";
        }

        if (gameIsPaused == false && mode != "Hurt")
        {
            rb.velocity = new Vector2(x, y).normalized * speed;
        }

        animator.Play("Mlafi_" + mode + "_" + direction);

        if (gameIsPaused == false && Input.GetMouseButtonDown(1))
        {
            ToggleWeapon();
        }
    }

    public void ToggleWeapon()
    {
        if (isMelee == true)
        {
            weaponName.text = "Kitchen Gun";
            weapon.sprite = gunSprite;
            weaponBuff.sprite = upgradeBuffSprite;
            fryingPan.SetActive(false);
            kitchenGun.SetActive(true);
            isMelee = false;
        }
        else
        {
            weaponName.text = "Frying Pan";
            weapon.sprite = panSprite;
            weaponBuff.sprite = noEleSprite;
            fryingPan.SetActive(true);
            kitchenGun.SetActive(false);
            isMelee = true;
        }
    }

    public void ToggleBag()
    {
        if (bagIsOpen == true)
        {
            bagUI.SetActive(false);
            bagIsOpen = false;
        }
        else
        {
            bagUI.SetActive(true);
            bagIsOpen = true;
        }
    }

    public void ToggleStats()
    {
        if (statsOpen == true)
        {
            statsPage.SetActive(false);
            cookingPage.SetActive(true);
            statsOpen = false;
            statsToggleButtonText.text = "C";
        }
        else
        {
            cookingPage.SetActive(false);
            statsPage.SetActive(true);
            statsOpen = true;
            statsToggleButtonText.text = "S";
        }
    }

    public void TogglePause()
    {
        if (gameIsPaused == true)
        {
            pausedUI.SetActive(false);
            gameIsPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            pausedUI.SetActive(true);
            gameIsPaused = true;
            Time.timeScale = 0f;
        }
    }

    private void changeDirection() {
        // Determine the direction based on mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionVector = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;

        if (angle > -45 && angle <= 45)
        {
            direction = "Side";
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (angle > 135 || angle <= -135)
        {
            direction = "Side";
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (gameIsPaused == false && x > 0)
        {
            x = 1;
        }


        if (gameIsPaused == false && y != 0)
        {
            if (direction == "Side" && x != 0)
            {
                direction = "Side";
            }
            else
            {
                if (y < 0)
                {
                    direction = "Front";
                }
                else
                {
                    direction = "Back";
                }
                transform.localScale = new Vector3(1, 1, 1);
            }

        }
        /*else if (y < 0) {
            direction = "Front";
            transform.localScale = new Vector3(1, 1, 1);
        }*/
        else if (gameIsPaused == false && x != 0)
        {
            direction = "Side";
        }
    }

    protected IEnumerator Attack() {
        rb.velocity = Vector2.zero;
        if (hitbox != null)
        {
            Destroy(hitbox);
        }
        switch (direction)
        {
            case "Front":
                hitbox = Instantiate(frontHitbox, transform);
                break;
            case "Side":
                hitbox = Instantiate(sideHitbox, transform);
                break;
            case "Back":
                hitbox = Instantiate(backHitbox, transform);
                break;
        }

        while (setTime > 0) {
            //Cancel into attack
            //Already in update


            //Cancel into dodge roll
            if (gameIsPaused == false && Input.GetButton("Fire3") && !rollStart && setTime < (0.067f))
            {

                mode = "Roll";
                setTime = 0.111f;

                if (x == 0 && y == 0)
                {
                    if (direction == "Side")
                    {
                        if (transform.localScale.x < 0)
                        {
                            x = 1;
                        }
                        else if (transform.localScale.x > 0)
                        {
                            x = -1;
                        }
                    }
                    else
                    {
                        if (direction == "Back")
                        {
                            y = 1;
                        }
                        else if (direction == "Front")
                        {
                            y = -1;
                        }
                        else
                        {
                            rollStart = true; // Just in case
                            Destroy(hitbox);
                            hitbox = null;
                            yield break;
                        }

                    }
                }

                rollVector.x = dodgeSpeed * x;
                rollVector.y = dodgeSpeed * y;
                if (x != 0 && y != 0)
                {
                    rollVector.x *= 0.707f;
                    rollVector.y *= 0.707f;
                }//For Diagonal

                rollStart = true;
                Destroy(hitbox);
                hitbox = null;
                changeDirection();
                rb.velocity = rollVector;
                animator.Play("Mlafi_" + mode + "_" + direction);
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(0.267f);
        Destroy(hitbox);
        hitbox = null;
    }

    public void addSpeed(float speedUp)
    {
        speed += speedUp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Boar_Trotter(Clone)")
        {
            boarTrotterCount += 1;
            boarTrotterText.text = "Boar Trotters: " + boarTrotterCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Shroom(Clone)")
        {
            shroomCount += 1;
            shroomText.text = "Shrooms: " + shroomCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Broccoli(Clone)")
        {
            broccoliCount += 1;
            broccoliText.text = "Broccolis: " + broccoliCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "ChickenFeet(Clone)")
        {
            chickenFeetCount += 1;
            chickenFeetText.text = "Chicken Feets: " + chickenFeetCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "RabbitLeg(Clone)")
        {
            Debug.Log("rabbit leg count " + rabbitLegCount);
            rabbitLegCount += 1;
            Debug.Log("rabbit leg count picked up" + rabbitLegCount);
            rabbitLegText.text = "Rabbit Legs: " + rabbitLegCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Shell(Clone)")
        {
            shellCount += 1;
            shellText.text = "Shells: " + shellCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Wing(Clone)")
        {
            wingCount += 1;
            wingText.text = "Wings: " + wingCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "Yolk(Clone)")
        {
            yolkCount += 1;
            yolkText.text = "Yolks: " + yolkCount;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "UnlockFire(Clone)")
        {
            upgradeBuffSprite = fireGunSprite;
            fireProjectileHolder.fireIsUnlocked = true;
            fireProjectileHolder.FireProjectile();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "UnlockIce(Clone)")
        {
            upgradeBuffSprite = freezeGunSprite;
            iceProjectileHolder.iceIsUnlocked = true;
            iceProjectileHolder.IceProjectile();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.name == "UnlockPoison(Clone)")
        {
            upgradeBuffSprite = poisonGunSprite;
            poisonProjectileHolder.poisonIsUnlocked = true;
            poisonProjectileHolder.PoisonProjectile();
            Destroy(collision.gameObject);
        }
    }
}
