using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace BridgeEdu.UI.HandConfiguration {
    public class HandConfigurationScreen : MonoBehaviour {
        [SerializeField] private GameConfiguration _handGeneratorConfig;
        private UIDocument _document;
        private Button _acceptButton;
        private Button _returnButton;
        private List<Button> _menuButtons = new();
        private AudioSource _audioSource;
        private IntegerField _minHCPField;
        [SerializeField] private AudioClip _buttonHoverSound;
        [SerializeField] private AudioClip _buttonClickSound;
        private IntegerField _maxHCPField;
        private IntegerField _pMinHCPField;
        private IntegerField _pMaxHCPField;

        private void Awake() {
            _document = GetComponent<UIDocument>();
            _acceptButton = _document.rootVisualElement.Q<Button>("AcceptButton");
            _returnButton = _document.rootVisualElement.Q<Button>("ReturnButton");
            _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
            _audioSource = GetComponent<AudioSource>();
            _minHCPField = _document.rootVisualElement.Q<IntegerField>("MinHCPField");
            _maxHCPField = _document.rootVisualElement.Q<IntegerField>("MaxHCPField");
            _pMinHCPField = _document.rootVisualElement.Q<IntegerField>("PMinHCPField");
            _pMaxHCPField = _document.rootVisualElement.Q<IntegerField>("PMaxHCPField");
        }

        private void OnEnable() {
            _acceptButton.RegisterCallback<ClickEvent>(OnGenerateButtonClicked);
            _returnButton.RegisterCallback<ClickEvent>(OnGoBackButtonClicked);
            foreach (var button in _menuButtons) {
                button.RegisterCallback<MouseEnterEvent>(OnButtonHover);
                button.RegisterCallback<ClickEvent>(OnButtonClick);
            }

            _minHCPField.RegisterValueChangedCallback(OnMinHCPChanged);
            _maxHCPField.RegisterValueChangedCallback(OnMaxHCPChanged);
            _pMinHCPField.RegisterValueChangedCallback(OnPMinHCPChanged);
            _pMaxHCPField.RegisterValueChangedCallback(OnPMaxHCPChanged);
        }

        private void OnPMaxHCPChanged(ChangeEvent<int> evt) {
            _pMaxHCPField.value = Math.Min(_pMaxHCPField.value, 37);
            _pMaxHCPField.value = Math.Max(_pMaxHCPField.value, 0);
        }

        private void OnPMinHCPChanged(ChangeEvent<int> evt) {
            _pMinHCPField.value = Math.Max(_pMinHCPField.value, 0);
            _pMinHCPField.value = Math.Min(_pMinHCPField.value, _pMaxHCPField.value);
        }

        private void OnMaxHCPChanged(ChangeEvent<int> evt) {
            _maxHCPField.value = Math.Min(_maxHCPField.value, 37);
            _maxHCPField.value = Math.Max(_maxHCPField.value, 0);
        }

        private void OnMinHCPChanged(ChangeEvent<int> evt) {
            _minHCPField.value = Math.Max(_minHCPField.value, 0);
            _minHCPField.value = Math.Min(_minHCPField.value, _maxHCPField.value);
        }

        private void OnDisable() {
            _acceptButton.UnregisterCallback<ClickEvent>(OnGenerateButtonClicked);
            _returnButton.UnregisterCallback<ClickEvent>(OnGoBackButtonClicked);

            foreach (var button in _menuButtons) {
                button.UnregisterCallback<MouseEnterEvent>(OnButtonHover);
                button.UnregisterCallback<ClickEvent>(OnButtonClick);
            }

            _minHCPField.UnregisterValueChangedCallback(OnMinHCPChanged);
            _maxHCPField.UnregisterValueChangedCallback(OnMaxHCPChanged);
            _pMinHCPField.UnregisterValueChangedCallback(OnPMinHCPChanged);
            _pMaxHCPField.UnregisterValueChangedCallback(OnPMaxHCPChanged);
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
}