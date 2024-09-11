using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;
using System;
using Unity.Netcode;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnstateChanged;
    public class OnStateChangedEventArgs:EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private SO_FryingRecipe[] sO_FryingRecipesArray;
    [SerializeField] private SO_BurningRecipe[] sO_BurningRecipesArray;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> BurningTimer = new NetworkVariable<float>(0f);
    private SO_FryingRecipe fryingRecipeSO;
    private SO_BurningRecipe burningRecipeSO;

    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged += FryingTtimer_OnValueChange;
        BurningTimer.OnValueChanged += BurningTimer_OnValueChange;
        state.OnValueChanged += State_OnValueChange;
    }

    private void FryingTtimer_OnValueChange(float previousValue, float newValue )
    {
        float fryingTimeMax = fryingRecipeSO != null ? fryingRecipeSO.fryingTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer.Value / fryingTimeMax
        });
    }
    private void BurningTimer_OnValueChange(float previousValue, float newValue)
    {
        float burningTimerMax = burningRecipeSO != null ? burningRecipeSO.buriningTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = BurningTimer.Value / burningTimerMax
        });
    }
    private void State_OnValueChange(State previousState, State newState)
    {
        OnstateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state.Value });
        if(state.Value == State.Burned || state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }


    private void Update()
    {      
        if(!IsServer) return;
        if(HaskitchenObject())
        {
            switch (state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer.Value += Time.deltaTime;
                    

                    if (fryingTimer.Value > fryingRecipeSO.fryingTimerMax)
                    {
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                        
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);                       
                        state.Value = State.Fried;
                        BurningTimer.Value = 0f;
                        SetBurningRecipeSOClientRpc(KitchenGameMultiplayer.instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO())); 
                    }
                    break;
                case State.Fried:
                    BurningTimer.Value += Time.deltaTime;
                  
                    Debug.Log("burning");
                    if (BurningTimer.Value > burningRecipeSO.buriningTimerMax)
                    {
                        Debug.Log("burned");
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                        
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state.Value = State.Burned;
                        
                    }
                    break;
                case State.Burned:
                    
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (player.HaskitchenObject() && !HaskitchenObject())
        {
            //player has kitchen Ojbect and Counter hasnt have kitchen object
            if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                //player has object can be fried
                //player Drop the cuttable object on the counter
                KitchenObject kitchenObject = player.GetKitchenObject();
                kitchenObject.KitchenObjectParent = this;

                InterractLogicPlaceObjectOnCounterServerRpc(
                    KitchenGameMultiplayer.instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO())
                    );

                
            }
        }
        else if (HaskitchenObject())
        {

            if (player.HaskitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        SetStateIdleServerRpc();

                    }
                }
            }
            else
            {
                GetKitchenObject().KitchenObjectParent = player;

                SetStateIdleServerRpc();
            }         
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    { state.Value = State.Idle; }

    [ServerRpc(RequireOwnership =false)]
    private void InterractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectIndex)
    {
        fryingTimer.Value = 0f;
        state.Value = State.Frying;
        SetFryingRecipeSOClientRpc(kitchenObjectIndex);
    }
    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectIndex)
    {
        SO_KitchenObject kitchenObjectso = KitchenGameMultiplayer.instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);

        fryingRecipeSO = FryingRecipeSO(kitchenObjectso);

    }
    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectIndex)
    {
        SO_KitchenObject kitchenObjectso = KitchenGameMultiplayer.instance.GetKitchenObjectSOFromIndex(kitchenObjectIndex);

        burningRecipeSO = BurningRecipeSO(kitchenObjectso);
    }

    private SO_KitchenObject GetOutPutForInput(SO_KitchenObject inputKichenObjectSO)
    {
        foreach(SO_FryingRecipe CR in sO_FryingRecipesArray)
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
        foreach (SO_FryingRecipe CR in sO_FryingRecipesArray)
        {
            if (CR.input == inputKichenObjectSO)
            {
                return true;
            }
        }
        return false;
    }

    private SO_FryingRecipe FryingRecipeSO(SO_KitchenObject inputKichenObjectSO)
    {
        foreach (SO_FryingRecipe FR in sO_FryingRecipesArray)
        {
            if (FR.input == inputKichenObjectSO)
            {
                return FR;
            }
        }
        return null;
    }

    private SO_BurningRecipe BurningRecipeSO(SO_KitchenObject inputKichenObjectSO)
    {
        foreach (SO_BurningRecipe BR in sO_BurningRecipesArray)
        {
            if (BR.input == inputKichenObjectSO)
            {
                return BR;
            }
        }
        return null;
    }
}
