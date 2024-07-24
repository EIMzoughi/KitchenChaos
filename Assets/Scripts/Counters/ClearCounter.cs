using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    

    [SerializeField] private SO_KitchenObject KitchenObjectSO;
    
     
    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            //there is no kitchenObject on the Counter
            if(player.HaskitchenObject())
            {
                //player is carrying something
                player.GetKitchenObject().KitchenObjectParent = this;
            }
            else
            {
                //player is not carrying something
            }
                    
        }
        else 
        {
            //the is kitchenObject on the counter 
            if (player.HaskitchenObject())
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player holding plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player is not carry plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //player holding plate
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //player isnt carrying something
                GetKitchenObject().KitchenObjectParent = player;
            }
            
        }               
    }


}
