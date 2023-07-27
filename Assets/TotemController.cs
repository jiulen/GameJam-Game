using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemController : MonoBehaviour
{
    public int hp = 20;
    public int healHP = 1;
    public float healInterval = 1f;
    private RoomTemplates room;
    private GameObject activeRoom;
    private GameObject roomObject;
    private bool isAlive;
    private Coroutine totemHealCoroutine = null;
  
    SpriteRenderer spriteRenderer;
    DoorManager door;

    // Start is called before the first frame update
    void Start()
    {
        room = FindObjectOfType<RoomTemplates>();
        isAlive = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(room);
        door = GetComponentInParent<DoorManager>();
    }

    public void startTotemHeal()
    {
        // Get a reference to the active room (assuming it's managed somewhere in your code)
        activeRoom = room.getActiveRoom();
        Debug.Log(activeRoom);
        if (activeRoom != null)
        {
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
            // Start the healing coroutine if it's not already running
            if (totemHealCoroutine == null)
            {
                totemHealCoroutine = StartCoroutine(TotemHealCoroutine());
            }
        }
    }

    public IEnumerator TotemHealCoroutine()
    {
        while (isAlive)
        {
            // Get all enemies with the "enemy" tag in the active room
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {

                EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
                if (enemyManager != null)
                {

                    int currentHp = enemyManager.getHp();
                    int startHp = enemyManager.getStartHp();

                    enemyManager.heal(currentHp, startHp, healHP);
                    enemyManager.TotemHealingEffect.SetActive(true);
                }
            }

            // Wait for the next heal tick
            yield return new WaitForSeconds(healInterval);
        }
    }

    public void Damage(int damage)
    {
        //startTotemHeal();
        if (isAlive)
        {
            hp -= damage;
            StartCoroutine(FlashRed());
            if (hp <= 0)
            {
                // The totem has been destroyed, perform any necessary actions
                isAlive = false;
                if (totemHealCoroutine != null)
                {
                    StopCoroutine(totemHealCoroutine);
                }                
                Destroy(this.gameObject);

                // Get all enemies with the "enemy" tag in the active room
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {

                    EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
                    if (enemyManager != null)
                    {
                        if (enemyManager.TotemHealingEffect != null) {
                            enemyManager.TotemHealingEffect.SetActive(false);
                        }                       
                    }
                }
                door.killEnemy();
            }
        }
    }


    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

}
