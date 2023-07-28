using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirepointScript : MonoBehaviour
{
    public Transform playerTransform;
    public move playerMove;
    public PlayerTest playerTest;
    public Animator animator;
    float rotationZ;
    public GameObject gunScale;
    public Vector3 offset;
    bool lookingLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 originalScale = new Vector3(-1, 1, 1);
        Vector3 desiredScale = new Vector3(-1, -1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.transform.position + offset;

        rotationZ = transform.rotation.eulerAngles.z;

        Vector3 originalScale = new Vector3(-1f, 1f, 1f);
        Vector3 desiredScale = new Vector3(-1f, -1f, 1f);

        if (playerMove.gameIsPaused == false && playerMove.isMelee == false && Input.GetMouseButtonDown(0) && playerTest.canShoot)
        {
            if (lookingLeft == true)
            {
                animator.Play("GunAnimationOtherSide");
            }
            else
            {
                animator.Play("GunAnimation");
            }
        }

        if (playerMove.gameIsPaused == false && playerMove.isMelee == true && Input.GetMouseButtonDown(0))
        {
            animator.Play("WeaponAnimation");
        }

        if (rotationZ > 180f)
        {
            rotationZ -= 360f;
        }

        if (90f <= rotationZ && rotationZ <= 180f)
        {
            gunScale.transform.localScale = desiredScale;
            lookingLeft = true;
        }
        else if (-180f <= rotationZ && rotationZ <= -90f)
        {
            gunScale.transform.localScale = desiredScale;
            lookingLeft = true;
        }
        else 
        {
            gunScale.transform.localScale = originalScale;
            lookingLeft = false;
        }
    }
}
