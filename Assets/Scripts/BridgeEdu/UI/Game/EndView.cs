using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using BridgeEdu.Game;

namespace BridgeEdu.UI.Game {
    public class EndView : MonoBehaviour {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _endViewUXML;
        private TemplateContainer _endViewInstance;
        private Button _mainMenuButton;

        public void Initialize(GameViewModel gameViewModel) {
            _endViewInstance = _endViewUXML.Instantiate();
            _document.rootVisualElement.Add(_endViewInstance);
            _endViewInstance.AddToClassList("popup-overlay");

            AnimatePopup(_endViewInstance);

            _mainMenuButton = _endViewInstance.Q<Button>("MainMenuButton");
            _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);

            var wonTricksLabel = _endViewInstance.Q<Label>("WonTricksLabel");
            wonTricksLabel.text = gameViewModel.WonTricksCount.Value.ToString();

            var contractLabel = _endViewInstance.Q<Label>("ContractLabel");
            contractLabel.text = gameViewModel.Contract.ToString();

            var neededTricksLabel = _endViewInstance.Q<Label>("NeededTricksLabel");
            neededTricksLabel.text = gameViewModel.TricksNeededToWin.ToString();

            var playerWon = gameViewModel.WonTricksCount.Value - gameViewModel.TricksNeededToWin >= 0;
            var resultLabel = _endViewInstance.Q<Label>("ResultLabel");

            string resultText;
            if (playerWon) {
                resultText = gameViewModel.IsPlayerAttacker ?
                    "Has cumplido con el contrato" :
                    "Has defendido el contrato";
            }
            else {
                resultText = gameViewModel.IsPlayerAttacker ?
                    "Has fallado el contrato" :
                    "No has defendido el contrato";
            }
            resultLabel.text = resultText;
        }

        public void AnimatePopup(VisualElement element) {
            // Start from scale 0
            element.transform.scale = new Vector3(0f, 0f, 1f);

            // Use DOTween.To to animate scale (Vector2 or Vector3 works)
            Vector3 startScale = new Vector3(0f, 0f, 1f);
            Vector3 endScale = new Vector3(1f, 1f, 1f);

            DOTween.To(() => startScale, x => {
                startScale = x;
                element.transform.scale = x;
            }, endScale, 0.5f).SetEase(Ease.OutBack);
        }

        private void OnMainMenuButtonClicked(ClickEvent evt) {
            Debug.Log("Main menu button clicked");
            SceneManager.LoadScene("MainMenu");
        }
    }
}