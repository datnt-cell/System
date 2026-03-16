using System;
using System.Collections.Generic;
using GameOfferSystem.Domain;

public class GameOfferFacadeService
{
    private readonly GameOfferService _offerService;
    private readonly GameOfferGroupService _groupService;

    public GameOfferFacadeService(
        GameOfferService offerService,
        GameOfferGroupService groupService)
    {
        _offerService = offerService;
        _groupService = groupService;
    }

    // =========================================================
    // OFFER
    // =========================================================

    public void ActivateOffer(string offerId)
    {
        _offerService.ActivateOffer(offerId);
    }

    public void MarkOfferSeen(string offerId)
    {
        _offerService.MarkSeen(offerId);
    }

    public OfferPurchaseError CanPurchaseOffer(string offerId)
    {
        return _offerService.CanPurchase(offerId);
    }

    public PurchaseOfferResponse PurchaseOffer(string offerId)
    {
        return _offerService.Purchase(offerId);
    }

    public List<GameOfferRuntimeData> GetActiveOffers()
    {
        return _offerService.GetActiveOffers();
    }

    public bool IsOfferActive(string offerId)
    {
        return _offerService.CanPurchase(offerId) != OfferPurchaseError.OfferNotActive;
    }

    public int GetOfferRemainingTime(string offerId)
    {
        return _offerService.GetRemainingTime(offerId);
    }

    // =========================================================
    // GROUP
    // =========================================================

    public GameOfferGroupRuntimeData ActivateGroup(string groupId)
    {
        return _groupService.ActivateGroup(groupId);
    }

    public void MarkGroupSeen(string groupId)
    {
        _groupService.MarkSeen(groupId);
    }

    public string GetAvailableOfferInGroup(string groupId)
    {
        return _groupService.GetAvailableOffer(groupId);
    }

    public OfferPurchaseError CanPurchaseGroupOffer(string groupId, string offerId)
    {
        return _groupService.CanPurchase(groupId, offerId);
    }

    public PurchaseOfferGroupResponse PurchaseGroupOffer(string groupId, string offerId)
    {
        return _groupService.Purchase(groupId, offerId);
    }

    public bool IsGroupActive(string groupId)
    {
        return !string.IsNullOrEmpty(_groupService.GetAvailableOffer(groupId));
    }

    public bool HasOfferInGroup(string groupId)
    {
        return _groupService.GetAvailableOffer(groupId) != null;
    }

    public int GetGroupRemainingTime(string offerId)
    {
        return _offerService.GetRemainingTime(offerId);
    }
}