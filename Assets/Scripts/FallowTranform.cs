using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowTranform : MonoBehaviour
{
    private Transform targetTransform;

    public void SetTargetTranform(Transform targetTransform)
    {
        this.targetTransform = targetTransform; 
    }

    private void LateUpdate()
    {
        if (this.targetTransform == null)
        {
            return;
        }
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
}
