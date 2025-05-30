using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BridgeEdu.UI.Game.Play {
    public class PlayHintView : MonoBehaviour {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _playHintUXML;
        private VisualElement _playHintInstance;
        private Button _closeButton;
        private Label _cardLabel;
        private VisualElement _hintContainer;

        public void Initialize() {
            _playHintInstance = _playHintUXML.Instantiate();
            _playHintInstance.style.display = DisplayStyle.None;
            _document.rootVisualElement.Add(_playHintInstance);
            _playHintInstance.AddToClassList("popup-overlay");

            _closeButton = _playHintInstance.Q<Button>("CloseButton");
            _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

            _cardLabel = _playHintInstance.Q<Label>("CardLabel");
            _hintContainer = _playHintInstance.Q<VisualElement>("HintContainer");

            _cardLabel.text = string.Empty;
        }

        public void Set(List<(string card, string message)> suggestions) {
            if (suggestions == null || suggestions.Count == 0) return;
            _hintContainer.Clear();
            _cardLabel.text = suggestions[0].card;
            foreach (var suggestion in suggestions) {
                VisualElement hintItem = CreateHintItem(suggestion.card, suggestion.message);
                _hintContainer.Add(hintItem);
            }
        }

        public VisualElement CreateHintItem(string card, string message) {
            VisualElement hintItem = new VisualElement();
            hintItem.AddToClassList("hint-item");

            Label cardLabel = new(card);
            cardLabel.AddToClassList("text30");
            hintItem.Add(cardLabel);

            Label messageLabel = new(message);
            messageLabel.AddToClassList("text30");
            hintItem.Add(messageLabel);

            return hintItem;
        }

        private void OnCloseButtonClicked(ClickEvent evt) {
            _playHintInstance.style.display = DisplayStyle.None;
        }

        public void Show() {
            _playHintInstance.style.display = DisplayStyle.Flex;
        }
    }
}