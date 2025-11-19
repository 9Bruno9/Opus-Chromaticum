using TMPro;
using UnityEngine;

public class MaterialBar : MonoBehaviour
{
        public GameObject Player;
        public TextMeshProUGUI equippedColorText;
    private Inventory inventario;
        private void Start()
        {
            Player = GameObject.Find("Player");
            inventario = Player.GetComponent<Inventory>();
        }

        void Update()
        {
            UpdateEquippedHUD();
        }

        void UpdateEquippedHUD()
        {      
                    equippedColorText.text = $"{inventario.listaColori[0].materialUnits}\n\n{inventario.listaColori[1].materialUnits}\n\n{inventario.listaColori[2].materialUnits}\n\n{inventario.listaColori[3].materialUnits}\n\n{inventario.listaColori[4].materialUnits}\n\n{inventario.listaColori[5].materialUnits}\n\n{inventario.listaColori[6].materialUnits}\n\n{inventario.listaColori[7].materialUnits}";
                    return;
        }
    }

