using UnityEngine;

public class CardUI : MonoBehaviour {
    private SpriteRenderer _cardImage;
    public void Initialize(Card card) {
        _cardImage = GetComponent<SpriteRenderer>();
        _cardImage.sprite = CardSpriteProvider.GetCardSprite(card);
    }
}