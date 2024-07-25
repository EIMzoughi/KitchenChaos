using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private SO_RecipeList _recipeListSO;

    private List<SO_Recipe> _waitingRecepiceSOList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax=4f;
    private float _waitingRecipeMaxNumber=4f;

    private void Awake()
    {
        Instance = this;

        _waitingRecepiceSOList = new List<SO_Recipe>();
    }
    private void Update()
    {
        _spawnRecipeTimer += Time.deltaTime;
        if(_spawnRecipeTimer > _spawnRecipeTimerMax )
        {
            _spawnRecipeTimer = 0;
            if(_waitingRecepiceSOList.Count < _waitingRecipeMaxNumber )
            {

                SpawnRecipe();
                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
            }
        }
    }

    public void SpawnRecipe() 
    {
        SO_Recipe waitingRecipeSO = _recipeListSO.recipeSOList[UnityEngine.Random.Range(0, _recipeListSO.recipeSOList.Count)];
        _waitingRecepiceSOList.Add(waitingRecipeSO);
        Debug.Log(waitingRecipeSO.recipeName);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i =0; i < _waitingRecepiceSOList.Count;i++)
        {
            SO_Recipe waitingRecipeSO = _waitingRecepiceSOList[i];

            if(waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetSO_KitchensList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach(SO_KitchenObject recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)
                {
                    bool ingredientFound = false;
                    foreach (SO_KitchenObject plateKitchenObjectSO in plateKitchenObject.GetSO_KitchensList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    //player delivered the Correct recipe!
                    _waitingRecepiceSOList.RemoveAt(i);
       
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        //player delivered the Wrong recipe!
        Debug.Log("BOOOOOOOOOOOO");
    }

    public List<SO_Recipe> GetWaitingRecipeSOLIST()
    {
        return _waitingRecepiceSOList;
    }
}
