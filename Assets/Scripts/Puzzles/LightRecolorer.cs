using ScriptableObjects;
using UnityEngine;

public class LightRecolorer : LightInteractable
{
    [Tooltip("The offset to the origin of the ray cast. MUST BE OUTSIDE OF THIS OBJECT TO PREVENT ERRORS")]
    [SerializeField] private float rayCastOffset = 0.15f;
    [SerializeField] private GameObject recolorObject;
    
    private void Start()
    {
        recolorObject.GetComponent<Renderer>().material.SetColor("_BaseColor", InteractableAlchemyColor.itemColor);
    }
    
    public override void OnLightHit(RaycastHit hit, Vector3 incomingDirection, AlchemyColor lightColor, LightInteractable source)
    {
        var outgoingLightColor = ColorMaster.Instance.MixLightRayColors(lightColor, interactableAlchemyColor);

        if (!outgoingLightColor) return;
        
        parent = source;
        lightCaster = source.LightCaster;
        lr.enabled = true;
        lr.positionCount = 2;
        lr.SetPosition(0, hit.point);
        lr.startColor = outgoingLightColor.itemColor;
        lr.endColor = outgoingLightColor.itemColor;


        var rayPosition = hit.point + incomingDirection * rayCastOffset;
        var rayDirection = incomingDirection;
        
        ExecuteRayCast(rayPosition, rayDirection, outgoingLightColor);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position-transform.forward, transform.position);
        Gizmos.color = interactableAlchemyColor.itemColor;
        Gizmos.DrawSphere(transform.position + Vector3.forward * rayCastOffset, 0.05f);
    }
}
