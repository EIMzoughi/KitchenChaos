using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionUI.Instance.Show();
        });
    }
    private void Start()
    {
        GameManager.Instance.OnGamePaused += GM_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GM_OnGameUnpaused;
        Hide();
    }

    private void GM_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GM_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
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
