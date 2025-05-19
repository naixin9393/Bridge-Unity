using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Play : IPlay {
    internal List<ITrick> _tricks = new();
    internal int _currentTrickIndex;
    private readonly List<IPlayer> _players = new();
    private int _currentPlayerIndex;
    internal int _tricksWonByAttackers = 0;
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
        CurrentTrick.PlayCard(card, player);

        if (!CurrentTrick.IsOver) {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            return;
        }

        if (Tricks.Count < 13) {
            StartNewTrick();
            return;
        }
        EndGame();
    }

    private void EndGame() {
        IsOver = true;
    }

    private void StartNewTrick() {
        if (IsAttacker(CurrentTrick.Winner))
            _tricksWonByAttackers++;
        _currentPlayerIndex = _players.IndexOf(CurrentTrick.Winner);
        _tricks.Add(new Trick(_players, Contract.Strain, CurrentTrick.Winner));
        _currentTrickIndex++;
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
}