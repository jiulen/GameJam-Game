using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingToNPC : MonoBehaviour
{
    public move playerMove;
    public GameObject npcDialogue;
    bool isTalking = false;
    bool inRangeOfNPC = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerMove.gameIsPaused == false)
        {
            ToggleDialogue();
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
}
