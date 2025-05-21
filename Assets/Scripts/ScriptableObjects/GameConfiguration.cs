using UnityEngine;

public class GameConfiguration : ScriptableObject {
    [SerializeField] public bool BalancedHand = false;
    [SerializeField] private Vector2 HCPRange = new(0, 37);
    [SerializeField] public int MinHCP => (int)HCPRange.x;
    [SerializeField] public int MaxHCP => (int)HCPRange.y;
    [SerializeField] private const int MIN_HCP_LIMIT = 0;
    [SerializeField] private const int MAX_HCP_LIMIT = 37;
    [SerializeField] public Position HumanPlayerPosition = Position.South;
    [SerializeField] public Position DealerPosition = Position.South;

    private void OnEnable() {
        HCPRange = new Vector2(MIN_HCP_LIMIT, MAX_HCP_LIMIT);
    }
}
