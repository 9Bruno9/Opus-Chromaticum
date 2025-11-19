using System;
using ScriptableObjects;
using UnityEngine;

public class LightMirror : LightInteractable
{
    private enum MovementType
    {
        TogglePositive90,
        ToggleNegative90,
        Rotate90
    }

    [SerializeField] private MovementType movementType;
    [SerializeField] private Transform mirrorObject;

    private bool _toggled;
    
    private void Start()
    {
        mirrorObject.GetComponent<Renderer>().material.SetColor("_BaseColor", InteractableAlchemyColor.itemColor);
    }

    public override void OnLightHit(RaycastHit hit, Vector3 incomingDirection, AlchemyColor lightColor, LightInteractable source)
    {
        parent = source;
        lightCaster = source.LightCaster;
        
        if (!CalculateReflectedLight(lightColor, out var outColor))
        {
            //ResetInteractable();
            return;
        }
        if (Vector3.Dot(incomingDirection, Vector3.Reflect(incomingDirection, hit.normal)) < -0.5)
        {
            // The reflection makes an angle that is too tight, in this case we imagine this mirror to be a wall,
            // done to avoid excessive light rebounds
            //ResetInteractable();
            return;
        }

        lr.enabled = true;
        lr.positionCount = 2;
        lr.SetPosition(0, hit.point);
        lr.startColor = outColor.itemColor;
        lr.endColor = outColor.itemColor;

        
        var rayPosition = hit.point;
        var rayDirection = Vector3.Reflect(incomingDirection, hit.normal);
        // TODO: add reflected ray rounding to prevent inconsistent reflections (es. Mathf.round(Vector3.Reflect(...)/360)*360)
        
        ExecuteRayCast(rayPosition, rayDirection, outColor);
    }

    [ContextMenu("Interact")]
    public override void OnInteract()
    {
        switch (movementType)
        {
            case MovementType.TogglePositive90:
                mirrorObject.Rotate(Vector3.up, _toggled ? -90f : 90f, Space.World);
                _toggled = !_toggled;
                break;
            case MovementType.ToggleNegative90:
                mirrorObject.Rotate(Vector3.up, _toggled ? 90f : -90f, Space.World);
                _toggled = !_toggled;
                break;
            case MovementType.Rotate90:
                mirrorObject.Rotate(Vector3.up, 90f, Space.World);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        base.OnInteract();
    }

    /// <summary>
    /// Perform a check to identify the reflected AlchemyColor based on the incoming light AlchemyColor and the AlchemyColor of the mirror
    /// </summary>
    /// <param name="lightColor">Incoming AlchemyColor</param>
    /// <param name="reflectedLightAlchemyColor">Outgoing AlchemyColor</param>
    /// <returns>True if the mirror will reflect something, false otherwise</returns>
    /// <exception cref="ArgumentOutOfRangeException">The AlchemyColor.ColorTypeEnum of the light is not handled</exception>
    private bool CalculateReflectedLight(AlchemyColor lightColor, out AlchemyColor reflectedLightAlchemyColor)
    {
        if (interactableAlchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Complex)
        {
            // A complex color mirror will always reflect
            reflectedLightAlchemyColor = lightColor;
            return true;
        }
        
        switch (lightColor.colorType.colorTypeEnum)
        {
            case AlchemyColor.ColorTypeEnum.Primary:
                if (interactableAlchemyColor.GetComponenti().Contains(lightColor))
                {
                    reflectedLightAlchemyColor = lightColor;
                    return true;
                }
                reflectedLightAlchemyColor = null;
                return false;
            case AlchemyColor.ColorTypeEnum.Secondary:
                if (interactableAlchemyColor.colorType.colorTypeEnum == AlchemyColor.ColorTypeEnum.Secondary)
                {
                    if (interactableAlchemyColor == lightColor)
                    {
                        reflectedLightAlchemyColor = interactableAlchemyColor;
                        return true;
                    }
                    // The two AlchemyColor.ColorTypeEnum.Secondary AlchemyColors do not match. Look for a common component...
                    var matching = lightColor.GetComponenti().Find(x => interactableAlchemyColor.GetComponenti().Contains(x));
                    reflectedLightAlchemyColor = matching;
                    return matching is not null;
                }
                if (lightColor.GetComponenti().Contains(interactableAlchemyColor))
                {
                    reflectedLightAlchemyColor = interactableAlchemyColor;
                    return true;
                }
                reflectedLightAlchemyColor = null;
                return false;
            case AlchemyColor.ColorTypeEnum.Complex:
                reflectedLightAlchemyColor = interactableAlchemyColor;
                return true;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
