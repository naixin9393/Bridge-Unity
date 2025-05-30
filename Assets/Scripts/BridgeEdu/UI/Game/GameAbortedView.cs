using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace BridgeEdu.UI.Game {
    public class GameAbortedView : MonoBehaviour {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _gameAbortedUXML;
        private TemplateContainer _gameAbortedInstance;
        private Button _mainMenuButton;

        public void Initialize() {
            _gameAbortedInstance = _gameAbortedUXML.Instantiate();
            _document.rootVisualElement.Add(_gameAbortedInstance);
            _gameAbortedInstance.AddToClassList("popup-overlay");

            _mainMenuButton = _gameAbortedInstance.Q<Button>("MainMenuButton");
            _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
        }

        private void OnMainMenuButtonClicked(ClickEvent evt) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}