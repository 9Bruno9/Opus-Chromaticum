using System;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    /// <summary>
    /// Called when the user interacts with this Interactable
    /// </summary>
    /// <exception cref="NotImplementedException">Thrown if called but not overridden</exception>
    public virtual void OnInteract()
    {
        throw new NotImplementedException();
    }
}
