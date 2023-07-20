using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class CollisionHandler : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider2D;
    PlayerStats playerStats;
    private bool noBlockBuff;

    void Start()
    {
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        noBlockBuff = false;

        // Get the TilemapCollider2D component attached to the walls
        //tilemapCollider2D = GameObject.Find("Layout Walls").GetComponentInChildren<TilemapCollider2D>();

        //Debug.Log(tilemapCollider2D);
    }

    //void Update()
    //{
    //    noBlockBuff = playerStats.getNoBlockBuff();
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Yuzuzu1");
    //    Debug.Log(collision.gameObject);
    //    noBlockBuff = playerStats.getNoBlockBuff();
    //    tilemapCollider2D = playerStats.getTilemapCollider2D();
    //    Debug.Log(tilemapCollider2D);
    //    Debug.Log(noBlockBuff + "  buff");
    //    if (noBlockBuff)
    //    {
    //        if (collision.gameObject.CompareTag("Enemy"))
    //        {
    //            // Handle collision between enemy and wall
    //            // For example, disable the TilemapCollider2D for the enemy
    //            //tilemapCollider2D.enabled = true;
    //            playerStats.setTilemapCollider2D(true);
    //            Debug.Log("is Enemy");
    //        }

    //        if (collision.gameObject.CompareTag("Player"))
    //        {
    //            // Handle collision between player and wall
    //            // For example, disable the TilemapCollider2D for the player
    //            //tilemapCollider2D.enabled = false;
    //            playerStats.setTilemapCollider2D(false);
    //            Debug.Log("is player");
    //        }
    //        Debug.Log("enabled walls??");
    //        Debug.Log(tilemapCollider2D.enabled);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Yuzuzu1");
        Debug.Log(collision.gameObject);
        noBlockBuff = playerStats.getNoBlockBuff();
        tilemapCollider2D = playerStats.getTilemapCollider2D();
        Debug.Log(tilemapCollider2D);
        Debug.Log(noBlockBuff + "  buff");

        if (noBlockBuff)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Ignore collision between enemy and wall
                Physics2D.IgnoreCollision(tilemapCollider2D, collision.collider, true);
                Debug.Log("is Enemy");
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                // Re-enable collision between player and wall
                Physics2D.IgnoreCollision(tilemapCollider2D, collision.collider, false);
                Debug.Log("is player");
            }

            Debug.Log("enabled walls??");
            Debug.Log(tilemapCollider2D.enabled);
        }
    }


    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    Debug.Log("Yuzuzu2");
    //    Debug.Log(collision.gameObject);
    //    noBlockBuff = playerStats.getNoBlockBuff();
    //    tilemapCollider2D = playerStats.getTilemapCollider2D();
    //    Debug.Log(tilemapCollider2D);
    //    Debug.Log(noBlockBuff + "  buff");
    //    if (noBlockBuff)
    //    {
    //        if (collision.gameObject.CompareTag("Enemy"))
    //        {
    //            // Handle collision exit between enemy and wall
    //            // For example, enable the TilemapCollider2D for the enemy
    //            //tilemapCollider2D.enabled = true;
    //            Physics2D.IgnoreLayerCollision(3, 9);
    //        }

    //        if (collision.gameObject.CompareTag("Player"))
    //        {
    //            //Handle collision exit between player and wall
    //            //For example, enable the TilemapCollider2D for the player

    //            //tilemapCollider2D.enabled = false;
    //            Physics2D.IgnoreCollision(tilemapCollider2D, collision.collider, true);
    //        }
    //    }
    //}
}
