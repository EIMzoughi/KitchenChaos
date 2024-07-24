using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_Recipe : ScriptableObject
{
    public List<SO_KitchenObject> kitchenObjectsSOList;
    public string recipeName;
}
