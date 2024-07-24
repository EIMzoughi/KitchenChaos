using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlateComeplteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public SO_KitchenObject kitchenObjectSO;
        public GameObject gameObject;

    }

    [SerializeField] private PlateKitchenObject platekitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;

    private void Start()
    {
        platekitchenObject.OnIngredientAdded += PlatekitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameObject KG in kitchenObjectSO_GameObjectList)
        {
            KG.gameObject.SetActive(false);
        }
    }

    private void PlatekitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArg e)
    {
        foreach(KitchenObjectSO_GameObject KG in kitchenObjectSO_GameObjectList)
        {
            if(KG.kitchenObjectSO == e.kitchenObjectSO)
            {
                KG.gameObject.SetActive(true);
            }
        }
    }
}
