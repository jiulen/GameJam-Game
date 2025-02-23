using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{
    Image room;
    public Image connectorBottom, connectorLeft, connectorRight, connectorTop = null;
    public bool connectB, connectL, connectR, connectT = false;
    Image icon;

    public void SetRoom(bool roomActive)
    {
        if (room == null) room = GetComponent<Image>();
        room.enabled = roomActive;
        if (icon == null)
        {
            icon = transform.GetChild(0).GetComponent<Image>();
        }
        if (icon.sprite != null)
        {
            icon.enabled = true;
        }

        if (roomActive)
        {
            SetConnectors(true);
        }
        else
        {
            SetConnectors(false);
        }
    }
    
    public void SetDirections(bool b, bool l, bool r, bool t)
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

    public void SetIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }
}
