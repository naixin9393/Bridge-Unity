using UnityEngine;
using UnityEngine.UIElements;

namespace BridgeEdu.UI.Game.Play {
    public class PlayHintView : MonoBehaviour {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _playHintUXML;
        private VisualElement _playHintInstance;
        private Button _closeButton;
        private Label _cardLabel;
        private Label _messageLabel;

        public void Initialize() {
            _playHintInstance = _playHintUXML.Instantiate();
            _playHintInstance.style.display = DisplayStyle.None;
            _document.rootVisualElement.Add(_playHintInstance);
            _playHintInstance.AddToClassList("popup-overlay");

            _closeButton = _playHintInstance.Q<Button>("CloseButton");
            _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

            _cardLabel = _playHintInstance.Q<Label>("CardLabel");
            _messageLabel = _playHintInstance.Q<Label>("ExplanationLabel");
        }

        public void Set(string card, string message) {
            _messageLabel.text = message;
            _cardLabel.text = card;
        }

        private void OnCloseButtonClicked(ClickEvent evt) {
            _playHintInstance.style.display = DisplayStyle.None;
        }

        public void Show() {
            _playHintInstance.style.display = DisplayStyle.Flex;
        }
    }
}