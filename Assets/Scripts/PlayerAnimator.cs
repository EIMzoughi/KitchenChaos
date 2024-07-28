using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private Animator m_Animator;

    [SerializeField] private Player m_Player;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if(!IsOwner) return;
        m_Animator.SetBool("IsWalking", m_Player.IsWalking());
    }
}
