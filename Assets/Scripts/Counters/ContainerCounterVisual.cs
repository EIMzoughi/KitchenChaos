using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    const string OPEN_CLOSE = "OpenClose";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        containerCounter.onPlayerGrabObject += ContainerCounter_onPlayerGrabObject;
    }

    private void ContainerCounter_onPlayerGrabObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
