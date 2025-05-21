using System.Collections.Generic;
using System.Linq;

public class Auction : IAuction {

    private List<IPlayer> _players;
    public List<IPlayer> Players => _players;
    private IPlayer _declarer;
    public IPlayer Declarer => _declarer;
    public ICall LastCall => _calls.Count > 0 ? _calls[^1] : null;
    private Bid _finalContract;
    public Bid FinalContract => _finalContract;
    public IPlayer CurrentPlayer => _players[_currentPlayerIndex];
    private IPlayer _dummy;
    public IPlayer Dummy => _dummy;

    private readonly IPlayer _dealer;
    public IPlayer Dealer => _dealer;

    private int _consecutivePasses;
    private int _currentPlayerIndex;
    private bool _isOver = false;
    public bool IsOver => _isOver;
    private BidCall _highestBid;
    private readonly List<IPlayer> _offendingSide = new();

    public BidCall HighestBid => _highestBid;
    private readonly List<ICall> _calls = new();

    public Auction(List<IPlayer> players, IPlayer dealer) {
        _players = players;
        _dealer = dealer;
        _currentPlayerIndex = players.IndexOf(dealer);
    }

    public void StartAuction() {
        CurrentPlayer.RequestPlayerCallDecision();
    }

    internal void MakeCall(ICall call) {
        if (call.Caller != CurrentPlayer)
            throw new InvalidCallException("Caller is not the current player");
        if (LastCall == null && call.Type != CallType.Pass && call.Type != CallType.Bid)
            throw new InvalidCallException("First call must be a bid or pass");
        if (IsPass(LastCall) && (IsDouble(call) || IsRedouble(call)))
            throw new InvalidCallException("Double and Redouble are not allowed after pass");
        if (!IsDouble(LastCall) && IsRedouble(call))
            throw new InvalidCallException("Redouble is only allowed after double");
        if (IsBid(LastCall) && IsBid(call) && IsLowerThanLastCall((call as BidCall).Bid))
            throw new InvalidCallException("Caller bid is lower than last bid");

        _consecutivePasses = IsPass(call) ? _consecutivePasses + 1 : 0;

        if (_consecutivePasses == 4 || (_consecutivePasses == 3 && _highestBid != null))
            _isOver = true;
        EndAuction();

        if (IsBid(call))
            _highestBid = call as BidCall;

        _calls.Add(call);
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
    }

    private void EndAuction() {
        DetermineDeclarer();
        DetermineDummy();
    }

    private void DetermineDeclarer() {
        if (_highestBid == null) return;
        _finalContract = _highestBid.Bid;

        Strain winningStrain = _highestBid.Bid.Strain;
        var winningCaller = _highestBid.Caller;
        var partner = PartnerOf(winningCaller);

        _offendingSide.Add(partner);
        _offendingSide.Add(winningCaller);

        _declarer = _calls
            .OfType<BidCall>()
            .Where(bidCall => bidCall.Bid.Strain == winningStrain && _offendingSide.Contains(bidCall.Caller))
            .FirstOrDefault()
            .Caller;
    }

    private void DetermineDummy() {
        _dummy = PartnerOf(_declarer);
    }

    private IPlayer PartnerOf(IPlayer player) {
        return _players[(_players.IndexOf(player) + 2) % _players.Count];
    }

    private bool IsLowerThanLastCall(Bid bid) {
        if (LastCall == null || LastCall.Type != CallType.Bid) return false;
        return bid < (LastCall as BidCall).Bid;
    }

    private bool IsBid(ICall call) {
        return call != null && call.Type == CallType.Bid;
    }

    private bool IsPass(ICall call) {
        return call != null && call.Type == CallType.Pass;
    }

    private bool IsDouble(ICall call) {
        return call != null && call.Type == CallType.Double;
    }
    private bool IsRedouble(ICall call) {
        return call != null && call.Type == CallType.Redouble;
    }
}