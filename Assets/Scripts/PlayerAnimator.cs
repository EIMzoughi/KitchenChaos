using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator m_Animator;

    [SerializeField] private Player m_Player;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        m_Animator.SetBool("IsWalking", m_Player.IsWalking());
    }
}
