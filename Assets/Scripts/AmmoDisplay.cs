using UnityEngine;
using TMPro;
using ScriptableObjects;

public class HUDManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject StaffSphere;
    public TextMeshProUGUI equippedColorText;

    private Inventory _inventory;
    private ColorStaff _staff;

    private void Start()
    {
        Player = GameObject.Find("Player");
        _inventory = Player.GetComponent<Inventory>();
        _staff = StaffSphere.GetComponent<ColorStaff>();
    }

    
    private void Update()
    {
        UpdateEquippedHUD();
    }

    private void UpdateEquippedHUD()
    {
        foreach (var colore in _inventory.listaColori)
        {
            if (colore.Itemcolor != _staff.GiveColor()) continue;
            equippedColorText.text = $"{colore.Itemcolor.LatinName}: {colore.colorUnits}";
            return;
        }

        // Nessun colore trovato / non equipaggiato
        equippedColorText.text = "No color equipped";
    }
    
    
    
    public void UpdateEquippedHUD(AlchemyColor colorToSet)
    {
        foreach (var colore in _inventory.listaColori)
        {
            if (colore.Itemcolor != colorToSet) continue;
            
            equippedColorText.text = $"{colore.Itemcolor.LatinName}: {colore.colorUnits}";
            return;
        }

        // Nessun colore trovato / non equipaggiato
        equippedColorText.text = "No color equipped";
    }
}
