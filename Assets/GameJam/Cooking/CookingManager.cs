using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CookingManager : MonoBehaviour
{
    [System.Serializable]
    public class Slot {
        public Image slotImg;
        public Button slotButton;
        public IngredientScriptable ingredientSO;
    };
    public List<Slot> slotList = new List<Slot>();
    public List<RecipeScriptable> recipeList = new List<RecipeScriptable>();
    
    [System.Serializable]
    public class ResultSlot {
        public Image slotImg;
        public Button slotButton;
        public string resultName;
    };
    public ResultSlot resultSlot;
    public move playerMove;

    public float totalCookTimer;
    private float cookTime;
    private string cookResult;

    public Image progressArrow;

    public Text bakKutTehText;
    public int bakKutTehCount;
    public Text xiaoJiDunMoGuText;
    public int xiaoJiDunMoGuCount;
    public Text oyakodonText;
    public int oyakodonCount;
    public Text rabbitOmeletText;
    public int rabbitOmeletCount;
    public Text succulentFeetText;
    public int succulentFeetCount;

    void Start()
    {
        cookTime = totalCookTimer;
        //bakKutTehCount = 0;
        //succulentFeetCount = 21;
        //oyakodonCount = 2;
        //rabbitOmeletCount = 2;
        //xiaoJiDunMoGuCount = 2;
    }

    void Update()
    {
        if (cookTime < totalCookTimer)
        {
            cookTime += Time.deltaTime;            

            if (cookTime >= totalCookTimer)
            {
                cookTime = totalCookTimer;
                GiveDish();
                CheckRecipes();
            }

            progressArrow.fillAmount = cookTime / totalCookTimer;
        }
        else
        {
            progressArrow.fillAmount = 0;
        }        
    }

    public void TryAddIngredient(IngredientScriptable ingredient)
    {
        if (cookTime >= totalCookTimer)
        {
            foreach (Slot slot in slotList)
            {
                if (!slot.ingredientSO)
                {
                    bool haveItem = false;
                    //Remove from inventory (if possible)
                    switch (ingredient.ingredientName)
                    {
                        case "BoarTrotter":
                            if (playerMove.boarTrotterCount > 0)
                            {
                                playerMove.boarTrotterCount -= 1;
                                playerMove.boarTrotterText.text = "x " + playerMove.boarTrotterCount.ToString();
                                haveItem = true;
                            }                        
                            break;
                        case "Shroom":
                            if (playerMove.shroomCount > 0)
                            {
                                playerMove.shroomCount -= 1;
                                playerMove.shroomText.text = "x " + playerMove.shroomCount.ToString();
                                haveItem = true;
                            }
                            break;
                        case "Broccoli":
                            if (playerMove.broccoliCount > 0)
                            {
                                playerMove.broccoliCount -= 1;
                                playerMove.broccoliText.text = "x " + playerMove.broccoliCount.ToString();
                                haveItem = true;
                            }
                            break;
                        case "ChickenFeet":
                            if (playerMove.chickenFeetCount > 0)
                            {
                                playerMove.chickenFeetCount -= 1;
                                playerMove.chickenFeetText.text = "x " + playerMove.chickenFeetCount.ToString();
                                haveItem = true;
                            }
                            break;
                        case "RabbitLeg":
                            if (playerMove.rabbitLegCount > 0)
                            {
                                playerMove.rabbitLegCount -= 1;
                                playerMove.rabbitLegText.text = "x " + playerMove.rabbitLegCount.ToString();
                                haveItem = true;
                            }
                            break;
                        case "Shell":
                            if (playerMove.shellCount > 0)
                            {
                                playerMove.shellCount -= 1;
                                playerMove.shellText.text = "x " + playerMove.shellCount.ToString();
                                haveItem = true;
                            }
                            break;
                        case "Wing":
                            if (playerMove.wingCount > 0)
                            {
                                playerMove.wingCount -= 1;
                                playerMove.wingText.text = "x " + playerMove.wingCount.ToString();
                                haveItem = true;
                            }
                            break;
                        case "Yolk":
                            if (playerMove.yolkCount > 0)
                            {
                                playerMove.yolkCount -= 1;
                                playerMove.yolkText.text = "x " + playerMove.yolkCount.ToString();
                                haveItem = true;
                            }
                            break;
                        default:
                            Debug.Log("Add ingredient with wrong name");
                            break;
                    }

                    if (haveItem)
                    {
                        //Add to cooking menu
                        slot.ingredientSO = ingredient;
                        slot.slotButton.interactable = true;
                        slot.slotImg.enabled = true;
                        slot.slotImg.sprite = ingredient.ingredientSprite;

                        //Check recipes                        
                        CheckRecipes();
                    }                

                    return;
                }
            }
        }        
    }


    public void TryRemoveIngredient(int slotNum)
    {
        int maxSlotNum = 2;

        if (slotNum > maxSlotNum)
        {
            Debug.Log("Slot Num too big");
            return;
        }

        //Add ingredient back to inventory
        switch (slotList[slotNum].ingredientSO.ingredientName)
        {
            case "Boar_Trotter":
                playerMove.boarTrotterCount += 1;
                playerMove.boarTrotterText.text = "x " + playerMove.boarTrotterCount.ToString();                      
                break;
            case "Shroom":
                playerMove.shroomCount += 1;
                playerMove.shroomText.text = "x " + playerMove.shroomCount.ToString();
                break;
            case "Broccoli":
                playerMove.broccoliCount += 1;
                playerMove.broccoliText.text = "x " + playerMove.broccoliCount.ToString();
                break;
            case "ChickenFeet":
                playerMove.chickenFeetCount += 1;
                playerMove.chickenFeetText.text = "x " + playerMove.chickenFeetCount.ToString();
                break;
            case "RabbitLeg":
                playerMove.rabbitLegCount += 1;
                playerMove.rabbitLegText.text = "x " + playerMove.rabbitLegCount.ToString();
                break;
            case "Shell":
                playerMove.shellCount += 1;
                playerMove.shellText.text = "x " + playerMove.shellCount.ToString();
                break;
            case "Wing":
                playerMove.wingCount += 1;
                playerMove.wingText.text = "x " + playerMove.wingCount.ToString();
                break;
            case "Yolk":
                playerMove.yolkCount += 1;
                playerMove.yolkText.text = "x " + playerMove.yolkCount.ToString();
                break;
            default:
                Debug.Log("Remove ingredient with wrong name");
                break;
        }

        //Shift slots
        for (int i = slotNum; i < maxSlotNum; ++i)
        {
            slotList[i].ingredientSO = slotList[i+1].ingredientSO;
            slotList[i].slotButton.interactable = slotList[i+1].slotButton.interactable;
            slotList[i].slotImg.sprite = slotList[i+1].slotImg.sprite;
            slotList[i].slotImg.enabled = slotList[i+1].slotImg.enabled;
        }

        slotList[maxSlotNum].ingredientSO = null;
        slotList[maxSlotNum].slotButton.interactable = false;
        slotList[maxSlotNum].slotImg.sprite = null;
        slotList[maxSlotNum].slotImg.enabled = false;

        //Check recipes
        List<IngredientScriptable> ingredients = new List<IngredientScriptable>();
        foreach (Slot slot2 in slotList)
        {
            if (slot2.ingredientSO)
            {
                ingredients.Add(slot2.ingredientSO);
            }
        }
        CheckRecipes();     
    }

    public void CheckRecipes()
    {
        List<IngredientScriptable> ingredients = new List<IngredientScriptable>();
        foreach (Slot slot in slotList)
        {
            if (slot.ingredientSO)
            {
                ingredients.Add(slot.ingredientSO);
            }
        }

        foreach (RecipeScriptable recipe in recipeList)
        {
            bool isEqual = Enumerable.SequenceEqual(recipe.ingredientsList.OrderBy(e => e.ingredientName), ingredients.OrderBy(e => e.ingredientName));
            if (isEqual)
            {
                resultSlot.resultName = recipe.outputDishName;
                resultSlot.slotButton.interactable = true;
                resultSlot.slotImg.enabled = true;
                resultSlot.slotImg.sprite = recipe.outputDishSprite;
                
                return;
            }
        }

        resultSlot.resultName = "";
        resultSlot.slotButton.interactable = false;
        resultSlot.slotImg.enabled = false;
        resultSlot.slotImg.sprite = null;
    }

    public void StartCookDish()
    {
        if (resultSlot.resultName != "" && cookTime >= totalCookTimer)
        {
            cookTime = 0.0f;
            cookResult = resultSlot.resultName;

            progressArrow.fillAmount = cookTime / totalCookTimer;

            foreach (Slot slot in slotList)
            {
                slot.ingredientSO = null;
                slot.slotButton.interactable = false;
                slot.slotImg.sprite = null;
                slot.slotImg.enabled = false;
            }
        }
    }

    public void GiveDish()
    {
        switch (cookResult)
        {
            case "BakKutTeh":
                //Add BakKutTeh here
                bakKutTehCount += 1;
                bakKutTehText.text = "x " + bakKutTehCount.ToString();
                Debug.Log("Added BKT");
                break;
            case "XiaoJiDunMoGu":
                //Add XiaoJiDunMoGu here
                xiaoJiDunMoGuCount += 1;
                xiaoJiDunMoGuText.text = "x " + xiaoJiDunMoGuCount.ToString();
                Debug.Log("Added XJDMG");
                break;
            case "Oyakodon":
                //Add Oyakodon here
                oyakodonCount += 1;
                oyakodonText.text = "x " + oyakodonCount.ToString();
                Debug.Log("Added Oyakodon");
                break;
            case "RabbitOmelet":
                //Add RabbitOmelet here
                rabbitOmeletCount += 1;
                rabbitOmeletText.text = "x " + rabbitOmeletCount.ToString();
                Debug.Log("Added RO");
                break;
            case "SucculentFeet":
                //Add SucculentFeet here
                succulentFeetCount += 1;
                succulentFeetText.text = "x " + succulentFeetCount.ToString();
                Debug.Log("Added SF");
                break;
        }
    }
}
