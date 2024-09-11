using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer instance;

    [SerializeField] private KitchenObjectListSO kitchenObjectListSO;
    private void Awake()
    {
        instance = this;
    }

    public void SpawnKitchenObject(SO_KitchenObject kitchenObject, IKitchenObjectParent kitchenObjectParent)
    {
      SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObject), kitchenObjectParent.GetNetworkObject());
    }
   
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectRef)
    {
        SO_KitchenObject kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        NetworkObject kichenObjectNetworkObject = KitchenObjectTransform.GetComponent<NetworkObject>();
        kichenObjectNetworkObject.Spawn(true);
        KitchenObject KO = KitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectRef.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        KO.KitchenObjectParent = kitchenObjectParent;
    }
    public  int GetKitchenObjectSOIndex(SO_KitchenObject kitchenObjectSO)
    {
        return kitchenObjectListSO.kitchenObjectsSOList.IndexOf(kitchenObjectSO);
    }
    public SO_KitchenObject GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
    {
        return kitchenObjectListSO.kitchenObjectsSOList[kitchenObjectSOIndex];
    }

    public void DestoryKitchenObject(KitchenObject kitchenObject)
    {
        DestoryKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestoryKitchenObjectServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectRef)
    {
        kitchenObjectParentNetworkObjectRef.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();
        ClearKitchenObjectOnparentClientRpc(kitchenObjectParentNetworkObjectRef);
        kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnparentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectRef)
    {
        kitchenObjectParentNetworkObjectRef.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectOnParent();
    }
}
