using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class ColorMaster : MonoBehaviour
{
    public static ColorMaster Instance { get; private set; }

    [SerializeField] private List<AlchemyColor> alchemyColors;
    [SerializeField] private AlchemyColor black;
    [SerializeField] private AlchemyColor white;
    public AlchemyColor Black => black;
    public AlchemyColor White => white;
    
    
    private void Awake()
    {
        if (!alchemyColors.Contains(black)) { alchemyColors.Add(black); }
        if (!alchemyColors.Contains(white)) { alchemyColors.Add(white); }
        
        if (!Instance)
            Instance = this;
    }

    public AlchemyColor MixLightRayColors(AlchemyColor color1, AlchemyColor color2)
    {
        switch (color1.colorType.colorTypeEnum)
        {
            // color1 primary
            case AlchemyColor.ColorTypeEnum.Primary:
                switch (color2.colorType.colorTypeEnum)
                {
                    case AlchemyColor.ColorTypeEnum.Primary:
                        if (color1 == color2)
                        {
                            return color1;
                            
                        }
                        foreach (var secondaryColor in GetSecondaryColors().Where(secondaryColor => secondaryColor.GetComponenti().Contains(color1) &&
                                     secondaryColor.GetComponenti().Contains(color2)))
                        {
                            return secondaryColor;
                        }
                        Debug.LogError($"Colors {color1} and {color2} can't mix!");
                        return null;
                    case AlchemyColor.ColorTypeEnum.Secondary:
                        return color2.GetComponenti().Contains(color1) ? color2 : white;
                    case AlchemyColor.ColorTypeEnum.Complex:
                        return color2;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            // color1 secondary
            case AlchemyColor.ColorTypeEnum.Secondary:
                switch (color2.colorType.colorTypeEnum)
                {
                    case AlchemyColor.ColorTypeEnum.Primary:
                        return MixLightRayColors(color2, color1);
                    case AlchemyColor.ColorTypeEnum.Secondary:
                        return color1 == color2 ? color1 : white;
                    case AlchemyColor.ColorTypeEnum.Complex:
                        return white;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            // color1 complex
            case AlchemyColor.ColorTypeEnum.Complex:
                return color1;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public List<AlchemyColor> GetPrimaryColors()
    {
        return alchemyColors.Where(alchemyColor => alchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Primary).ToList();
    }
    /// <summary>
    /// Returns all the primary colors excluding the one provided
    /// </summary>
    public List<AlchemyColor> GetPrimaryColors(AlchemyColor primaryColorToExclude)
    {
        return alchemyColors.Where(alchemyColor => alchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Primary && primaryColorToExclude != alchemyColor).ToList();
    }
    public List<AlchemyColor> GetSecondaryColors()
    {
        return alchemyColors.Where(alchemyColor => alchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Secondary).ToList();
    }
    /// <summary>
    /// Returns all the secondary colors excluding the one provided
    /// </summary>
    public List<AlchemyColor> GetSecondaryColors(AlchemyColor secondaryColorToExclude)
    {
        return alchemyColors.Where(alchemyColor => alchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Secondary && secondaryColorToExclude != alchemyColor).ToList();
    }
    public List<AlchemyColor> GetComplexColors()
    {
        return alchemyColors.Where(alchemyColor => alchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Complex).ToList();
    }

    public List<AlchemyColor> GetComplexColors(AlchemyColor colore)
    {
        if (colore == black)
        {
            return  new List<AlchemyColor> {black};
        }
        else {
            return GetComplexColors();
        }
    }
}
