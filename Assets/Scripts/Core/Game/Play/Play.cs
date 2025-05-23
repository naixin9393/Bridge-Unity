using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Play : IPlay {
    internal List<ITrick> _tricks = new();
    internal int _currentTrickIndex;
    internal int _tricksWonByAttackers = 0;
    private readonly List<IPlayer> _players = new();
    private int _currentPlayerIndex;
    private List<IPlayer> _attackers = new();
    private List<IPlayer> _defenders = new();

    public ITrick CurrentTrick => _tricks[_currentTrickIndex];
    public ReadOnlyCollection<ITrick> Tricks => new(_tricks);
    public ReadOnlyCollection<IPlayer> Players => new(_players);
    public IPlayer LeadPlayer { get; private set; }
    public IPlayer CurrentPlayer => _players.Count > 0 ? _players[_currentPlayerIndex] : null;
    public int TricksWonByAttackers => _tricksWonByAttackers;
    public Bid Contract { get; private set; }
    public bool IsOver { get; private set; }
    public bool ContractIsMade => TricksWonByAttackers >= Contract.Level + 6;

    public Play(IAuction auction) {
        Contract = auction.FinalContract;
        _players = new List<IPlayer>(auction.Players); ;
        DetermineLeadPlayer(auction);
        DetermineTeams(LeadPlayer);
        _tricks.Add(new Trick(_players, Contract.Strain, CurrentPlayer));
    }

    public void PlayCard(Card card, IPlayer player) {
        if (IsOver)
            throw new GameOverException();
        if (player != CurrentPlayer)
            throw new NotPlayersTurnException(player, CurrentPlayer);
        if (DoesNotFollowLead(player, card))
            throw new NotFollowingLeadException(player, card.Suit);
        CurrentTrick.PlayCard(card, player);
        player.PlayCard(card);

        if (!CurrentTrick.IsOver) {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            return;
        }
        if (IsAttacker(CurrentTrick.Winner))
            _tricksWonByAttackers++;
        _currentPlayerIndex = _players.IndexOf(CurrentTrick.Winner);
        if (Tricks.Count == 13)
            EndGame();
    }

    private bool DoesNotFollowLead(IPlayer player, Card cardToPlay) {
        if (CurrentTrick.Plays.Count == 0) return false;
        var leadSuit = CurrentTrick.Plays[0].Card.Suit;
        if (leadSuit == cardToPlay.Suit) return false;
        return HasCardsOfSameSuit(player, leadSuit);
    }

    private bool HasCardsOfSameSuit(IPlayer player, Suit leadSuit) {
        return player.Hand.Any(card => card.Suit == leadSuit);
    }

    private void EndGame() {
        IsOver = true;
    }

    private void DetermineLeadPlayer(IAuction auction) {
        _currentPlayerIndex = (_players.IndexOf(auction.Declarer) + 1) % _players.Count;
        int leadPlayerIndex = _currentPlayerIndex;
        LeadPlayer = _players[leadPlayerIndex];
    }

    private void DetermineTeams(IPlayer leadPlayer) {
        _defenders = new List<IPlayer> {
            leadPlayer,
            PartnerOf(leadPlayer)
        };
        _attackers = _players.Where(player => !_defenders.Contains(player)).ToList();
    }

    private IPlayer PartnerOf(IPlayer player) {
        return _players[(_players.IndexOf(player) + 2) % _players.Count];
    }

    private bool IsAttacker(IPlayer player) {
        return _attackers.Contains(player);
    }
    
    public void StartNewTrick() {
        _tricks.Add(new Trick(_players, Contract.Strain, CurrentTrick.Winner));
        _currentTrickIndex++;
    }

    public void RequestPlayerPlayDecision() {
        List<Card> possibleCards = CalculatePossibleCards();
        CurrentPlayer.RequestPlayerPlayDecision(new PlayingContext(possibleCards, _tricks.ToList()));
    }

    private List<Card> CalculatePossibleCards() {
        if (CurrentTrick.Plays.Count == 0 || !HasCardsOfSameSuit(CurrentPlayer, CurrentTrick.Plays[0].Card.Suit)) return CurrentPlayer.Hand.ToList();
        return CurrentPlayer.Hand.Where(card => card.Suit == CurrentTrick.Plays[0].Card.Suit).ToList();
    }
}