
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingToStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] protected Image button;

    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
    }

    private void GameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.IsLocalPlayerReady())
        {
            text.text = "Waiting For The Players";
            button.color = Color.green;
            
        }
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
