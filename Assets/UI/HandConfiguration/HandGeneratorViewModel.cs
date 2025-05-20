using UnityEngine;

[CreateAssetMenu(fileName = "HandGenerator", menuName = "Scriptable Objects/HandGenerator")]
public class HandGeneratorViewModel : ScriptableObject {
    [SerializeField] private bool BalancedHand = false;
    [SerializeField] private Vector2 HCPRange = new(0, 37);
    [SerializeField] private int MinHCP => (int)HCPRange.x;
    [SerializeField] private int MaxHCP => (int)HCPRange.y;
    [SerializeField] private const int MIN_HCP_LIMIT = 0;
    [SerializeField] private const int MAX_HCP_LIMIT = 37;

    private void OnEnable() {
        HCPRange = new Vector2(MIN_HCP_LIMIT, MAX_HCP_LIMIT);
    }
}
