using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverBehaviour : MonoBehaviour
{
    public Vector3 offset;
    public Image hoverBg;
    public Text hoverText;
    RectTransform hoverRectTransform;

    Rect screenRect;

    bool isEnabled = false;

    int flip = 1;

    // Start is called before the first frame update
    void Start()
    {
        hoverRectTransform = GetComponent<RectTransform>();

        screenRect = new Rect (0, 0, Screen.width, Screen.height);

        DisableHover();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPos();

        if (!gameObject.activeInHierarchy)
        {
            hoverText.text = "";

            isEnabled = false;

            hoverBg.enabled = false;
            hoverText.enabled = false;
        }
    }

    void CheckPos()
    {
        if (isEnabled)
        {
            hoverRectTransform.pivot = new Vector2(0, 0.5f);
            transform.position = Input.mousePosition + offset;

            Vector3[] corners = new Vector3[4];
		    hoverRectTransform.GetWorldCorners(corners);
            
            if(screenRect.Contains(corners[0]) && screenRect.Contains(corners[1]) && 
               screenRect.Contains(corners[2]) && screenRect.Contains(corners[3]))
            {
                flip = 1;
                hoverRectTransform.pivot = new Vector2(0, 0.5f);
            }
            else
            {
                flip = -1;
                hoverRectTransform.pivot = new Vector2(1, 0.5f);
            }
            
            transform.position = Input.mousePosition + offset * flip;
        }
        else
        {
            hoverRectTransform.pivot = new Vector2(0, 0.5f);
            transform.position = Input.mousePosition + offset;
        }
    }

    public void EnableHover(string textShown)
    {        
        hoverText.text = textShown;

        isEnabled = true;

        hoverBg.enabled = true;
        hoverText.enabled = true;

        CheckPos();
    }

    public void DisableHover()
    {
        hoverText.text = "";

        isEnabled = false;

        hoverBg.enabled = false;
        hoverText.enabled = false;

        CheckPos();
    }
}
