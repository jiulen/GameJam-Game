using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TeleportPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private TeleportManager teleportManager;
    public Tilemap[] symbolTilemaps;
    public bool[] symbolList = new bool[4];
    public float symbolTime = 1f;
    float currentTime = 0;

    bool teleporting = false;
    Transform playerTransform;

    public float delay = 1f;
    float delayTime = 0;

    void Start()
    {
        teleportManager = FindObjectOfType<TeleportManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (teleporting)
        {
            bool symboling = false;

            currentTime += Time.deltaTime;
            float extraTime = 0;

            for (int i = 0; i < symbolList.Length; ++i)
            {
                if (!symbolList[i])
                {
                    symboling = true;

                    if (currentTime > symbolTime)
                    {
                        extraTime = currentTime - symbolTime;
                        currentTime = symbolTime;
                    }

                    symbolTilemaps[i].color = new Color(1, 1, 1, currentTime / symbolTime);

                    if (currentTime == symbolTime)
                    {
                        symbolList[i] = true;
                        currentTime = extraTime;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (symbolList[3])
            {
                if (!symboling)
                {
                    delayTime += Time.deltaTime;
                }
                
                if (delayTime >= delay)
                {
                    teleporting = false;

                    playerTransform.position = teleportManager.getBossRoom().position;
                    teleportManager.teleported(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            teleporting = true;
            playerTransform = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            teleporting = false;

            for (int i = 0; i < symbolList.Length; ++i)
            {
                symbolList[i] = false;
                symbolTilemaps[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
}
