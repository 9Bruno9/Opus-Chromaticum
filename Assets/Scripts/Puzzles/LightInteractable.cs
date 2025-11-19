using System;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightInteractable : PlayerInteractable
{
    [SerializeField] private PuzzleManager puzzleManager;

    [SerializeField] private float maxRayDistance = 30f;
    
    [SerializeField] protected AlchemyColor interactableAlchemyColor;
    public AlchemyColor InteractableAlchemyColor => interactableAlchemyColor;

    [SerializeField] protected LineRenderer lr;

    [SerializeField] protected LightInteractable lastHit;
    
    [Header("Deprecated")]
    [SerializeField] protected LightInteractable parent;
    [SerializeField] protected LightCaster lightCaster;
    public LightCaster LightCaster => lightCaster;
    
    
    private void Awake()
    {
        SetUp();
    }
    protected void SetUp()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.positionCount = 2;
    }
    
    /// <summary>
    /// Perform a recalculation of the light path
    /// </summary>
    public override void OnInteract()
    {
        //lightCaster.RecalculateRay();
        ResetInteractable();
        puzzleManager.UpdateAllLightCasters();
    }

    /// <summary>
    /// Defines the behaviour when a light ray hits this GameObject
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="incomingDirection"></param>
    /// <param name="lightColor"></param>
    /// <param name="source"></param>
    /// <exception cref="NotImplementedException">Throws exception if called when not overridden</exception>
    public virtual void OnLightHit(RaycastHit hit, Vector3 incomingDirection, AlchemyColor lightColor, LightInteractable source)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Reset the last LightInteractable hit only if it's different from the provided one
    /// </summary>
    /// <param name="lightInteractableHit">The "new" LightInteractable that has been hit</param>
    public virtual void CheckLastHit(LightInteractable lightInteractableHit)
    {
        if (lastHit!=lightInteractableHit)
            CheckLastHit();
    }
    
    /// <summary>
    /// Reset the last LightInteractable hit, if there is one
    /// </summary>
    public virtual void CheckLastHit()
    {
        if (!lastHit.IsUnityNull())
        {
            lastHit.ResetInteractable();
        }
    }

    /// <summary>
    /// Forward recursive reset of the LightInteractable
    /// </summary>
    public virtual void ResetInteractable()
    {
        lr.enabled = false;
        
        if (lastHit.IsUnityNull()) return;
        
        var tempLastHit = lastHit;
        lastHit = null;
        tempLastHit.ResetInteractable();
    }
    
    /// <summary>
    /// Perform a ray cast from a given point, direction and AlchemyColor of the light
    /// </summary>
    /// <param name="rayPosition">The position to cast from</param>
    /// <param name="rayDirection">The direction to cast to</param>
    /// <param name="lightColor">The AlchemyColor of the light that is being cast</param>
    protected void ExecuteRayCast(Vector3 rayPosition, Vector3 rayDirection, AlchemyColor lightColor)
    {
        if (Physics.Raycast(rayPosition, rayDirection, out var newHit, maxRayDistance, LayerMask.GetMask("LightModifier")))
        {
            //Hit an interactable type...
            lr.SetPosition(1, newHit.point);
            var lightInteractableHit = newHit.transform.gameObject.GetComponent<LightInteractable>();
            CheckLastHit(lightInteractableHit);
            lastHit = lightInteractableHit;
            lightInteractableHit.OnLightHit(newHit, rayDirection, lightColor, this);
            return;

        }
        if (Physics.Raycast(rayPosition, rayDirection, out var newWallHit, maxRayDistance))
        {
            //Hit a wall...
            lr.SetPosition(1, newWallHit.point);
            CheckLastHit();
            return;
        }
        //Hit nothing...
        CheckLastHit();
        lr.SetPosition(1, rayPosition+rayDirection*30);
    }
}
