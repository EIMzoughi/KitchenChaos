using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countDownText;

    private void Start()
    {
        GameManager.Instance.OnStateChange += GM_OnStateChange;
        Hide();
    }

    private void Update()
    {
        _countDownText.text = Mathf.Ceil(GameManager.Instance.GetCountDownTostartTimer()).ToString();
    }
    private void GM_OnStateChange(object sender , System.EventArgs e)
    {
        if((sender as GameManager).IsCountDownToStartActive())
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
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
