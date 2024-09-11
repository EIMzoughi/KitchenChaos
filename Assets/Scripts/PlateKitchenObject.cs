using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArg> OnIngredientAdded;
    public class OnIngredientAddedEventArg : EventArgs
    {
        public SO_KitchenObject kitchenObjectSO;
    }

    [SerializeField] private List<SO_KitchenObject> validKitchenObjectSOList;
    private List<SO_KitchenObject> _kitchenObjectsSOList;
    protected override void Awake()
    {
        base.Awake();
        _kitchenObjectsSOList = new List<SO_KitchenObject>();
    }
    public bool TryAddIngredient(SO_KitchenObject kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if(_kitchenObjectsSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        AddIngredientServerRpc(KitchenGameMultiplayer.instance.GetKitchenObjectSOIndex(kitchenObjectSO));
        return true;
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectIndex)
    {
        AddIngredientClientRpc(kitchenObjectIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectIndex)
    {
        SO_KitchenObject kitchenObjectSO = KitchenGameMultiplayer.instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);
        _kitchenObjectsSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArg
        {
            kitchenObjectSO = kitchenObjectSO
        });      
    }

    public List<SO_KitchenObject> GetSO_KitchensList()
    {
        return _kitchenObjectsSOList;
    }
}
