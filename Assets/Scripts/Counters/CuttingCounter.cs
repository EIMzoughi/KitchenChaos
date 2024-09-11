using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IKitchenObjectParent, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler OnCutAnimation;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    

    [SerializeField] private SO_CuttingRecipe[] cutKitchenObjectSOArray;
    private int CuttingCount;
    public override void Interact(Player player)
    {
        if (player.HaskitchenObject() && !HaskitchenObject())
        {
            //player has kitchen Ojbect and Counter hasnt have kitchen object
            if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                //player has cuttable object
                //player Drop the cuttable object on the counter
                KitchenObject kitchenObject = player.GetKitchenObject();
                kitchenObject.KitchenObjectParent = this;
                InteractLogicPlaceObjectOnCounterServerRpc();
            }        
        }
        else if (HaskitchenObject() )
        {
            if(player.HaskitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestoryKitchenObject(player.GetKitchenObject());
                    }
                }               
            }
            else
            {
                GetKitchenObject().KitchenObjectParent = player;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {
        CuttingCount = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        { progressNormalized = 0f });
    }

    public override void InteractAlternante(Player player)
    {
        if(HaskitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();
            CuttingProgressDoneServerRpc();

        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        CuttingCount++;

        OnCutAnimation?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        { progressNormalized = (float)CuttingCount / CuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO()).numberOfCuttingRecuirment });

        
    }

    [ServerRpc(RequireOwnership = false)]
    private void CuttingProgressDoneServerRpc()
    {
        if (CuttingCount == CuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO()).numberOfCuttingRecuirment)
        {
            SO_KitchenObject outPutKichenObject = GetOutPutForInput(GetKitchenObject().GetKitchenObjectSO());
            KitchenObject.DestoryKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(outPutKichenObject, this);
        }
    }


    private SO_KitchenObject GetOutPutForInput(SO_KitchenObject inputKichenObjectSO)
    {
        foreach(SO_CuttingRecipe CR in cutKitchenObjectSOArray)
        {
            if(CR.input == inputKichenObjectSO)
            {
                return CR.output;
            }
        }
        return null;
    }

    private bool HasRecipeWithInput(SO_KitchenObject inputKichenObjectSO)
    {
        foreach (SO_CuttingRecipe CR in cutKitchenObjectSOArray)
        {
            if (CR.input == inputKichenObjectSO)
            {
                return true;
            }
        }
        return false;
    }

    private SO_CuttingRecipe CuttingRecipeSO(SO_KitchenObject inputKichenObjectSO)
    {
        foreach (SO_CuttingRecipe CR in cutKitchenObjectSOArray)
        {
            if (CR.input == inputKichenObjectSO)
            {
                return CR;
            }
        }
        return null;
    }
}
