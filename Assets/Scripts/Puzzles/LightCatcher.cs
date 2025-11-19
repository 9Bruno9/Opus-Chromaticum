using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LightCatcher : LightInteractable
{
    [Tooltip("Event raised when the correct light hits this LightCatcher")]
    public UnityEvent<int> onCorrectLightHit;

    [SerializeField] private bool activateOnlyOnce;

    [SerializeField] private AudioSource audioFeedback;
    [SerializeField] private ParticleSystem visualFeedback;
    
    private bool _activated;
    public bool Activated => _activated;

    
    [SerializeField] private float degreesPerFrame = 0.3f;
    [SerializeField] private Transform catcherObject;

    [SerializeField] private int activationTimesBeforeShutdown = 1;
    
    private void Start()
    {
        var colorToSet = InteractableAlchemyColor.itemColor;
        colorToSet.a = 1f;
        catcherObject.GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", colorToSet);
        if (!visualFeedback.IsUnityNull())
        {
            visualFeedback.GetComponent<Renderer>().material.SetColor("_BaseColor", InteractableAlchemyColor.itemColor);
            visualFeedback.Stop();
        }
    }

    public override void OnLightHit(RaycastHit hit, Vector3 incomingDirection, AlchemyColor lightColor, LightInteractable source)
    {
        if (lightColor != interactableAlchemyColor) return;
        
        parent = source;
        lightCaster = source.LightCaster;
        // Add last bit of light ray to make it reach the center of the LightCatcher
        lr.enabled = true;
        lr.positionCount = 2;
        lr.SetPosition(0, hit.point);
        lr.startColor = lightColor.itemColor;
        lr.endColor = lightColor.itemColor;
        lr.SetPosition(1,transform.position);
            
        //source.Lock(); TODO: define function to lock puzzle in completed position
        if (activateOnlyOnce && _activated) return;
        _activated = true;

    /*
        if(!audioFeedback.IsUnityNull())
        {
            audioFeedback.Play();
        }
        if(!visualFeedback.IsUnityNull())
        {
            if (visualFeedback.isPlaying)
            {
                visualFeedback.Stop();
            }
            visualFeedback.Play();
        }*/
        onCorrectLightHit.Invoke(1);
    }

    public void SetActiveAlphaValue()
    {
        if (activationTimesBeforeShutdown > 1)
        {
            activationTimesBeforeShutdown--;
            return;
        }
        var colorToSet = InteractableAlchemyColor.itemColor;
        colorToSet.a = .2f;
        catcherObject.GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", colorToSet);
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        catcherObject.Rotate(Vector3.up, degreesPerFrame);
    }
}
