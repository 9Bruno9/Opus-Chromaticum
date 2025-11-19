using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMr : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public GameObject deathPanel;
    

    private CharacterController controller;
    private float verticalVelocity = 0f;

    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;

    private float yRotation = 0f;
    private float xRotation = 90f;

    public float verticalClampMin = -80f;
    public float verticalClampMax = 80f;
    private float currentVerticalRotation = 0f;

    public bool blockMovement = false; 



    public void EnableMovement(bool allowmovement)
    {
        blockMovement = allowmovement;
    }


    public void SetLookEnabled(bool isEnabled) //per blocccare la rotazione quando � aperto il men� radiale 
    {
        if (isEnabled)
            lookAction.Enable();
        else
            lookAction.Disable();
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        if(GameManager.instance.getPosition)
        {
            controller.enabled = false;
            transform.position = GameManager.instance.playerPosition;
            controller.enabled = true;
        }   
    }

    // Funzione per verificare se il player � a terra usando un raycast
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + 0.1f);    // Funzione per verificare se il player tocca il suolo: Lancia un raggio verso il basso, Se tocca qualcosa a distanza "poco sotto i piedi", il player � a terra
    }

    void Update()
    {

        if (blockMovement) return;

        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // Movimento (WASD)
        Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;
        controller.Move(move * (movementSpeed * Time.deltaTime));

       
        // Controllo di IsGrounded con raycast e debug
        bool isGrounded = IsGrounded();
        //Debug.Log("Is Grounded: " + isGrounded);

        // Gravit�
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -4f;

        // Salto 
        if (isGrounded && jumpAction.WasPressedThisFrame())
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aggiornamento della velocit� verticale (gravit�)
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * (verticalVelocity * Time.deltaTime));
    }
}
