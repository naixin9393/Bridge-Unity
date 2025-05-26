
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class BiddingHintView: MonoBehaviour {
    [SerializeField] private UIDocument _document;
    [SerializeField] private VisualTreeAsset _biddingHintUXML;
    private GameViewModel _gameViewModel;
    private VisualElement _biddingHintInstance;
    private Button _closeButton;
    public void Initialize(GameViewModel gameViewModel) {
        _gameViewModel = gameViewModel;
        _biddingHintInstance = _biddingHintUXML.Instantiate();
        _biddingHintInstance.style.display = DisplayStyle.None;
        _document.rootVisualElement.Add(_biddingHintInstance);
        _biddingHintInstance.AddToClassList("popup-overlay");

        _closeButton = _biddingHintInstance.Q<Button>("CloseButton");
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
        
        var callLabel = _biddingHintInstance.Q<Label>("CallLabel");
        callLabel.dataSource = _gameViewModel.BiddingHintCall;
        callLabel.SetBinding(nameof(Label.text), new DataBinding {
            dataSourcePath = new PropertyPath(nameof(BindableProperty<string>.Value)),
            bindingMode = BindingMode.ToTarget
        });

        var explanationLabel = _biddingHintInstance.Q<Label>("ExplanationLabel");
        explanationLabel.dataSource = _gameViewModel.BiddingHintMessage;
        explanationLabel.SetBinding(nameof(Label.text), new DataBinding {
            dataSourcePath = new PropertyPath(nameof(BindableProperty<string>.Value)),
            bindingMode = BindingMode.ToTarget
        });
    }

    private void OnCloseButtonClicked(ClickEvent evt) {
        _biddingHintInstance.style.display = DisplayStyle.None;
    }
    
    public void Show() {
        _biddingHintInstance.style.display = DisplayStyle.Flex;
    }
}