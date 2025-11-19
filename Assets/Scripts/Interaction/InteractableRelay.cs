using UnityEngine;
using UnityEngine.Events;

public class InteractableRelay : PlayerInteractable
{
    public UnityEvent<bool> onInteractEvent;

    [SerializeField] private bool needNoMenuOpen;
    
    public override void OnInteract()
    {
        if (needNoMenuOpen && GameManager.instance.isAMenuOpen) return;
        onInteractEvent.Invoke(true);
    }
}
