using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour {
    private UIDocument _document;
    private Button _startButton;
    private Button _exitButton;
    private List<Button> _menuButtons = new();
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonHoverSound;
    [SerializeField] private AudioClip _buttonClickSound;

    public void Awake() {
        _document = GetComponent<UIDocument>();
        _startButton = _document.rootVisualElement.Q<Button>("StartButton");
        _exitButton = _document.rootVisualElement.Q<Button>("ExitButton");
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(_audioSource);
    }

    private void OnEnable() {
        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        foreach (var button in _menuButtons) {
            button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
            button.RegisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnDisable() {
        _startButton.UnregisterCallback<ClickEvent>(OnStartButtonClicked);
        _exitButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
        foreach (var button in _menuButtons) {
            button.UnregisterCallback<MouseEnterEvent>(OnButtonHover);
            button.UnregisterCallback<ClickEvent>(OnButtonClick);
        }
    }

    private void OnStartButtonClicked(ClickEvent evt) {
        SceneManager.LoadScene("GameConfiguration");
    }

    private void OnExitButtonClicked(ClickEvent evt) {
        Application.Quit();
    }

    private void OnButtonHover(MouseEnterEvent evt) {
        _audioSource.PlayOneShot(_buttonHoverSound);
    }

    private void OnButtonClick(ClickEvent evt) {
        _audioSource.PlayOneShot(_buttonClickSound);
    }
}
