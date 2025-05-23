using System;
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

    public List<ICall> Calls => _calls;

    private readonly List<ICall> _calls = new();

    public Auction(List<IPlayer> players, IPlayer dealer) {
        _players = players;
        _dealer = dealer;
        _currentPlayerIndex = players.IndexOf(dealer);
    }

    public void RequestPlayerCallDecision() {
        CurrentPlayer.RequestPlayerCallDecision(new AuctionContext(_highestBid, _calls));
    }

    public void MakeCall(ICall call) {
        if (call.Caller != CurrentPlayer)
            throw new InvalidCallException("Caller is not the current player");
        if (LastCall == null && call.Type != CallType.Pass && call.Type != CallType.Bid)
            throw new InvalidCallException("First call must be a bid or pass");
        if (IsPass(LastCall) && (IsDouble(call) || IsRedouble(call)))
            throw new InvalidCallException("Double and Redouble are not allowed after pass");
        if (IsDouble(LastCall) && IsDouble(call))
            throw new InvalidCallException("Double is not allowed after double");
        if (!IsDouble(LastCall) && IsRedouble(call))
            throw new InvalidCallException("Redouble is only allowed after double");
        if (_highestBid != null && IsBid(call) && IsEqualOrLowerThanHighest((call as BidCall).Bid))
            throw new InvalidCallException("Caller bid is equal or lower than the highest bid");

        _consecutivePasses = IsPass(call) ? _consecutivePasses + 1 : 0;

        _calls.Add(call);
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

        if (IsBid(call))
            _highestBid = call as BidCall;

        //OnCallMade?.Invoke(call);

        if (_consecutivePasses == 4 || (_consecutivePasses == 3 && _highestBid != null)) {
            EndAuction();
        }
    }

    private void EndAuction() {
        _isOver = true;
        DetermineDeclarer();
        DetermineDummy();
        //OnAuctionEnd?.Invoke();
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

    private bool IsEqualOrLowerThanHighest(Bid bid) {
        return !(bid > _highestBid.Bid);
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