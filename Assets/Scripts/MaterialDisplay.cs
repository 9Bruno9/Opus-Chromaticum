using UnityEngine;
using TMPro;
using ScriptableObjects;

public class MaterialDisplay : MonoBehaviour
{
    public GameObject Player;
    public GameObject StaffSphere;
    public TextMeshProUGUI equippedMat;

    void Update()
    {
        UpdateEquippedMat();
    }

    void UpdateEquippedMat()
    {
        foreach (var colore in Player.GetComponent<Inventory>().listaColori)
        {
            if (colore.Itemcolor == StaffSphere.GetComponent<ColorStaff>().GiveColor())

            {
                equippedMat.text = $"materiali : {colore.materialUnits}";
                return;
            }
        }

        // Nessun colore trovato / non equipaggiato
        equippedMat.text = "No color equipped";
    }
}
