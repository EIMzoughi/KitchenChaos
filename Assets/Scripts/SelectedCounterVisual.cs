using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        if(Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
            
        }
        else
        {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
        
    }
    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if(Player.LocalInstance!=null)
        {
            Player.LocalInstance.OnSelectedCounterChange -= Player_OnSelectedCounterChange;
            Player.LocalInstance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
        }
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeEventArgs e)
    {
        if(e.selectedCounter==baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject go in visualGameObjectArray)
        {
            go.SetActive(true);
        }

        }
    private void Hide()
    {
        foreach (GameObject go in visualGameObjectArray)
        {
            go.SetActive(false);
        }
    }
}
