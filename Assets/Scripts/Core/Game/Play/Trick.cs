using System;
using System.Collections.Generic;
using System.Linq;

public class Trick : ITrick {
    private readonly List<(Card Card, IPlayer Player)> _plays = new();
    private readonly List<IPlayer> _players = new();
    private readonly Strain _strain;

    public List<(Card Card, IPlayer Player)> Plays => _plays;
    public bool IsOver => _plays.Count == 4;
    public IPlayer CurrentPlayer { get; private set; }
    public IPlayer Winner => IsOver ? GetWinner() : null;
    public Suit? LeadSuit { get; private set; }

    public Trick(List<IPlayer> players, Strain strain, IPlayer currentPlayer) {
        _players.AddRange(players);
        _strain = strain;
        CurrentPlayer = currentPlayer;
    }

    private IPlayer GetWinner() {
        if (!LeadSuit.HasValue) throw new Exception("Cannot get winner when lead suit is not set");
        return _plays.ToList()
            .OrderBy(play => play.Card, new BridgeCardComparer(LeadSuit.Value, _strain)).Last().Player;
    }

    public void PlayCard(Card card, IPlayer player) {
        if (_plays.Count == 0) LeadSuit = card.Suit;
        if (player != CurrentPlayer)
            throw new NotPlayersTurnException(player, CurrentPlayer);
        _plays.Add((card, player));
        CurrentPlayer = PlayerUtils.GetNextPlayer(CurrentPlayer, _players);
    }
}