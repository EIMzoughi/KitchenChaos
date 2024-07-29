using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTranform();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HaskitchenObject();

    public NetworkObject GetNetworkObject();
}
