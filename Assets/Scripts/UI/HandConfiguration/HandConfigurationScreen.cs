using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HandConfigurationScreen : MonoBehaviour {
    [SerializeField] private GameConfiguration _handGeneratorConfig;
    private UIDocument _document;
    private Button _acceptButton;
    private Button _returnButton;
    private List<Button> _menuButtons = new();
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonHoverSound;
    [SerializeField] private AudioClip _buttonClickSound;

    private void Awake() {
        _document = GetComponent<UIDocument>();
        _acceptButton = _document.rootVisualElement.Q<Button>("AcceptButton");
        _returnButton = _document.rootVisualElement.Q<Button>("ReturnButton");
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        _acceptButton.RegisterCallback<ClickEvent>(OnGenerateButtonClicked);
        _returnButton.RegisterCallback<ClickEvent>(OnGoBackButtonClicked);
        foreach (var button in _menuButtons) {
            button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
            button.RegisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnDisable() {
        _acceptButton.UnregisterCallback<ClickEvent>(OnGenerateButtonClicked);
        _returnButton.UnregisterCallback<ClickEvent>(OnGoBackButtonClicked);
    }

    private void OnGenerateButtonClicked(ClickEvent evt) {
        SceneManager.LoadScene("Game");
    }

    private void OnGoBackButtonClicked(ClickEvent evt) {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnButtonHover(MouseEnterEvent evt) {
        _audioSource.PlayOneShot(_buttonHoverSound);
    }

    private void OnButtonClick(ClickEvent evt) {
        _audioSource.PlayOneShot(_buttonClickSound);
    }
}
