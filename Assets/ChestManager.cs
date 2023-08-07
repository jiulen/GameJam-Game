using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{

    public int hp, startHp = 3;
    private bool isInvincible = false;
    private float invincibleTimer;
    [SerializeField] private GameObject[] possibleUpgrades;
    SpriteRenderer spriteRenderer;

    Animator chestAnimator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = startHp;

        chestAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleTimer > 0.0f){
            invincibleTimer -= Time.deltaTime;
        }
        else   
            isInvincible = false;
    }

    public void Damage()
    {
        if (!isInvincible && hp > 0)
        {
            hp--;
            if (hp <= 0)
            {
                //Choose stuff to drop
                int dropIndex = Random.Range(0, possibleUpgrades.Length);
                GameObject upgradeToDrop = possibleUpgrades[dropIndex];

                //Drop stuff
                StartCoroutine(SpawnDrop(upgradeToDrop));

                chestAnimator.Play("ChestOpen", spriteRenderer.sortingLayerID, 0);
                
                return;
            }
            else
            {
                chestAnimator.Play("ChestHit", spriteRenderer.sortingLayerID, 0);
            }

            invincibleTimer = 0.3f;
            isInvincible = true;
        }
    }

    IEnumerator SpawnDrop(GameObject upgradeDropped)
    {
        yield return new WaitForSeconds(0.375f);
        Instantiate(upgradeDropped, transform.position, transform.rotation);
    }
}
