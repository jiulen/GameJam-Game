using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Display_Inven : MonoBehaviour
{
    public HP_Display display;

    public Image[] hearts;
    public Sprite[] heartSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; ++i)
        {
            if (i >= display.maxHP)
            {
                hearts[i].enabled = false;
            }
            else
            {
                hearts[i].enabled = true;

                if (i >= display.targetHp)
                {
                    hearts[i].sprite = heartSprites[1];
                }
                else
                {
                    hearts[i].sprite = heartSprites[0];
                }
            }
        }
    }
}
