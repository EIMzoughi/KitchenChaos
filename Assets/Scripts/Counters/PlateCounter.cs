using System;
using System.Collections;
using System.Collections.Generic;
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
        
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > 4f)
        {
            spawnPlateTimer = 0;

            if(plateSpawnAmount < plateSpawnAmountMax)
            {
                plateSpawnAmount++;
                onPlateSpawn?.Invoke(this,EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player)
    {
        if(!player.HaskitchenObject())
        {
            if(plateSpawnAmount>0)
            {
                plateSpawnAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                onPlateGrab?.Invoke(this,EventArgs.Empty);
            }
        }
    }
}
