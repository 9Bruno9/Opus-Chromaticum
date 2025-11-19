using System.Collections.Generic;

using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventoryUI
{
    public class InventoryUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryUI;
        
        [SerializeField] private GameObject cauldronPermanentUpgradeUI;
        [SerializeField] private GameObject incineratorPermanentUpgradeUI;
        [SerializeField] private GameObject fountainPermanentUpgradeUI;

        [SerializeField] private List<RectTransform> enabledOnOpen;
        [SerializeField] private List<RectTransform> disabledOnOpen;

        [SerializeField] private PlayerInput toDisable;
        
        [SerializeField] private List<InventoryUIItem> uiItems;
        [SerializeField] private Inventory inventory;

        public GameObject healthbar;
        public GameObject healthBarFake;
        public GameObject colorunitscounter;
        public GameObject materialBar;

        public bool IsInventoryOpen => inventoryUI.activeSelf;
        private InputAction _openInventoryAction;

        private void Awake()
        {
            _openInventoryAction = InputSystem.actions.FindAction("Cancel");
            _openInventoryAction.performed += ToggleInventory;
        }

     

        private void OnEnable()
        {
            inventoryUI.SetActive(false);
            if (!_openInventoryAction.IsUnityNull())
                _openInventoryAction.performed += ToggleInventory;
        }

        private void OnDisable()
        {
            _openInventoryAction.performed -= ToggleInventory;
        }

        [ContextMenu("InitializeUI")]
        public void InitializeUI()
        {
            inventory = GameObject.Find("Player").GetComponent<Inventory>();
            for (var i = 0; i < uiItems.Count; i++)
            {
                if (inventory.listaColori[i].IsUnityNull()) return;
                
                uiItems[i].LabelText.text = inventory.listaColori[i].Itemcolor.LatinName;
            }

            
            incineratorPermanentUpgradeUI.SetActive(GameManager.instance.BlackAvailable);
            fountainPermanentUpgradeUI.SetActive(GameManager.instance.WhiteAvailable);
                
        }
        
        [ContextMenu("UpdateUI")]
        public void UpdateUI()
        {
            for (var i = 0; i < uiItems.Count; i++)
            {
                
                if (inventory.listaColori[i].IsUnityNull()) return;
                
                uiItems[i].MaterialQuantityText.text = inventory.listaColori[i].materialUnits.ToString();
                uiItems[i].ColorQuantityText.text = inventory.listaColori[i].colorUnits.ToString();
            }
        }

        public void UpdateUI(AlchemyColor colorToUpdate)
        {
            for (var i = 0; i < inventory.listaColori.Length; i++)
            {
                if (inventory.listaColori[i] == null || inventory.listaColori[i].Itemcolor != colorToUpdate) continue;
                
                uiItems[i].MaterialQuantityText.text = inventory.listaColori[i].materialUnits.ToString();
                uiItems[i].ColorQuantityText.text = inventory.listaColori[i].colorUnits.ToString();
            }
        }

        public void UpdateUI(int colorIndexToUpdate)
        {
            uiItems[colorIndexToUpdate].MaterialQuantityText.text = inventory.listaColori[colorIndexToUpdate].materialUnits.ToString();
            uiItems[colorIndexToUpdate].ColorQuantityText.text = inventory.listaColori[colorIndexToUpdate].colorUnits.ToString();
        }

        public void SetCauldronUpgradeUI(bool active)
        {
            cauldronPermanentUpgradeUI.SetActive(active);
        }
        public void SetIncineratorUpgradeUI(bool active)
        {
            incineratorPermanentUpgradeUI.SetActive(active);
        }
        public void SetFountainUpgradeUI(bool active)
        {
            fountainPermanentUpgradeUI.SetActive(active);
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            
            
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
                healthbar.SetActive(true);
                healthBarFake.SetActive(true);
                colorunitscounter.SetActive(true);
                materialBar.SetActive(true);    

                // Prepare the UI for the next time it is open, resetting it to the desired state
                foreach (var item in enabledOnOpen)
                {
                    item.gameObject.SetActive(true);
                }
                foreach (var item in disabledOnOpen)
                {
                    item.gameObject.SetActive(false);
                }

                GameManager.instance.isAMenuOpen = false;
            }
            else
            {
                if (GameManager.instance.isAMenuOpen) return;
                UpdateUI();
                inventoryUI.SetActive(true);
                healthbar.SetActive(false);
                healthBarFake.SetActive(false);
                colorunitscounter.SetActive(false);
                materialBar.SetActive(false);
                GameManager.instance.isAMenuOpen = true;
            }
        }
    }
}
