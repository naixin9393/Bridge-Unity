using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace BridgeEdu.UI.Game {
    public class GameSettingsView : MonoBehaviour {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _gameSettingsUXML;
        private VisualElement _gameSettingsInstance;
        private Button _closeButton;
        private Button _mainMenuButton;

        public void Initialize() {
            _gameSettingsInstance = _gameSettingsUXML.Instantiate();
            _gameSettingsInstance.style.display = DisplayStyle.None;
            _document.rootVisualElement.Add(_gameSettingsInstance);
            _gameSettingsInstance.AddToClassList("popup-overlay");

            _closeButton = _document.rootVisualElement.Q<Button>("CloseButton");
            _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

            _mainMenuButton = _document.rootVisualElement.Q<Button>("MainMenuButton");
            _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
        }

        private void OnMainMenuButtonClicked(ClickEvent evt) {
            SceneManager.LoadScene("MainMenu");
        }

        private void OnCloseButtonClicked(ClickEvent evt) {
            _gameSettingsInstance.style.display = DisplayStyle.None;
        }

        public void Show() {
            _gameSettingsInstance.style.display = DisplayStyle.Flex;
        }
    }
}