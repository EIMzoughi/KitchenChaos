using System;
using System.Collections;
using System.Collections.Generic;
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
        _kitchenObjectsSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArg
        {
            kitchenObjectSO = kitchenObjectSO
        });
        return true;
    }

    public List<SO_KitchenObject> GetSO_KitchensList()
    {
        return _kitchenObjectsSOList;
    }
}
