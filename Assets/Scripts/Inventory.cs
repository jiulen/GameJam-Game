using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM {
    NONE,
    FIRE,
    THUNDER,
    EARTH,
    SHADOW,
    POTION,
    bakKutTeh,
    succulentFeet,
    oyakodon,
    rabbitOmelet,
    xiaoJiDunMoGu
}

public class Inventory : MonoBehaviour
{
    const float magicTime = 0.25f;
    const float animTime = 0.4f;
    //checks whether an item is in an inventiry slot
    //public bool isFull;
    public int Capacity {
        get { return 4; }
    }

    public List<ITEM> slots = new List<ITEM>();
    public int slotUsed;

    [SerializeField]
    private GameObject fireShot;
    [SerializeField]
    private GameObject sparkField;
    [SerializeField]
    private GameObject poisonBall;
    [SerializeField]
    private GameObject atkBuff;
    [SerializeField]
    private GameObject evasionBuff;
    [SerializeField]
    private GameObject speedBuff;
    [SerializeField]
    private GameObject poisonousBuff;
    [SerializeField]
    private GameObject noBlockBuff;
    [SerializeField]
    private GameObject atkBuffIntry;
    [SerializeField]
    private GameObject evasionBuffIntry;
    [SerializeField]
    private GameObject speedBuffIntry;
    [SerializeField]
    private GameObject poisonousBuffIntry;
    [SerializeField]
    private GameObject noBlockBuffIntry;
    [SerializeField]
    private float buffDuration = 10f;

    public Buffs playerBuff;
    private List<GameObject> buffList;
    move playerMove;

    private CookingManager cookingManager;

    void Start()
    {
        playerMove = GetComponent<move>();
        playerBuff = GetComponent<Buffs>();
        buffList = new List<GameObject> { atkBuff, evasionBuff, speedBuff, poisonousBuff, noBlockBuff, atkBuffIntry, evasionBuffIntry, speedBuffIntry, poisonousBuffIntry, noBlockBuffIntry };
        playerBuff.InitializeBuffIcons(buffList);
        
        cookingManager = FindObjectOfType<CookingManager>();
    }

    void Update()
    {
        if (slots.Count < 1)
        {
            return;
        }

        if (playerMove.gameIsPaused == false && slots.Count > 0 && (gameObject.GetComponent<move>().Mode == "Idle" || gameObject.GetComponent<move>().Mode == "Walk"))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("Execute Item Effect");

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    slotUsed = 0;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    slotUsed = 1;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    slotUsed = 2;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    slotUsed = 3;
                }

