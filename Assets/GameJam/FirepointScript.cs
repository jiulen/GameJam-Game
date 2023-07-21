using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirepointScript : MonoBehaviour
{
    public Transform playerTransform;
    public move playerMove;
    public PlayerTest playerTest;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.transform.position;

        if (playerMove.gameIsPaused == false && playerMove.isMelee == false && Input.GetMouseButtonDown(0) && playerTest.canShoot)
        {
            animator.Play("GunAnimation");
        }

        if (playerMove.gameIsPaused == false && playerMove.isMelee == true && Input.GetMouseButtonDown(0))
        {
            animator.Play("WeaponAnimation");
        }
    }
}
