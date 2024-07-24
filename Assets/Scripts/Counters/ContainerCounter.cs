using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    public event EventHandler onPlayerGrabObject;

    [SerializeField] private SO_KitchenObject KitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (!HaskitchenObject())
        {
            if(!player.HaskitchenObject())
            {
                //player doesnt have kitchen object!               
                KitchenObject.SpawnKitchenObject(KitchenObjectSO, player);
                onPlayerGrabObject?.Invoke(this, EventArgs.Empty);
            }
            
        }
        
    }

}
