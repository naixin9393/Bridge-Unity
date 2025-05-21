public class AuctionController {
    private readonly IAuction _auction;
    public AuctionController(AuctionView auctionView, IAuction auction) {

    }
    public void StartAuction() => _auction.RequestPlayerCallDecision();
}