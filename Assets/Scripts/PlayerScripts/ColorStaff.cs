using ScriptableObjects;

using UnityEngine;

public class ColorStaff : MonoBehaviour
{
    
    private AlchemyColor colorebastone;
    

    public AlchemyColor GiveColor()
    {
        return colorebastone;
    }

    public void SetColor(AlchemyColor changeColor)
    {
        colorebastone = changeColor;
        //Debug.Log("coloreCambiato"+changeColor.ToString());
    }
}
