using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlateComeplteVisual : MonoBehaviour
{
    public struct KitchenObjectSO_GameObject
    {
        public SO_KitchenObject kitchenObjectSO;

    }

    [SerializeField] private PlateKitchenObject platekitchenObject;

    private void Start()
    {
        platekitchenObject.OnIngredientAdded += PlatekitchenObject_OnIngredientAdded;
    }

    private void PlatekitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArg e)
    {
        e.kitchenObjectSO
    }
}
