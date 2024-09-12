using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;

    [SerializeField] private TextMeshProUGUI _MoveUPText;
    [SerializeField] private TextMeshProUGUI _MoveDownText;
    [SerializeField] private TextMeshProUGUI _MoveLeftText;
    [SerializeField] private TextMeshProUGUI _MoveRightText;
    [SerializeField] private TextMeshProUGUI _InteractText;
    [SerializeField] private TextMeshProUGUI _InteractAltText;
    [SerializeField] private TextMeshProUGUI _PauseText;

    [SerializeField] private Button _MoveUPButton;
    [SerializeField] private Button _MoveDownButton;
    [SerializeField] private Button _MoveLeftButton;
    [SerializeField] private Button _MoveRightButton;
    [SerializeField] private Button _InteractButton;
    [SerializeField] private Button _InteractAltButton;
    [SerializeField] private Button _PauseButton;

    [SerializeField] private Transform _PressedToRebindKeyTansform;

    private void Awake()
    {
        Instance = this;
        _soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        _MoveUPButton.onClick.AddListener(() => {RebindingBinding(InputManager.Binding.MoveUp);});
        _MoveDownButton.onClick.AddListener(() => { RebindingBinding(InputManager.Binding.MoveDown); });
        _MoveLeftButton.onClick.AddListener(() => { RebindingBinding(InputManager.Binding.MoveLeft); });
        _MoveRightButton.onClick.AddListener(() => { RebindingBinding(InputManager.Binding.MoveRight); });
        _InteractButton.onClick.AddListener(() => { RebindingBinding(InputManager.Binding.Interact); });
        _InteractAltButton.onClick.AddListener(() => { RebindingBinding(InputManager.Binding.InteractAlt); });
        _PauseButton.onClick.AddListener(() => { RebindingBinding(InputManager.Binding.Pause); });
    }
    private void Start()
    {
        GameManager.Instance.OnLocalGameUnpaused += GM_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void GM_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        _soundEffectsText.text = $"Sound effects: {Mathf.Round(SoundManager.Instance.GetVolume()*10)}";
        _musicText.text = $"Sound effects: {Mathf.Round(MusicManager.Instance.GetVolume() * 10)}";

        _MoveUPText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveUp);
        _MoveDownText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveDown);
        _MoveLeftText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveLeft);
        _MoveRightText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveRight);
        _InteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Interact);
        _InteractAltText.text = InputManager.Instance.GetBindingText(InputManager.Binding.InteractAlt);
        _PauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        _PressedToRebindKeyTansform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _PressedToRebindKeyTansform.gameObject.SetActive(false);
    }

    private void RebindingBinding(InputManager.Binding binding)
    {
        ShowPressToRebindKey();
        InputManager.Instance.RebindBinding(binding,()=> {
            HidePressToRebindKey();
            UpdateVisual();      
            });
    }

}
