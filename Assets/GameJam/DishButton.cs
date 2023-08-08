using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishButton : MonoBehaviour
{
    public ITEM itemType;
    private Inventory inventory;
    public CookingManager cookingManager;
    
    // Start is called before the first frame update
    void Start()
    {
        cookingManager = FindObjectOfType<CookingManager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void DishToHotkey()
    {
        switch (itemType)
        {
            case ITEM.bakKutTeh:
                if (cookingManager.bakKutTehCount > 0)
                {
                    if (inventory.slots.Count < inventory.Capacity)
                    {
                        //Add to last slot available
                        cookingManager.bakKutTehCount -= 1;
                        cookingManager.bakKutTehText.text = "x " + cookingManager.bakKutTehCount.ToString();
                        Debug.Log("Item Added");
                        inventory.slots.Add(itemType);
                    }
                    else
                    {
                        //Pop out the active slot
                        cookingManager.bakKutTehCount -= 1;
                        cookingManager.bakKutTehText.text = "x " + cookingManager.bakKutTehCount.ToString();
                        Debug.Log("Item Replaced");
                        inventory.slots[inventory.slotUsed] = itemType;
                    }
                }
            break;
            case ITEM.xiaoJiDunMoGu:
                if (cookingManager.xiaoJiDunMoGuCount > 0)
                {
                    if (inventory.slots.Count < inventory.Capacity)
                    {
                        //Add to last slot available
                        cookingManager.xiaoJiDunMoGuCount -= 1;
                        cookingManager.xiaoJiDunMoGuText.text = "x " + cookingManager.xiaoJiDunMoGuCount.ToString();
                        Debug.Log("Item Added");
                        inventory.slots.Add(itemType);
                    }
                    else
                    {
                        //Pop out the active slot
                        cookingManager.xiaoJiDunMoGuCount -= 1;
                        cookingManager.xiaoJiDunMoGuText.text = "x " + cookingManager.xiaoJiDunMoGuCount.ToString();
                        Debug.Log("Item Replaced");
                        inventory.slots[inventory.slotUsed] = itemType;
                    }
                }
            break;
            case ITEM.oyakodon:
                if (cookingManager.oyakodonCount > 0)
                {
                    if (inventory.slots.Count < inventory.Capacity)
                    {
                        //Add to last slot available
                        cookingManager.oyakodonCount -= 1;
                        cookingManager.oyakodonText.text = "x " + cookingManager.oyakodonCount.ToString();
                        Debug.Log("Item Added");
                        inventory.slots.Add(itemType);
                    }
                    else
                    {
                        //Pop out the active slot
                        cookingManager.oyakodonCount -= 1;
                        cookingManager.oyakodonText.text = "x " + cookingManager.oyakodonCount.ToString();
                        Debug.Log("Item Replaced");
                        inventory.slots[inventory.slotUsed] = itemType;
                    }
                }
            break;
            case ITEM.rabbitOmelet:
                if (cookingManager.rabbitOmeletCount > 0)
                {
                    if (inventory.slots.Count < inventory.Capacity)
                    {
                        //Add to last slot available
                        cookingManager.rabbitOmeletCount -= 1;
                        cookingManager.rabbitOmeletText.text = "x " + cookingManager.rabbitOmeletCount.ToString();
                        Debug.Log("Item Added");
                        inventory.slots.Add(itemType);
                    }
                    else
                    {
                        //Pop out the active slot
                        cookingManager.rabbitOmeletCount -= 1;
                        cookingManager.rabbitOmeletText.text = "x " + cookingManager.rabbitOmeletCount.ToString();
                        Debug.Log("Item Replaced");
                        inventory.slots[inventory.slotUsed] = itemType;
                    }
                }
            break;
            case ITEM.succulentFeet:
                if (cookingManager.succulentFeetCount > 0)
                {
                    if (inventory.slots.Count < inventory.Capacity)
                    {
                        //Add to last slot available
                        cookingManager.succulentFeetCount -= 1;
                        cookingManager.succulentFeetText.text = "x " + cookingManager.succulentFeetCount.ToString();
                        Debug.Log("Item Added");
                        inventory.slots.Add(itemType);
                    }
                    else
                    {
                        //Pop out the active slot
                        cookingManager.succulentFeetCount -= 1;
                        cookingManager.succulentFeetText.text = "x " + cookingManager.succulentFeetCount.ToString();
                        Debug.Log("Item Replaced");
                        inventory.slots[inventory.slotUsed] = itemType;
                    }
                }
            break;
        }
    }
}
