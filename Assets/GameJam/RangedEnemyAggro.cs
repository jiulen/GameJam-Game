using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAggro : MonoBehaviour
{
    public Animator enemyAnim;
    [SerializeField] EnemyControl enemyControl;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!enemyControl.isAttacking) 
            {
                enemyAnim.Play("Run");
                Debug.Log("Run");
            }
        }
    }
}
