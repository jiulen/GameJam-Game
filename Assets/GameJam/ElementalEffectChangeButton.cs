using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalEffectChangeButton : MonoBehaviour
{
    public PlayerTest playerTest;
    public float projectileSlotNumber;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeProjectileType()
    {
        int index = (int)projectileSlotNumber;
        playerTest.projectile[0] = playerTest.projectile[index];
    }

}
