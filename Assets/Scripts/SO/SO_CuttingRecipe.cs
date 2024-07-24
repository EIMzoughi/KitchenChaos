using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_CuttingRecipe : ScriptableObject
{
    public SO_KitchenObject input;
    public SO_KitchenObject output;
    public int numberOfCuttingRecuirment;
}
