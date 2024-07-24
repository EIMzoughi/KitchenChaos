using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private SO_RecipeList _recipeListSO;

    private List<SO_Recipe> _waitingRecepiceList;
    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax=4f;
    private float _waitingRecipeMaxNumber=4f;

    private void Awake()
    {
        Instance = this;

        _waitingRecepiceList = new List<SO_Recipe>();
    }
    private void Update()
    {
        _spawnRecipeTimer += Time.deltaTime;
        if(_spawnRecipeTimer > _spawnRecipeTimerMax )
        {
            _spawnRecipeTimer = 0;
            if(_waitingRecepiceList.Count < _waitingRecipeMaxNumber )
            {

                SpawnRecipe();
            }
        }
    }

    public void SpawnRecipe() 
    {
        SO_Recipe waitingRecipeSO = _recipeListSO.recipeSOList[Random.RandomRange(0, _recipeListSO.recipeSOList.Count)];
        _waitingRecepiceList.Add(waitingRecipeSO);
        Debug.Log(waitingRecipeSO.recipeName);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i =0; i < _waitingRecepiceList.Count;i++)
        {
            SO_Recipe waitingRecipeSO = _waitingRecepiceList[i];

            if(waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetSO_KitchensList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach(SO_KitchenObject recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)
                {
                    bool ingredientFound = false;
                    foreach (SO_KitchenObject plateKitchenObjectSO in plateKitchenObject.GetSO_KitchensList())
                    {
                        if(plateKitchenObjectSO == plateKitchenObjectSO)
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
                    _waitingRecepiceList.RemoveAt(i);
                    Debug.Log("Correct");
                    return;
                }
            }
        }
        Debug.Log("BOOOOOOOOOOOO");
    }
}
