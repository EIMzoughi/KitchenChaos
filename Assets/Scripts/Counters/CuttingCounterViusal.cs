using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterViusal : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    const string CUT = "Cut";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        cuttingCounter.OnCutAnimation += CuttingCounter_onCuttingAnimation;
    }

    private void CuttingCounter_onCuttingAnimation(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
