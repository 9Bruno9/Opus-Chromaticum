using System.Collections;
using System.Collections.Generic;
using System.Text;
using ScriptableObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    [SerializeField] private GameObject craftingPanel;
    
    [SerializeField] private TMP_Dropdown colorDropDown;
    [SerializeField] private TMP_Dropdown ingredientDropDown;

    [SerializeField] private TMP_Text colorFeedbackText;
    [SerializeField] private TMP_Text ingredientFeedbackText;
    [SerializeField] private TMP_Text transmuteButtonText;
    
    [SerializeField] private Sprite colorSprite;
    
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private int multiplier = 3;
    
    private bool _keepCrafting;
    private Coroutine _constantCraftCoroutine;
    
    [ContextMenu("Initialize")]
    public void InitializeCraftingMenu()
    {

        playerInventory = GameObject.Find("Player").GetComponent<Inventory>();


        transmuteButtonText.text = "Effettua\nTrasmutazione";
        colorFeedbackText.text = "";
        ingredientFeedbackText.text = "";
        colorDropDown.options = new List<TMP_Dropdown.OptionData>();
        foreach (var color in playerInventory.listaColori)
        {
            if (color.Itemcolor == ColorMaster.Instance.Black && !GameManager.instance.BlackAvailable)
            {
                continue;
            }
            if (color.Itemcolor == ColorMaster.Instance.White && !GameManager.instance.WhiteAvailable)
            {
                continue;
            }
            colorDropDown.options.Add(new TMP_Dropdown.OptionData(color.Itemcolor.LatinName, colorSprite, color.Itemcolor.itemColor));
        }

        ingredientDropDown.options = new List<TMP_Dropdown.OptionData>
        {
            new("Materiali"),
            new("Colori")
        };
        
        //Force initial selection and update
        colorDropDown.value = 0;
        ingredientDropDown.value = 0;
        UpdateIngredientDropDown(0);
        UpdateIngredientText(0);
    }

    public void AttemptMenuOpen()
    {
        if (GameManager.instance.isAMenuOpen) return;
        GameManager.instance.isAMenuOpen = true;
        craftingPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        GameManager.instance.isAMenuOpen = false;
        craftingPanel.SetActive(false);
    }

    public void UpdateIngredientDropDown(int selectedColor)
    {
        if (playerInventory.listaColori[selectedColor].Itemcolor.colorType.colorTypeEnum ==
            AlchemyColor.ColorTypeEnum.Primary)
        {
            //Primary colors can't be transmuted from other color, only from materials...
            ingredientDropDown.options = new List<TMP_Dropdown.OptionData>
            {
                new("Materiali")
            };
            //Reset value since the "Color Units" options does no longer exists
            ingredientDropDown.value = 0;
        }
        else
        {
            ingredientDropDown.options = new List<TMP_Dropdown.OptionData>
            {
                new("Materiali"),
                new("Colori")
            };
        }
    }
    
    public void UpdateColorText(int selectedColor)
    {
        colorFeedbackText.text = playerInventory.listaColori[selectedColor].colorUnits.ToString();
        UpdateIngredientText(ingredientDropDown.value);
        UpdateTransmuteButtonText();
    }
    public void UpdateIngredientText(int selectedIngredientType)
    {
        //No item selected in first drop-down
        if (colorDropDown.value < 0) return;
        
        if (selectedIngredientType == 0)
        {
            ingredientFeedbackText.text = playerInventory.listaColori[colorDropDown.value].materialUnits.ToString();
        }
        else
        {
            var sb = new StringBuilder();
            foreach (var component in playerInventory.listaColori[colorDropDown.value].Itemcolor.GetComponenti())
            {
                sb.Append(component.LatinName);
                sb.Append(": ");
                foreach (var inventoryColor in playerInventory.listaColori)
                {
                    if(inventoryColor.Itemcolor == component)
                        sb.Append(inventoryColor.colorUnits);
                }
                sb.Append("\n");
            }
            ingredientFeedbackText.text = sb.ToString();
        }

        UpdateTransmuteButtonText();
    }

    
    private void UpdateTransmuteButtonText()
    {
        var colorToCraft = playerInventory.listaColori[colorDropDown.value];
        if (!playerInventory.CheckColor(colorToCraft.Itemcolor, 100))
        {
            switch (ingredientDropDown.value)
            {
                case 0://With materials
                    {
                        if (colorToCraft.materialUnits > 0)
                        {
                            transmuteButtonText.text = "Effettua\nTrasmutazione";
                            break;
                        }
                        transmuteButtonText.text = "Ingredienti\nInsufficienti";
                        break;
                    }
                case 1://With color components
                    {
                        var allComponents = true;
                        foreach (var colorComponent in colorToCraft.Itemcolor.GetComponenti())
                        {
                            allComponents &= playerInventory.CheckColor(colorComponent, 1);
                        }
                        if (allComponents)
                        {
                            transmuteButtonText.text = "Effettua\nTrasmutazione";
                            break;
                        }
                        transmuteButtonText.text = "Ingredienti\nInsufficienti";
                        break;
                    }
            }
        }
        else
        {
            transmuteButtonText.text = "Inventario\npieno";
            
        }
    }

    public void Craft()
    {
        var colorToCraft = playerInventory.listaColori[colorDropDown.value].Itemcolor;
        if (!playerInventory.CheckColor(colorToCraft, 100))
        {
            switch (ingredientDropDown.value)
            {
                case 0://With materials
                    {
                        if (playerInventory.CheckColorMaterial(colorToCraft, 1))
                        {
                            playerInventory.SubMaterial(colorToCraft, 1);
                            playerInventory.AddColor(colorToCraft, multiplier);
                            //Craft success
                        }
                        //Craft fail
                        break;
                    }
                case 1://With color components
                    {
                        var allComponents = true;
                        foreach (var colorComponent in colorToCraft.GetComponenti())
                        {
                            allComponents &= playerInventory.CheckColor(colorComponent, 1);
                        }
                        if (allComponents)
                        {
                            var n = 0;
                            foreach (var colorComponent in colorToCraft.GetComponenti())
                            {
                                playerInventory.SubColor(colorComponent, 1);
                                n++;
                            }
                            playerInventory.AddColor(colorToCraft, n);
                        }
                        break;
                    }
            }
        }
        InternalTextUpdate();
    }

    public void BeginConstantCraft()
    {
        _keepCrafting = true;
        _constantCraftCoroutine = StartCoroutine(ConstantCraft());
    }
    public void EndConstantCraft()
    {
        _keepCrafting = false;
        if (!_constantCraftCoroutine.IsUnityNull())
        {
            StopCoroutine(_constantCraftCoroutine);
        }
    }

    private IEnumerator ConstantCraft()
    {
        var waitTime = .5f;
        while (_keepCrafting)
        {
            Craft();
            yield return new WaitForSeconds(waitTime);
            if (waitTime > 0.05f)
            {
                waitTime *= 0.8f;
            }
        }
    }

    private void InternalTextUpdate()
    {
        UpdateColorText(colorDropDown.value);
        UpdateIngredientText(ingredientDropDown.value);
    }
}
