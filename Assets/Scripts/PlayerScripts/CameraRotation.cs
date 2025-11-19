using System.Collections;
using System.Collections.Generic;
using InventoryUI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    private Transform playerTransform;
    private Transform cameraTransform;

    private float rotationSpeed = 100f;
    private float verticalClampMin = -89f;
    private float verticalClampMax = 89f;

    private float currentVerticalRotation = 0f;

    InputAction lookAction;
    private InventoryUI.InventoryUIManager inventoryUIManager;

    public GameObject deathPanel;

    public bool cameraBlocked; // se true blocca il movimento di camera, es. quando dialoghi aperti 



    /// <summary>
    /// Change the sensitivity. This function should only be used by "SensitivitySetter" since changes made by calling
    /// this function are not kept between play sessions or scenes
    /// </summary>
    /// <param name="newSens">The new sensitivity</param>
    public void SetSensitivity(float newSens)
    {
        rotationSpeed = newSens;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform.parent;  // la telecamera � child del player
        cameraTransform = GetComponent<Transform>();
        lookAction = InputSystem.actions.FindAction("Look");
       Cursor.lockState = CursorLockMode.Locked;   
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (cameraBlocked)
            return;


        Vector2 lookValue = lookAction.ReadValue<Vector2>();

        // Rotazione orizzontale del player (Y)
        playerTransform.Rotate(Vector3.up * (lookValue.x * rotationSpeed * Time.deltaTime));

        // Rotazione verticale della camera (X)
        currentVerticalRotation -= lookValue.y * rotationSpeed * Time.deltaTime;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, verticalClampMin, verticalClampMax);
        cameraTransform.localRotation = Quaternion.Euler(currentVerticalRotation, 0f, 0f);
        
    }



    public void BlockCamera(bool blockingCamera)
    {
        cameraBlocked = blockingCamera;
    }



    public void CursorState(bool free)
    {
        if (!free)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameraBlocked = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cameraBlocked = true;
        }
    }
}
