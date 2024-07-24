using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_FryingRecipe : ScriptableObject
{
    public SO_KitchenObject input;
    public SO_KitchenObject output;
    public float fryingTimerMax;
}
