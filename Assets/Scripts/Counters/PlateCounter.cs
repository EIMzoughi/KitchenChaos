using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class PlateCounter : BaseCounter
{
    public event EventHandler onPlateSpawn;
    public event EventHandler onPlateGrab;

    [SerializeField] private SO_KitchenObject plateKitchenObjectSO;

    private float spawnPlateTimer;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;
    private void Update()
    {
        if(!IsServer) return;

        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > 4f)
        {
            spawnPlateTimer = 0;

            if(plateSpawnAmount < plateSpawnAmountMax)
            {
                SpawnPlateServerRpc();
            }
        }
    }

    [ServerRpc]
    private void SpawnPlateServerRpc() 
    {
        SpawnPlateClientRpc();
    }
    [ClientRpc]
    private void SpawnPlateClientRpc() 
    {
        plateSpawnAmount++;
        onPlateSpawn?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if(!player.HaskitchenObject())
        {
            if(plateSpawnAmount>0)
            {
                
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
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
        plateSpawnAmount--;
        onPlateGrab?.Invoke(this, EventArgs.Empty);
    }
}
