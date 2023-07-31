using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public MinimapRoom[] rooms;
    public Image[] connectorsH;
    public Image[] connectorsV;
    public Transform currentRoomMarker;

    void Awake()
    {
        for (int i = 0 ; i < rooms.Length; ++i)
        {
            if (i / 7 < 6) rooms[i].connectorBottom = connectorsV[i];
            if (i % 7 > 0) rooms[i].connectorLeft = connectorsH[i - i / 7 - 1];
            if (i % 7 < 6) rooms[i].connectorRight = connectorsH[i - i / 7];
            if (i / 7 > 0) rooms[i].connectorTop = connectorsV[i - 7];

            rooms[i].SetRoom(false);
        }
    }
}
