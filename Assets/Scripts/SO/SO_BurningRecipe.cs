using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_BurningRecipe : ScriptableObject
{
    public SO_KitchenObject input;
    public SO_KitchenObject output;
    public float buriningTimerMax;
}
