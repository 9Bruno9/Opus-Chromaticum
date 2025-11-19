using System.Collections;
using UnityEngine;

public class LightCaster : LightInteractable
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Transform casterObject;
    [SerializeField] private float degreesPerFrame;
    
    private void Start()
    {
        lr.startColor = interactableAlchemyColor.itemColor;
        lr.endColor = interactableAlchemyColor.itemColor;
        RecalculateRay();
        casterObject.GetComponent<Renderer>().material.SetColor("_BaseColor", InteractableAlchemyColor.itemColor);
    }

    private void FixedUpdate()
    {
        Rotate();
        //Remove comment on following line ONLY FOR DEBUG PURPOSES!
        //ExecuteRayCast(rayOrigin.position, rayOrigin.forward, interactableAlchemyColor);
    }
    
    /// <summary>
    /// Start the recalculation of the light path starting from this LightCaster
    /// </summary>
    [ContextMenu("RecalculateRay")]
    public void RecalculateRay()
    {
        lr.enabled = false;
        lr.SetPosition(0, rayOrigin.position);
        lr.enabled = true;
        StartCoroutine(LateRecalculate());
    }

    private IEnumerator LateRecalculate()
    {
        // Wait for the mesh colliders of the other LightInteractable to update...
        yield return new WaitForFixedUpdate();
        ExecuteRayCast(rayOrigin.position, rayOrigin.forward, interactableAlchemyColor);
    }

    private void Rotate()
    {
        casterObject.Rotate(0f, 0f, degreesPerFrame);
    }
}
