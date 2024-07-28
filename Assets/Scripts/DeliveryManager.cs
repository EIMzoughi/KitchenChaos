using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSucc;
    public event EventHandler OnRecipeFail;
    public static DeliveryManager Instance { get; private set; }
    public int SuccRecipesNum { get; private set; }

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
        if (!IsServer) return;
        _spawnRecipeTimer += Time.deltaTime;
        if(_spawnRecipeTimer > _spawnRecipeTimerMax )
        {
            _spawnRecipeTimer = 0;
            if(_waitingRecepiceSOList.Count < _waitingRecipeMaxNumber )
            {
                SpawnRecipe();
                
            }
        }
    }

    [ClientRpc(RequireOwnership = false)]
   private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        SO_Recipe waitingRecipeSO = _recipeListSO.recipeSOList[waitingRecipeSOIndex];
        _waitingRecepiceSOList.Add(waitingRecipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnRecipe() 
    {
        int waitingRecipeSOIndex = UnityEngine.Random.Range(0, _recipeListSO.recipeSOList.Count);
        
        SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int i)
    {
        DeliverCorrectRecipeClientRpc(i);
    }
    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int i)
    {
        _waitingRecepiceSOList.RemoveAt(i);
        SuccRecipesNum++;
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSucc?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc]
    private void DeliverWrongRecipeServerRpc()
    {
        DeliverWrongRecipeClientRpc();
    }
    [ClientRpc]
    private void DeliverWrongRecipeClientRpc()
    {
        //player delivered the Wrong recipe!
        Debug.Log("BOOOOOOOOOOOO");
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
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
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }
        DeliverWrongRecipeClientRpc();
    }

    public List<SO_Recipe> GetWaitingRecipeSOLIST()
    {
        return _waitingRecepiceSOList;
    }
}
