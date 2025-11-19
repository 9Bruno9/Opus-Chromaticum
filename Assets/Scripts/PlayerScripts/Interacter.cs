using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interacter : MonoBehaviour
{
    [SerializeField] private float maxInteractionDistance = 5f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private string[] interactableLayers;

    [SerializeField] private RawImage feedbackIcon;
    [SerializeField] private InputAction interactAction;
    
    private GameObject _lastLookedGameObject;
    public bool boolDisabled=false; 

    
    private void OnEnable()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        
        if (interactAction.IsUnityNull()) return;
        
        interactAction.started += Interact;
    }

    private void OnDisable()
    {
        if (interactAction.IsUnityNull()) return;
        
        interactAction.started -= Interact;
    }

    private void Update()
    {
        if (boolDisabled) { return; }
        
        // At each frame update, if necessary, the enabled state of the icon used to provide feedback to the user in case
        // something that can be interacted with is within the maxInteractionDistance
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out var hit, maxInteractionDistance,
                LayerMask.GetMask(interactableLayers)))
        {
            _lastLookedGameObject = hit.transform.gameObject;
            if (!feedbackIcon.enabled)
            {
                feedbackIcon.enabled = !feedbackIcon.enabled;
            }
        }
        else
        {
            _lastLookedGameObject = null;
            if (feedbackIcon.enabled)
            {
                feedbackIcon.enabled = !feedbackIcon.enabled;
            }
        }
    }

    /// <summary>
    /// Should be called on user input. Recovers the PlayerInteractable component on the last GameObject that can be
    /// interacted with that the user has looked.
    /// Does nothing if there is nothing that the user can interact with in front of him.
    /// </summary>
    /// <param name="context">CallbackContext of the InputAction</param>
    private void Interact(InputAction.CallbackContext context)
    {
        if (boolDisabled) { return; }
        if (_lastLookedGameObject.IsUnityNull()) return;

        Debug.Log("interagisco");

        var interactable = _lastLookedGameObject.GetComponentInChildren<PlayerInteractable>();
        if (!interactable.IsUnityNull())
        {
            interactable.OnInteract();
            return;
        }
        
        Debug.Log($"No available PlayerInteractable in {_lastLookedGameObject}");
    }
}
