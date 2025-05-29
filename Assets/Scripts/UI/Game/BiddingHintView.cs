
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class BiddingHintView: MonoBehaviour {
    [SerializeField] private UIDocument _document;
    [SerializeField] private VisualTreeAsset _biddingHintUXML;
    private GameViewModel _gameViewModel;
    private VisualElement _biddingHintInstance;
    private Button _closeButton;
    private Label _callLabel;
    private Label _explanationLabel;

    public void Initialize(GameViewModel gameViewModel) {
        _gameViewModel = gameViewModel;
        _biddingHintInstance = _biddingHintUXML.Instantiate();
        _biddingHintInstance.style.display = DisplayStyle.None;
        _document.rootVisualElement.Add(_biddingHintInstance);
        _biddingHintInstance.AddToClassList("popup-overlay");

        _closeButton = _biddingHintInstance.Q<Button>("CloseButton");
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
        
        _callLabel = _biddingHintInstance.Q<Label>("CallLabel");
        _explanationLabel = _biddingHintInstance.Q<Label>("ExplanationLabel");
    }

    public void Set(string call, string message) {
        _callLabel.text = call;
        _explanationLabel.text = message;
    }

    private void OnCloseButtonClicked(ClickEvent evt) {
        _biddingHintInstance.style.display = DisplayStyle.None;
    }
    
    public void Show() {
        _biddingHintInstance.style.display = DisplayStyle.Flex;
    }
}