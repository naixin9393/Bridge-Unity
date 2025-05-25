using UnityEngine;
using UnityEngine.UIElements;

public class SettingsView : MonoBehaviour {
    [SerializeField] private UIDocument _document;
    [SerializeField] private VisualTreeAsset _settingsPopupUXML;
    [SerializeField] private ScreenResolutionController _screenResolutionController;
    private Button _closeButton;
    private Button _1920x1080Button;
    private Button _1366x768Button;
    private TemplateContainer _popUpInstance;

    private void OnEnable() {
        _popUpInstance = _settingsPopupUXML.Instantiate();
        _document.rootVisualElement.Add(_popUpInstance);
        _popUpInstance.AddToClassList("popup-overlay");

        _popUpInstance.style.display = DisplayStyle.None;
        _closeButton = _popUpInstance.Q<Button>("CloseButton");
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);
        
        _1920x1080Button = _popUpInstance.Q<Button>("1920x1080Button");
        _1920x1080Button.RegisterCallback<ClickEvent>(On1920x1080ButtonClicked);

        _1366x768Button = _popUpInstance.Q<Button>("1366x768Button");
        _1366x768Button.RegisterCallback<ClickEvent>(On1366x768ButtonClicked);
    }

    private void On1366x768ButtonClicked(ClickEvent evt) {
        _screenResolutionController.ApplyResolutionSettings(1366, 768);
    }

    private void On1920x1080ButtonClicked(ClickEvent evt) {
        _screenResolutionController.ApplyResolutionSettings(1920, 1080);
    }

    private void OnCloseButtonClicked(ClickEvent evt) {
        _popUpInstance.style.display = DisplayStyle.None;
    }

    public void Show() {
        _popUpInstance.style.display = DisplayStyle.Flex;
    }
}