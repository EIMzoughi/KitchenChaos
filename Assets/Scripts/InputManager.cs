using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public event EventHandler OnInteractAlternateAction;

    public event EventHandler OnInteractAction;

    private PlayerInputAction playerInputAction;


    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += InteractPerformed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this,EventArgs.Empty);
    }

    private void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this,EventArgs.Empty);
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
}
