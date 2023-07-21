using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalEffectChangeButton : MonoBehaviour
{
    public PlayerTest playerTest;
    public float projectileSlotNumber;
    public GameObject projectileChooser;
    public bool isProjectileChooserActive;
    public move playerMove;
    public bool isUnlocked;
    public Image elementSprite;
    // Start is called before the first frame update
    void Start()
    {
        isUnlocked = false;
        isProjectileChooserActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUnlocked == true)
        {
            elementSprite.color = Color.white;
        }
    }
    public void ChangeProjectileType()
    {
        if (isUnlocked == true)
        {
            int index = (int)projectileSlotNumber;
            playerTest.projectile[0] = playerTest.projectile[index];
        }
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
