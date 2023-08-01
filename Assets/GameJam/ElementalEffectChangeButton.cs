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
    public bool fireIsUnlocked = false;
    public bool iceIsUnlocked = false;
    public bool poisonIsUnlocked = false;
    public Image elementSprite;
    // Start is called before the first frame update
    void Start()
    {
        isProjectileChooserActive = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void FireProjectile()
    {
        if (fireIsUnlocked == true)
        {
            Debug.Log("fire ele ----------");
            int index = (int)projectileSlotNumber;
            playerTest.projectile[0] = playerTest.projectile[1];
            playerMove.upgradeBuffSprite = playerMove.fireGunSprite;
            playerMove.weaponBuffAnim.Play("FireIcon");
        }
    }

    public void IceProjectile()
    {
        if (iceIsUnlocked == true)
        {
            Debug.Log("ice ele ----------");
            int index = (int)projectileSlotNumber;
            playerTest.projectile[0] = playerTest.projectile[2];
            playerMove.upgradeBuffSprite = playerMove.freezeGunSprite;
            playerMove.weaponBuffAnim.Play("IceIcon");
        }
    }

    public void PoisonProjectile()
    {
        if (poisonIsUnlocked == true)
        {
            Debug.Log("poison ele ----------");
            int index = (int)projectileSlotNumber;
            playerTest.projectile[0] = playerTest.projectile[3];
            playerMove.upgradeBuffSprite = playerMove.poisonGunSprite;
            playerMove.weaponBuffAnim.Play("PoisonIcon");
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
