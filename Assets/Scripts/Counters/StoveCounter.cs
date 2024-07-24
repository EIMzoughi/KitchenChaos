using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;
using System;

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

    private State state;
    private float fryingTimer;
    private float BurningTimer;
    private SO_FryingRecipe fryingRecipeSO;
    private SO_BurningRecipe burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {      
        if(HaskitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {                      
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burningRecipeSO = BurningRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Fried;
                        BurningTimer = 0f;
                        OnstateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:
                    BurningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = BurningTimer / burningRecipeSO.buriningTimerMax
                    });
                    Debug.Log("burning");
                    if (BurningTimer > burningRecipeSO.buriningTimerMax)
                    {
                        Debug.Log("burned");
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;
                        OnstateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Burned:
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0f
                    });
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
                player.GetKitchenObject().KitchenObjectParent = this;
                fryingRecipeSO = FryingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
                fryingTimer = 0f;
                state = State.Frying;
                OnstateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                });
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

                        state = State.Idle;
                        OnstateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                GetKitchenObject().KitchenObjectParent = player;

                state = State.Idle;
                OnstateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
           
        }
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
