using UnityEngine;

public class CardSpriteProvider {
    public static Sprite GetCardSprite(Card card) {
        string rank;
        if ((int)card.Rank > 10)
            rank = card.Rank.ToString();
        else
            rank = ((int)card.Rank).ToString();
        string textureName = rank.ToLower() + "_" + card.Suit.ToString().ToLower();
        return Resources.Load<Sprite>("Cards/" + textureName);
    }
}