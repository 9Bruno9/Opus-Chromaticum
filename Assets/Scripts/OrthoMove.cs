using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrthoMove : MonoBehaviour
{

    [SerializeField] private InputAction move;
    [SerializeField] private float sensitivity = .15f;

    private void Start()
    {
        move = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        var m = move.ReadValue<Vector2>();
        m *= sensitivity;
        transform.position += new Vector3(m[0], 0f, m[1]);
    }
    
    
}
