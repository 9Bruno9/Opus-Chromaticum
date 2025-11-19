using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{
    public Vector2 normalisedMousePosition;
    public float currentAngle;
    public int selection;
    private int previousSelection;

    public Image Selector;
    public GameObject PieMenu;
    public GameObject StaffColor;
    public AlchemyColor[] menuItems;
    private AlchemyColor equip_color;
    public AlchemyColor white;
    public AlchemyColor black;
    public GameObject BlackImm;
    public GameObject WhiteImm;

    private bool menuActive = false;

    private float qHoldTime = 0f;
    public float qActivationThreshold = 3f;
    public GameObject Player;

    private PlayerMr playerMr;
    private Inventory Inventario;
    

    InputAction CraftAction;
    InputAction RadialmenuAction;
    private InventoryUI.InventoryUIManager inventoryUIManager;

    public bool BlackAvailable = false;
    public bool WhiteAvailable = false;

    private bool _keepCrafting;

    public GameEvent CursorON;
    public GameEvent CursorOFF;

    public AlchemyColor Equipaggiato()
    {
        return equip_color;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // All'avvio il menù è disattivo
        PieMenu.SetActive(false);
        equip_color = menuItems[0];
        
        StaffColor.GetComponent<ColorStaff>().SetColor(equip_color);
        playerMr = Object.FindAnyObjectByType<PlayerMr>();
        Inventario = Player.GetComponent<Inventory>();
        inventoryUIManager = FindFirstObjectByType<InventoryUI.InventoryUIManager>();
        BlackAvailable = GameManager.instance.BlackAvailable;
        WhiteAvailable = GameManager.instance.WhiteAvailable;
        


        //disattiva le due immagini
    }


    private Coroutine _craftCoroutine;





    private void OnDisable()
    {
        CraftAction.started -= Startcrafting;
        RadialmenuAction.started -= OpenRadMenu;
        RadialmenuAction.canceled -= CloseRadMenu;
    }

    private void OnEnable()
    {

        CraftAction = InputSystem.actions.FindAction("crafting");
        CraftAction.started += Startcrafting;
        CraftAction.canceled += context =>
        {
            _keepCrafting = false;
           /* if(!_craftCoroutine.IsUnityNull())
                StopCoroutine(_craftCoroutine);*/
        };

        RadialmenuAction = InputSystem.actions.FindAction("OpenRadial");
        RadialmenuAction.started += OpenRadMenu;
        RadialmenuAction.canceled += CloseRadMenu;



    }

    void Startcrafting(InputAction.CallbackContext context)
    {
        _keepCrafting = true;
        _craftCoroutine = StartCoroutine(ConstantCraft());
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

    private void OpenRadMenu(InputAction.CallbackContext context)
    {
        if (GameManager.instance.isAMenuOpen) return;
        // Apri il menù
        menuActive = true;
        GameManager.instance.isAMenuOpen = true;
        PieMenu.SetActive(true);
        CursorON.TriggerEvent();
        if (!BlackAvailable)
        {
            BlackImm.SetActive(false);
        }
        if (!WhiteAvailable)
        {
            WhiteImm.SetActive(false);
        }

        playerMr.SetLookEnabled(false);
    }

    private void CloseRadMenu(InputAction.CallbackContext context)
    {
        if (!menuActive) return;
        menuActive = false;
        GameManager.instance.isAMenuOpen = false;
        PieMenu.SetActive(false);
        CursorOFF.TriggerEvent();
        playerMr.SetLookEnabled(true);
    }

    private void Craft()
    {


        if (equip_color.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Primary)
        {
            if (Inventario.CheckColor(equip_color, 100))
                return;
            //TODO: Execute Player.GetComponent<Inventory>() in Start() then use the reference in the methods
            if (Inventario.SubMaterial(equip_color, 1) )
            {
                Inventario.AddColor(equip_color, 1);
            }

        }

        if (equip_color.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Secondary)
        {
            if (Inventario.CheckColor(equip_color, 100))
                return;

            if (Inventario.SubMaterial(equip_color, 1))
            {
                Inventario.AddColor(equip_color, 1);
            }
            else if (Inventario.CheckColorMaterial(equip_color.GetComponenti()[0], 1) && Inventario.CheckColorMaterial(equip_color.GetComponenti()[1], 1))
            {
                Inventario.SubMaterial(equip_color.GetComponenti()[0], 1);
                Inventario.SubMaterial(equip_color.GetComponenti()[1], 1);
                Inventario.AddColor(equip_color, 1);
            }
            //else if()
        }


        if (equip_color == black && BlackAvailable)
        {
            if (Inventario.CheckColor(equip_color, 100))
                return;

            if (Inventario.SubMaterial(equip_color, 1))
            {
                Inventario.AddColor(equip_color, 1);
            }
            else if (Inventario.CheckColorMaterial(equip_color.GetComponenti()[0], 1) && Inventario.CheckColorMaterial(equip_color.GetComponenti()[1], 1)
                && Inventario.CheckColorMaterial(equip_color.GetComponenti()[2], 1) && Inventario.CheckColorMaterial(equip_color.GetComponenti()[3], 1)
                && Inventario.CheckColorMaterial(equip_color.GetComponenti()[4], 1) && Inventario.CheckColorMaterial(equip_color.GetComponenti()[5], 1))
            {
                Inventario.SubMaterial(equip_color.GetComponenti()[0], 1);
                Inventario.SubMaterial(equip_color.GetComponenti()[1], 1);
                Inventario.SubMaterial(equip_color.GetComponenti()[2], 1);
                Inventario.SubMaterial(equip_color.GetComponenti()[3], 1);
                Inventario.SubMaterial(equip_color.GetComponenti()[4], 1); 
                Inventario.SubMaterial(equip_color.GetComponenti()[5], 1);
                Inventario.AddColor(equip_color, 1);
            }
            //else sounds not working

        }


        if (equip_color == white && WhiteAvailable)
        {
            if (Inventario.CheckColor(equip_color, 100))
                return;
            if (Inventario.SubMaterial(equip_color, 1))
            {
                Inventario.AddColor(equip_color, 1);
            }
            else if (Inventario.SubMaterial(equip_color.GetComponenti()[0], 1))
            {
                Inventario.AddColor(equip_color, 1);
            }
        }


    }




    // Update is called once per frame
    void Update()
    {



        if (!menuActive) return; // Se il menù chiuso, esci


        normalisedMousePosition = Mouse.current.position.ReadValue();
        normalisedMousePosition.x = normalisedMousePosition.x - (Screen.width / 2);
        normalisedMousePosition.y = normalisedMousePosition.y - (Screen.height / 2);
        currentAngle = Mathf.Atan2(normalisedMousePosition.x, normalisedMousePosition.y) * Mathf.Rad2Deg;


        currentAngle = (currentAngle + 360+ (45/2)) % 360;

        selection = (int)currentAngle / 45; //360/8=45


        if (!BlackAvailable)
        { 
            selection = Mathf.Min(selection, 5); 

        }
        else if (!WhiteAvailable)
        { 
            selection = Mathf.Min(selection, 6); 
        }



        if (selection != previousSelection)
        {
            previousSelection = selection;
            // Ruota il Selector
            Selector.rectTransform.rotation = Quaternion.Euler(0f, 0f, -selection * 45f);

            equip_color = menuItems[selection];
           
            Renderer renderer = StaffColor.GetComponent<Renderer>();
            renderer.material.color = equip_color.itemColor;

            StaffColor.GetComponent<ColorStaff>().SetColor(equip_color);


        }
    }
    


    public void SetWhite()
    {
        WhiteAvailable = true;
        WhiteImm.SetActive(true);
    }
    public void SetBlack()
    {
        BlackAvailable = true;
        BlackImm.SetActive(true);

    }

}
