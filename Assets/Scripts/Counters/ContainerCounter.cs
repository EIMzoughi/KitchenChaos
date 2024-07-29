using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

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
                InteractLogicServerRpc();
            }
            
        }
        
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        onPlayerGrabObject?.Invoke(this, EventArgs.Empty);
    }

}
