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

    bool teleporting = false;

    void Start()
    {
        teleportManager = FindObjectOfType<TeleportManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (teleporting)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            teleporting = true;

            collision.transform.position = teleportManager.getBossRoom().position;
            teleportManager.teleported(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            teleporting = false;
        }
    }
}
