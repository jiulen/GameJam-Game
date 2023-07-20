using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int atkDamage; //melee
    [SerializeField] private int dexDamage; //ranged
    [SerializeField] private int soulCount;
    [SerializeField]
    private GameObject poisonBall;
    public RoomTemplates room;
    private GameObject activeRoom;
    private GameObject roomObject;
    public TilemapCollider2D tilemapCollider2D;
    private Collision wallCollision;
    private GameObject playerObject;
    private GameObject enemyObject;
    private RoomTrigger roomTrigger;
    public GameObject layoutWalls;
    public bool noBlockBuff;
    private float playerSpeed;
    public int currentLevel;
    public int currentExperiencePoints;
    public int levelUpCost;
    // Level Up Stats
    public int bonusATK = 0, bonusDEX = 0, bonusVIT = 0; //ATK for more melee dmg, DEX for more ranged dmg, VIT for more health
    public int unusedStatPoints = 0;
    public string playerLayerName = "";
    private int playerLayerNum;
    public string wallsLayerName = "";
    private int wallsLayerNum;

    private HealthManager healthManager;
    private move move;

    // For buffs
    public int atkBuff = 10;
    public float speedBuff = 10f;
    //Upgrades

    public List<(string, string)> upgradeDescriptions; // Item1 is the name of upgrade, Item2 is the effect of upgrade

    // Start is called before the first frame update
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        move = GetComponent<move>();
        room = FindObjectOfType<RoomTemplates>();
        roomTrigger = FindObjectOfType<RoomTrigger>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        playerLayerNum = LayerMask.NameToLayer(playerLayerName);
        wallsLayerNum = LayerMask.NameToLayer(wallsLayerName);
        tilemapCollider2D = null;
        noBlockBuff = false;
        levelUpCost = 10 * currentLevel + 10;
        upgradeDescriptions = new List<(string, string)>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getSoulCount()
    {
        return soulCount;
    }

    public void addSouls(int souls)
    {
        soulCount += souls;
    }

    public void spendSouls(int cost)
    {
        soulCount -= cost;
    }

    public int getMeleeDamage()
    {
        return atkDamage;
    }

    public int getRangedDamage()
    {
        return dexDamage;
    }

    public void addMeleeDamage(int damage)
    {
        atkDamage += damage;
    }

    public void addRangedDamage(int damage)
    {
        dexDamage += damage;
    }

    public void GainExperience(int experience)
    {
        currentExperiencePoints += experience;
        if (currentExperiencePoints >= levelUpCost)
        {
            currentLevel++;
            Debug.Log(currentLevel);
            currentExperiencePoints -= levelUpCost;
            healthManager.heal(1);
            unusedStatPoints++;

            levelUpCost = 10 * currentLevel + 10;
        }
    }

    public void IncreaseStats(int statType)
    {
        if (unusedStatPoints <= 0)
            return;

        switch (statType)
        {
            case 0:
                ++bonusATK;
                addMeleeDamage(1);
                Debug.Log("IncreaseStats : bonusATK = " + bonusATK);
                break;
            case 1:
                ++bonusDEX;
                addRangedDamage(1);
                Debug.Log("IncreaseStats : bonusDEX = " + bonusDEX);
                break;
            case 2:
                ++bonusVIT;
                healthManager.addMaxHealth(1);
                Debug.Log("IncreaseStats : bonusDEF = " + bonusVIT);
                break;
            default:
                Debug.Log("IncreaseStats Error : INVALID STAT TYPE");
                break;
        }

        unusedStatPoints--;
    }

    public int GetBonusStats(int statType)
    {
        switch (statType)
        {
            case 0:
                Debug.Log("GetBonusStats : bonusATK = " + bonusATK);
                return bonusATK;
            case 1:
                Debug.Log("GetBonusStats : bonusDEX = " + bonusDEX);
                return bonusDEX;
            case 2:
                Debug.Log("GetBonusStats : bonusDEF = " + bonusVIT);
                return bonusVIT;
            default:
                Debug.Log("GetBonusStats : INVALID STAT TYPE");
                return 0;

        }
    }

    public void AddBuffs(int buffType)
    {
        GameObject obj;
        switch (buffType)
        {
            case 0:
                Debug.Log("Increase attack buff");
                addMeleeDamage(atkBuff);
                addRangedDamage(atkBuff);
                break;
            case 1:
                Debug.Log("Speed up buff");
                move.addSpeed(speedBuff);
                break;
            case 2:
                Debug.Log("Evasion buff");
                healthManager.setInvis(10);
                break;
            case 3:
                Debug.Log("Poisonous buff");
                obj = Instantiate(poisonBall);
                switch (gameObject.GetComponent<move>().Direction)
                {
                    case "Front":
                        obj.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - 0.5f);
                        obj.transform.localEulerAngles = new Vector3(0, 0, 270);
                        break;
                    case "Back":
                        obj.transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 1.5f);
                        obj.transform.localEulerAngles = new Vector3(0, 0, 90);
                        break;
                    default:
                        if (transform.localScale.x < 0)
                        {
                            obj.transform.localPosition = new Vector2(transform.localPosition.x + 1, transform.localPosition.y + 1);
                            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            obj.transform.localPosition = new Vector2(transform.localPosition.x - 1, transform.localPosition.y + 1);
                            obj.transform.localEulerAngles = new Vector3(0, 0, 180);
                        }
                        break;
                }
                break;
            case 4:
                Debug.Log("No block buff");
                //roomTrigger.noBlockBuff = true;
                noBlockBuff = true;
                activeRoom = room.getActiveRoom();
                enemyObject = GameObject.FindGameObjectWithTag("Enemy");
                Debug.Log("---------");
                Debug.Log(activeRoom);

                // Iterate through the parent's children to find the desired child GameObjects
                for (int i = 0; i < activeRoom.transform.childCount; i++)
                {
                    Transform childTransform = activeRoom.transform.GetChild(i);

                    // Check if the childTransform matches the desired name pattern
                    if (childTransform.name.StartsWith("Room") && childTransform.name.EndsWith("(Clone)"))
                    {
                        // Access and use the childTransform.gameObject as needed
                        roomObject = childTransform.gameObject;
                        Debug.Log(roomObject);
                        break; // Break the loop once the desired child GameObject is found
                    }
                }

                GameObject layoutGrid = roomObject.transform.Find("Layout Grid").gameObject;
                Debug.Log(layoutGrid);

                layoutWalls = layoutGrid.transform.Find("Layout Walls").gameObject;
                Debug.Log(layoutWalls);
                tilemapCollider2D = layoutWalls.GetComponentInChildren<TilemapCollider2D>();
                Debug.Log("player and walls----- " + playerLayerNum + " " + wallsLayerNum);
                Physics2D.IgnoreLayerCollision(playerLayerNum, wallsLayerNum);
                //roomTrigger.tilemapCollider2D = tilemapCollider2D;
                //roomTrigger.setBuffActive(tilemapCollider2D);

                break;
            default:
                break;
        }
    }



    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("cOLLIED");
    //    Debug.Log(noBlockBuff);

    //    if (noBlockBuff)
    //    {
    //        //Collider2D collider = collision.collider;
    //        Collider2D otherCollider = collision.otherCollider;
    //        //Debug.Log(collider); // walls
    //        //Debug.Log(otherCollider); // player

    //        // Access the game objects associated with the colliders
    //        //GameObject gameObjectA = collider.gameObject;
    //        GameObject gameObjectB = otherCollider.gameObject;

    //        // Perform actions based on the collided game objects
    //        if (layoutWalls && gameObjectB.CompareTag("Player"))
    //        {
    //            Debug.Log("Player collided with: " + gameObjectB.name);
    //            // Perform actions specific to player colliding with gameObjectB
    //            tilemapCollider2D.enabled = false;
    //        }
    //    }
    //}

    //// This function is called when a 2D collider exits the trigger collider of the TilemapCollider2D (wall)
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (noBlockBuff)
    //    {
    //        Debug.Log("cOLLIED - exit");
    //        //Collider2D collider = collision.collider; // walls
    //        Collider2D otherCollider = collision.otherCollider; // player
    //                                                            //Debug.Log(collider); // walls
    //                                                            //Debug.Log(otherCollider); // player

    //        GameObject gameObjectB = otherCollider.gameObject;

    //        // Check if the collider is the player or enemy
    //        if (layoutWalls && gameObjectB.CompareTag("Player"))
    //        {
    //            // Handle collision exit between player and wall
    //            // For example, enable the TilemapCollider2D for the player
    //            tilemapCollider2D.enabled = true;
    //        }
    //    }
    //}


    public void RemoveBuffs(int buffType)
    {
        switch (buffType)
        {
            case 0: 
                Debug.Log("Remove attack buff");
                addMeleeDamage(atkBuff * -1);
                addRangedDamage(atkBuff * -1);
                break;
            case 1:
                Debug.Log("Remove Speed up buff");
                move.addSpeed(speedBuff * -1);
                break;
            case 2:
                Debug.Log("Remove Evasion buff");
                break;
            case 3:
                Debug.Log("Remove Poisonous buff");
                break;
            case 4:
                Debug.Log("Remove No block buff");
                //roomTrigger.noBlockBuff = false;
                //roomTrigger.tilemapCollider2D = null;
                noBlockBuff = false;
                //roomTrigger.setBuffActive(null);
                //tilemapCollider2D.enabled = true;
                Physics2D.IgnoreLayerCollision(playerLayerNum, wallsLayerNum, false);
                Debug.Log("walls enabled????");
                //Debug.Log(tilemapCollider2D.enabled);
                break;
            default:
                break;
        }
    }

    public bool getNoBlockBuff()
    {
        return noBlockBuff;
    }

    public GameObject getLayoutWalls()
    {
        return layoutWalls;
    }

    public TilemapCollider2D getTilemapCollider2D()
    {
        return tilemapCollider2D;
    }

    public void setTilemapCollider2D(bool ifEnabled)
    {
        Debug.Log("I am called func" + ifEnabled);
        this.tilemapCollider2D.enabled = ifEnabled;
        Debug.Log("I am called func result" + this.tilemapCollider2D.enabled);
    }
}
