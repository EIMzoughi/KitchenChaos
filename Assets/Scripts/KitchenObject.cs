using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenObject : MonoBehaviour
{
    
    [SerializeField] private SO_KitchenObject KitchenObjectOS;

    private IKitchenObjectParent kitchenObjectParent;


    public SO_KitchenObject GetKitchenObjectSO()
    {
        return KitchenObjectOS;
    }
    
    public IKitchenObjectParent KitchenObjectParent 
    {
        get => kitchenObjectParent;
        
        set
        {
            if (this.KitchenObjectParent != null)
                this.kitchenObjectParent.ClearKitchenObject();

            kitchenObjectParent = value;

            if (value.HaskitchenObject())
                Debug.Log("aleardy has nikomk");
            else
            {
                value.SetKitchenObject(this);

                transform.parent = value.GetKitchenObjectFollowTranform();
                transform.localPosition = Vector3.zero;
            }
            
        }
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject() ;
        Destroy(gameObject);
    }
    
    public static KitchenObject SpawnKitchenObject(SO_KitchenObject kitchenObject, IKitchenObjectParent kitchenObjectParent)
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObject.prefab);
        KitchenObject KO = KitchenObjectTransform.GetComponent<KitchenObject>();
        KO.KitchenObjectParent = kitchenObjectParent;

        return KO;
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
