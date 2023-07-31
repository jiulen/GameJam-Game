using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private bool isCleared;
    [SerializeField] private bool playerInside;

    private RoomTemplates all;

    public int roomIndex = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInside = false;
        all = FindObjectOfType<RoomTemplates>();
    }

    // If the player enters a room, that room is set as the active room
    //This is used for camera control and spawning enemies
    void Update()
    { 
    }

    public bool getIsCleared()
    {
        return isCleared;
    }

    public void setIsCleared(bool isCleared)
    {
        this.isCleared = isCleared;
    }

    public bool getPlayerInside()
    {
        return playerInside;
    }

    public void setPlayerInside(bool playerInside)
    {
        this.playerInside = playerInside;
        if (this.playerInside)
        {
            all.setActiveRoom(this.gameObject, roomIndex);
        }
    }
}
