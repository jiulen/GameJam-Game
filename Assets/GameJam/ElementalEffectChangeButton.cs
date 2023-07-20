using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalEffectChangeButton : MonoBehaviour
{
    public PlayerTest playerTest;
    public float projectileSlotNumber;
    public GameObject projectileChooser;
    public bool isProjectileChooserActive;
    public move playerMove;
    // Start is called before the first frame update
    void Start()
    {
        isProjectileChooserActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeProjectileType()
    {
        int index = (int)projectileSlotNumber;
        playerTest.projectile[0] = playerTest.projectile[index];
    }

    public void ToggleProjectileChooser()
    {
        if (isProjectileChooserActive == false)
        {
            projectileChooser.SetActive(true);
            isProjectileChooserActive = true;
        }
        else
        {
            projectileChooser.SetActive(false);
            isProjectileChooserActive = false;
        }
    }

}