                StartCoroutine(UseItem(slots[slotUsed]));
                slots.RemoveAt(slotUsed);
            }
        }
    }

    IEnumerator UseItem(ITEM type)
    {
        GameObject obj;
        switch (type)
        {
            case ITEM.FIRE:

                gameObject.GetComponent<move>().SetAnimation("Magic", animTime);
                yield return new WaitForSeconds(magicTime);

                //create fire prefab
                obj = Instantiate(fireShot);

                switch (gameObject.GetComponent<move>().Direction)
                {
                    case "Front":
                        obj.transform.localPosition = new Vector2(transform.localPosition.x - 1, transform.localPosition.y - 0.5f);
                        obj.transform.localEulerAngles = new Vector3(0, 0, 270);
                        break;
                    case "Back":
                        obj.transform.localPosition = new Vector2(transform.localPosition.x + 1, transform.localPosition.y + 1.5f);
                        obj.transform.localEulerAngles = new Vector3(0, 0, 90);
                        break;
                    default:
                        if (transform.localScale.x < 0)
                        {
                            obj.transform.localPosition = new Vector2(transform.localPosition.x + 1, transform.localPosition.y);
                            obj.transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        else
                        {
                            obj.transform.localPosition = new Vector2(transform.localPosition.x - 1, transform.localPosition.y + 2);
                            obj.transform.localEulerAngles = new Vector3(0, 0, 180);
                        }
                        break;
                }
                break;
            case ITEM.THUNDER:
                gameObject.GetComponent<move>().SetAnimation("Magic", animTime);
                yield return new WaitForSeconds(magicTime);
                obj = Instantiate(sparkField);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(0, 1, 0);
                break;
            case ITEM.EARTH:
                gameObject.GetComponent<move>().SetAnimation("Magic", animTime);
                yield return new WaitForSeconds(magicTime);
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
            case ITEM.SHADOW:
                //Set Invis Frames Here
                GetComponent<HealthManager>().setInvis(5);
                break;
            case ITEM.POTION:
                //Increase HP
                GetComponent<HealthManager>().heal(12);
                break;
            case ITEM.bakKutTeh:
                //Put Food Buff Here
                GetComponent<PlayerStats>().AddBuffs(0);
                //Increase attack
                int meleeAtk = GetComponent<PlayerStats>().getMeleeDamage();
                int rangedAtk = GetComponent<PlayerStats>().getMeleeDamage();
                Debug.Log("add buff, melee atk" + meleeAtk);
                Debug.Log("add buff, ranged atk" + rangedAtk);

                // Add the buff icon to the container list
                playerBuff.AddBuffIcon(atkBuff, buffDuration);
                playerBuff.AddBuffIcon(atkBuffIntry, buffDuration);

                yield return new WaitForSeconds(buffDuration);

                GetComponent<PlayerStats>().RemoveBuffs(0);
                //playerBuff.RemoveBuffIcon(meleeAtkBuff);
                //playerBuff.RemoveBuffIcon(rangedAtkBuff);

                meleeAtk = GetComponent<PlayerStats>().getMeleeDamage();
                rangedAtk = GetComponent<PlayerStats>().getMeleeDamage();
                Debug.Log("Remove buff");
                Debug.Log("after remove, melee atk" + meleeAtk);
                Debug.Log("after remove, ranged atk" + rangedAtk);
                break;
            case ITEM.oyakodon:
                // Increase Speed
                Debug.Log("Used  oyakodon");
                GetComponent<PlayerStats>().AddBuffs(1);
                // Add the buff icon to the container list
                playerBuff.AddBuffIcon(speedBuff, buffDuration);
                playerBuff.AddBuffIcon(speedBuffIntry, buffDuration);

                yield return new WaitForSeconds(buffDuration);

                GetComponent<PlayerStats>().RemoveBuffs(1);
                Debug.Log("Remove buff");
                break;
            case ITEM.rabbitOmelet:
                // 100% Evasion for 10S
                Debug.Log("Used  ro");
                GetComponent<PlayerStats>().AddBuffs(2);
                // Add the buff icon to the container list
                playerBuff.AddBuffIcon(evasionBuff, buffDuration);
                playerBuff.AddBuffIcon(evasionBuffIntry, buffDuration);

                yield return new WaitForSeconds(buffDuration);

                GetComponent<PlayerStats>().RemoveBuffs(2);
                break;
            case ITEM.succulentFeet:
                // Poisonous Vomit
                Debug.Log("Used  sf");
                gameObject.GetComponent<move>().SetAnimation("Magic", animTime);
                GetComponent<PlayerStats>().AddBuffs(3);
                playerBuff.AddBuffIcon(poisonousBuff, buffDuration - 3f); // the travel speed for the poison ball is 3s
                playerBuff.AddBuffIcon(poisonousBuffIntry, buffDuration - 3f);
                yield return new WaitForSeconds(buffDuration);

                GetComponent<PlayerStats>().RemoveBuffs(3);
                break;
            case ITEM.xiaoJiDunMoGu:
                // Walk over walls
                Debug.Log("Used  xjdmg");
                GetComponent<PlayerStats>().AddBuffs(4);
                playerBuff.AddBuffIcon(noBlockBuff, buffDuration);
                playerBuff.AddBuffIcon(noBlockBuffIntry, buffDuration);
                yield return new WaitForSeconds(buffDuration);
                GetComponent<PlayerStats>().RemoveBuffs(4);
                break;
        }
    }

    public void UnequipDish(int slotNum)
    {
        if (slots.Count <= slotNum) return;

        slots.RemoveAt(slotUsed);
        ITEM type = slots[slotUsed];
        
        switch (type)
        {
            case ITEM.bakKutTeh:            
                cookingManager.bakKutTehCount += 1;
                cookingManager.bakKutTehText.text = "x " + cookingManager.bakKutTehCount.ToString();
                break;
            case ITEM.oyakodon:
                cookingManager.oyakodonCount += 1;
                cookingManager.oyakodonText.text = "x " + cookingManager.oyakodonCount.ToString();
                break;
            case ITEM.rabbitOmelet:
                cookingManager.rabbitOmeletCount += 1;
                cookingManager.rabbitOmeletText.text = "x " + cookingManager.rabbitOmeletCount.ToString();
                break;
            case ITEM.succulentFeet:
                cookingManager.succulentFeetCount += 1;
                cookingManager.succulentFeetText.text = "x " + cookingManager.succulentFeetCount.ToString();
                break;
            case ITEM.xiaoJiDunMoGu:
                cookingManager.xiaoJiDunMoGuCount += 1;
                cookingManager.xiaoJiDunMoGuText.text = "x " + cookingManager.xiaoJiDunMoGuCount.ToString();
                break;
        }
    }
}
