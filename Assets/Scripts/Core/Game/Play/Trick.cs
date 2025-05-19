using System.Collections.Generic;
using System.Linq;

public class Trick : ITrick {
    private readonly List<(Card Card, IPlayer Player)> _plays = new();
    private readonly List<IPlayer> _players = new();
    private int _currentPlayerIndex;
    private readonly Strain _strain;

    public List<(Card Card, IPlayer Player)> Plays => _plays;
    public bool IsOver => _plays.Count == 4;
    public IPlayer CurrentPlayer => _players[_currentPlayerIndex];
    public IPlayer Winner => IsOver ? GetWinner() : null;
    public Suit LeadSuit { get; private set; }

    public Trick(List<IPlayer> players, Strain strain, IPlayer currentPlayer) {
        _players.AddRange(players);
        _strain = strain;
        _currentPlayerIndex = players.IndexOf(currentPlayer);
    }

    private IPlayer GetWinner() {
        LeadSuit = _plays[0].Card.Suit;
        return _plays.ToList()
            .OrderBy(play => play.Card, new BridgeCardComparer(LeadSuit, _strain)).Last().Player;
    }

    public void PlayCard(Card card, IPlayer player) {
        if (player != CurrentPlayer)
            throw new NotPlayersTurnException(player, CurrentPlayer);
        _plays.Add((card, player));
        player.PlayCard(card);
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
    }
}