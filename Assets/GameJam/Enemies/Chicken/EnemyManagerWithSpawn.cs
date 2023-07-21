using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerWithSpawn : EnemyManager
{
    [SerializeField]
    Transform enemySpawn;
    [SerializeField]
    GameObject enemyToSpawnPrefab;
    [SerializeField]
    Vector3 enemySpawnOffset;

    public void Damage(int p)
    {
        if (!isInvincible)
        {
            hp -= p;
            StartCoroutine(FlashRed());
            if (hp <= 0)
            {
                if (!roomIndependent)
                {
                    room.getActiveRoom().GetComponent<DoorManager>().killEnemy();
                }

               
                if (endGame)
                {
                    StartCoroutine(GameOver());
                }
                else {
                    Instantiate(lootItem, transform.position, Quaternion.identity);
                    Instantiate(enemyToSpawnPrefab, enemySpawn.position + enemySpawnOffset, Quaternion.identity);
                    Destroy(gameObject);
                    playerStats.GainExperience(experienceToGive);
                }
                
            }
        }
    }
}
