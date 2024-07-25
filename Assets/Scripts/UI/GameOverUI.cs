using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDeliveredNumberText;

    private void Start()
    {
        GameManager.Instance.OnStateChange += GM_OnStateChange;
        Hide();
    }

    
    private void GM_OnStateChange(object sender, System.EventArgs e)
    {
        if ((sender as GameManager).IsGameOver())
        {
            recipesDeliveredNumberText.text = Mathf.Ceil(DeliveryManager.Instance.SuccRecipesNum).ToString();
            Show();
        }
        else
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
