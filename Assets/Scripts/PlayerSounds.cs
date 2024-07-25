using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footStepTimer;
    private float footStepTimeMax=0.4f;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footStepTimer -= Time.deltaTime;
        if(footStepTimer < 0)
        {
            footStepTimer = footStepTimeMax;

            if(player.IsWalking())
            {
                SoundManager.Instance.PlayFootStepsSounds(transform.transform.position, 1f);
            }
            
                
        }
    }


}
