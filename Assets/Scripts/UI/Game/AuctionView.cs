using UnityEngine;
using UnityEngine.UIElements;

public class AuctionView : MonoBehaviour {
    private UIDocument _document;
    private VisualElement _auctionHandContainer;
    void Awake() {

    }

    public void Initialize(UIDocument document) {
        _document = document;
        _auctionHandContainer = _document.rootVisualElement.Q<VisualElement>("AuctionHandContainer");
        var button = new Button();
        button.text = "Play";
        _auctionHandContainer.Add(button);
    }
}
