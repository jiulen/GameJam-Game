using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{
    Image room;
    public Image connectorBottom, connectorLeft, connectorRight, connectorTop = null;
    public bool connectB, connectL, connectR, connectT = false;

    // Start is called before the first frame update
    void Awake()
    {
        room = GetComponent<Image>();
    }

    public void SetRoom(bool roomActive)
    {
        room.enabled = roomActive;

        if (roomActive)
        {
            SetConnectors(true);
        }
        else
        {
            SetConnectors(false);
        }
    }
    
    public void SetDirections(bool b = false, bool l = false, bool r = false, bool t = false)
    {
        connectB = b;
        connectL = l;
        connectR = r;
        connectT = t;
    }

    void SetConnectors(bool active)
    {
        if (active)
        {
            if (connectorBottom != null) connectorBottom.enabled = connectB;
            if (connectorLeft != null) connectorLeft.enabled = connectL;
            if (connectorRight != null) connectorRight.enabled = connectR;
            if (connectorTop != null) connectorTop.enabled = connectT;
        }
        else
        {
            if (connectorBottom != null) connectorBottom.enabled = false;
            if (connectorLeft != null) connectorLeft.enabled = false;
            if (connectorRight != null) connectorRight.enabled = false;
            if (connectorTop != null) connectorTop.enabled = false;
        }
    }
}
