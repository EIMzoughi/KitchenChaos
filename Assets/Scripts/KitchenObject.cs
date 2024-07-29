using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class KitchenObject : NetworkBehaviour
{
    
    [SerializeField] private SO_KitchenObject KitchenObjectOS;

    private IKitchenObjectParent kitchenObjectParent;
    private FallowTranform followTranform;
    protected virtual void Awake()
    {
        followTranform = GetComponent<FallowTranform>();
    }
    public SO_KitchenObject GetKitchenObjectSO()
    {
        return KitchenObjectOS;
    }
    
    public IKitchenObjectParent KitchenObjectParent 
    {
        get => kitchenObjectParent;
        
        set
        {
            SetKitchenObjectParentServerRpc(value.GetNetworkObject());
            
        }
    }
    [ServerRpc(RequireOwnership =false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectRef)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectRef);
    }
    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectRef)
    {
        kitchenObjectParentNetworkObjectRef.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (this.kitchenObjectParent != null)
            this.kitchenObjectParent.ClearKitchenObject();

        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HaskitchenObject())
            Debug.Log("aleardy has nikomk");

        kitchenObjectParent.SetKitchenObject(this);
            followTranform.SetTargetTranform(kitchenObjectParent.GetKitchenObjectFollowTranform());
        
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject() ;
        Destroy(gameObject);
    }
    
    public static void SpawnKitchenObject(SO_KitchenObject kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);    
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject =null;
            return false;
        }
            
    }
}
