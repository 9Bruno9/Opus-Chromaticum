using System;
using System.Linq;
using System.Security.Cryptography;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {

    [SerializeField] AlchemyColor Red;
    [SerializeField] AlchemyColor Blue;
    [SerializeField] AlchemyColor Yellow;
    [SerializeField] AlchemyColor Green;
    [SerializeField] AlchemyColor Orange;
    [SerializeField] AlchemyColor Purple;
    [SerializeField] AlchemyColor Black;
    [SerializeField] AlchemyColor White;


    public UnityEvent<bool> OnInventorySetUp;
    public InventoryColor[] listaColori = new InventoryColor[8];

    public bool BlackAvailable = false;
    public bool WhiteAvailable = false;


    private void Awake()
    {
        // inizializzazione iniziale
        listaColori[0] = new InventoryColor(Red, 0, 0);
        listaColori[1] = new InventoryColor(Blue, 0, 0);
        listaColori[2] = new InventoryColor(Yellow, 0, 0);
        listaColori[3] = new InventoryColor(Green, 0, 0);
        listaColori[4] = new InventoryColor(Purple, 0, 0);
        listaColori[5] = new InventoryColor(Orange, 0, 0);
        listaColori[6] = new InventoryColor(Black, 0, 0);
        listaColori[7] = new InventoryColor(White, 0, 0);
    }

  
    

    public void SaveInventory()
    {
    
        if (GameManager.instance != null)
        {
            GameManager.instance.savedInventory = listaColori;

        }
    }
  

    public class InventoryColor
    {
        //[HideInInspector]
        public int materialUnits = 100;
        //[HideInInspector]
        public int colorUnits = 100;
        public AlchemyColor Itemcolor;


        public InventoryColor(AlchemyColor colore, int materials, int colors)
        {
            Itemcolor = colore;
            materialUnits = materials;
            colorUnits = colors;
        }

    }




    public void AddMaterial(AlchemyColor materiale, int quantity)
    {
        for (int i = 0; i < listaColori.Length; i++)
        {
            if (listaColori[i].Itemcolor == materiale)
            {
                listaColori[i].materialUnits = Mathf.Min(100, listaColori[i].materialUnits + quantity);
                //Debug.Log("Elemento " + i + " aggiornato");
                SaveInventory();
            }
        }
    }


    public bool SubMaterial(AlchemyColor materiale, int quantity)
    {
        for (int i = 0; i < listaColori.Length; i++)
        {
            if (listaColori[i].Itemcolor == materiale)
            {
                if (listaColori[i].materialUnits >= quantity)
                {
                    listaColori[i].materialUnits -= quantity;
                    SaveInventory();
                    return true;
                    
                }
                return false;
            }
           
        }
        return false;
    }


    /// <summary>
    /// Check if an AlchemyColor is in the inventory
    /// </summary>
    /// <param name="materiale">The AlchemyColor to look for</param>
    /// <param name="quantity">The minimum quantity that has to be present</param>
    /// <returns></returns>
    public bool CheckColor(AlchemyColor materiale, int quantity)
    {
        return listaColori.Any(t => t.Itemcolor == materiale && t.colorUnits >= quantity);
    }

    public bool CheckColorMaterial(AlchemyColor materiale, int quantity)
    {
        return listaColori.Any(t => t.Itemcolor == materiale && t.materialUnits >= quantity);
    }

   

    public void AddColor(AlchemyColor materiale, int quantity)
    {
        for (int i = 0; i < listaColori.Length; i++)
        {
            if (listaColori[i].Itemcolor == materiale)
            {
                listaColori[i].colorUnits = Mathf.Min(100, listaColori[i].colorUnits + quantity);
                SaveInventory();
            }
        }
    }


    public bool SubColor(AlchemyColor materiale, int quantity)
    {
        for (int i = 0; i < listaColori.Length; i++)
        {
            if (listaColori[i].Itemcolor == materiale)
            {
                if(listaColori[i].colorUnits >= quantity)
                {
                    listaColori[i].colorUnits -= quantity;
                    SaveInventory();
                    //Debug.Log("Elemento " + i + " aggiornato, quantit�" + listaColori[i].colorUnits);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }


    public void AddWhiteMaterials(int quantity)
    {
        if (listaColori.IsUnityNull())
        {
            Awake();
        }
        if (listaColori[7].materialUnits > 0 || listaColori[7].colorUnits > 0)
        {
            return;
        }
        else { AddMaterial(White, 5); }
    }

    public void AddBlackMaterials(int quantity)
    {

        if (listaColori.IsUnityNull())
        {
            Awake();
        }
        if ( (listaColori[6].materialUnits > 0 || listaColori[6].colorUnits > 0) )
        {
            return;
        }
        else { AddMaterial(Black, 5); }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.instance != null && GameManager.instance.savedInventory[0] != null)
        {
            
            Debug.Log("colore" + GameManager.instance.savedInventory[0].Itemcolor.LatinName);
            listaColori = GameManager.instance.savedInventory;

            BlackAvailable = GameManager.instance.BlackAvailable;
            WhiteAvailable = GameManager.instance.WhiteAvailable;
        }
        else
        {
            
            SaveInventory();
        }
        OnInventorySetUp.Invoke(true);
    }
       
   
}
