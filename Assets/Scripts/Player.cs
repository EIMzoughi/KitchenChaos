using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System;
using Unity.Netcode;

public class Player : NetworkBehaviour, IKitchenObjectParent
{   
    //public static Player Instance {get;private set;}

    private void Awake()
    {
        /*if (Instance == null)
            Instance = this;      
        */
    }


    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter; 
    }

    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private Transform spawnPostion;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Start()
    {
        InputManager.Instance.OnInteractAction += InputManager_OnInteractAction;
        InputManager.Instance.OnInteractAlternateAction += InputManager_OnInteractAlternateAction;
    }

    private void InputManager_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternante(this);
        }
    }

    private void InputManager_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = InputManager.Instance.GetMovementVector().normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float playerRaduis = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDir, playerSpeed * Time.deltaTime);

        isWalking = moveDir != Vector3.zero;
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDirX, playerSpeed * Time.deltaTime);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = moveDir.z!=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDirZ, playerSpeed * Time.deltaTime);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }

            }


        }
        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * playerSpeed;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = InputManager.Instance.GetMovementVector().normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 1.5f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }  
            }
            else
            {
                SetSelectedCounter(null);
            }

        }
        else
        {
            SetSelectedCounter(null);
        }            
    }
    private void SetSelectedCounter(BaseCounter selecteCounter)
    {
        this.selectedCounter = selecteCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTranform()
    {
        return spawnPostion;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnPickSomething?.Invoke(this,EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HaskitchenObject()
    {
        return kitchenObject != null;
    }
}
