using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingToNPC : MonoBehaviour
{
    public move playerMove;
    public GameObject npcDialogue;
    bool isTalking = false;
    bool inRangeOfNPC = false;
    public int enemyNumber;
    public Text enemyName;
    public Text enemyType;
    public Text enemyTip;
    public Image enemyImage;
    public Sprite[] enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        int randomNumber = Random.Range(1, 5);
        enemyNumber = randomNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerMove.gameIsPaused == false && playerMove.bagIsOpen == false)
        {
            ToggleDialogue();
        }
        if (npcDialogue.activeSelf)
        {
            if (enemyNumber == 1)
            {
                enemyName.text = "Boar";
                enemyType.text = "Type: Melee";
                enemyTip.text = "The boar will charge at you in a straight line";
                enemyImage.sprite = enemySprite[0];
            }
            else if (enemyNumber == 2)
            {
                enemyName.text = "Bunny";
                enemyType.text = "Type: Melee";
                enemyTip.text = "The bunny has to stop moving to attack";
                enemyImage.sprite = enemySprite[1];
            }
            else if (enemyNumber == 3)
            {
                enemyName.text = "Mushroom";
                enemyType.text = "Type: Ranged";
                enemyTip.text = "The mushroom can attack in all directions";
                enemyImage.sprite = enemySprite[2];
            }
            else if (enemyNumber == 4)
            {
                enemyName.text = "Chicken";
                enemyType.text = "Type: Ranged";
                enemyTip.text = "After defeating it, you may be in for a surprise";
                enemyImage.sprite = enemySprite[3];
            }
            else if (enemyNumber == 5)
            {
                enemyName.text = "Egg";
                enemyType.text = "Type: Melee";
                enemyTip.text = "Kaboom";
                enemyImage.sprite = enemySprite[4];
            }
            if (Input.GetMouseButtonDown(0) && playerMove.gameIsPaused == false && playerMove.bagIsOpen == false)
        {
                NextTip();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            inRangeOfNPC = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            inRangeOfNPC = false;
            if (npcDialogue.activeSelf)
            {
                npcDialogue.SetActive(false);
                isTalking = false;
            }
        }
    }
    void ToggleDialogue()
    {
        if (inRangeOfNPC == true)
        {
            if (isTalking == false)
            {
                npcDialogue.SetActive(true);
                isTalking = true;
            }
            else if (isTalking == true)
            {
                npcDialogue.SetActive(false);
                isTalking = false;
            }
        }
    }
    void NextTip()
    {
        enemyNumber += 1;
        if (enemyNumber == 6)
        {
            enemyNumber = 1;
        }
    }
}
