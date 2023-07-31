using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public MinimapRoom[] rooms;
    public Image[] connectorsH;
    public Image[] connectorsV;

    void Awake()
    {
        for (int i = 0 ; i < rooms.Length; ++i)
        {
            if (i / 7 < 6) rooms[i].connectorBottom = connectorsH[i];
            if (i % 7 > 0) rooms[i].connectorBottom = connectorsV[i - i / 7 - 1];
            if (i % 7 < 6) rooms[i].connectorBottom = connectorsV[i - i / 7];
            if (i / 7 > 0) rooms[i].connectorTop = connectorsH[i - 7];

            rooms[i].SetRoom(false);
        }
    }
}
