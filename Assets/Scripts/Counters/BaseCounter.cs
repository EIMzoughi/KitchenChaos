using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform spawnPostion;

    private KitchenObject kitchenObject;
    public virtual void Interact(Player player){}
    public virtual void InteractAlternante(Player player) {}
    
    public Transform GetKitchenObjectFollowTranform()
    {
        return spawnPostion;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnAnyObjectPlacedHere?.Invoke(this,EventArgs.Empty);
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
