using GameEventModule.Application;

public class GameOfferServiceAdapter : IGameOfferService
{
    private readonly GameOfferService offerService;
    private readonly GameOfferGroupService groupService;

    public GameOfferServiceAdapter(
        GameOfferService offerService,
        GameOfferGroupService groupService)
    {
        this.offerService = offerService;
        this.groupService = groupService;
    }

    public void ActivateOffer(string offerId)
    {
        if (string.IsNullOrEmpty(offerId))
            return;

        offerService.ActivateOffer(offerId);
    }

    public void ActivateGroup(string groupId)
    {
        if (string.IsNullOrEmpty(groupId))
            return;

        groupService.ActivateGroup(groupId);
    }
}