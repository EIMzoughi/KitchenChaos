using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private const string PLAYER_PREFES_BINDING = "InputBindings";
    public static InputManager Instance {  get; private set; }

    public event EventHandler OnInteractAlternateAction;

    public event EventHandler OnInteractAction;

    public event EventHandler OnPauseAction;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        Interact,
        InteractAlt,
        Pause,
    }

    private PlayerInputAction playerInputAction;


    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFES_BINDING))
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFES_BINDING));
        }

        playerInputAction.Player.Enable();

        playerInputAction.Player.Interact.performed += InteractPerformed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;

        
    }

    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= InteractPerformed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
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

    public string GetBindingText(Binding binding)
    {
        switch(binding)
        {
            default:
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
                break;
            case Binding.InteractAlt:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
                break;
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
                break;
            case Binding.MoveUp:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
                break;
            case Binding.MoveDown:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
                break;
            case Binding.MoveLeft:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
                break;
            case Binding.MoveRight:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
                break;


        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound )
    {
        playerInputAction.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch(binding)
        {
            default:
            case Binding.MoveUp:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
        }


        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputAction.Player.Enable(); 
                onActionRebound();

                ;
                PlayerPrefs.SetString(PLAYER_PREFES_BINDING, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
    }
}
