using GameEventModule.Domain;

namespace GameEventModule.Application
{
    public class GameEventAttachmentExecutor : IGameEventAttachmentExecutor
    {
        private readonly IGameOfferService offerService;
        private readonly ICurrencyService currencyService;
        private readonly IPopupService popupService;
        private readonly IMissionService missionService;

        public GameEventAttachmentExecutor(
            IGameOfferService offerService,
            ICurrencyService currencyService,
            IPopupService popupService,
            IMissionService missionService)
        {
            this.offerService = offerService;
            this.currencyService = currencyService;
            this.popupService = popupService;
            this.missionService = missionService;
        }

        public void Execute(IGameEventAttachment attachment)
        {
            if (attachment == null)
                return;

            switch (attachment)
            {
                case GameOfferAttachment offer:
                    ExecuteOfferAttachment(offer);
                    break;

                case CurrencyAttachment currency:
                    currencyService.AddCurrency(currency.CurrencyId, currency.Amount);
                    break;

                case PopupAttachment popup:
                    popupService.ShowPopup(popup.PopupId);
                    break;

                case MissionAttachment mission:
                    missionService.ActivateMission(mission.MissionId);
                    break;

#if UNITY_EDITOR
                default:
                    UnityEngine.Debug.LogWarning(
                        $"Unsupported attachment type: {attachment.GetType().Name}");
                    break;
#endif
            }
        }

        private void ExecuteOfferAttachment(GameOfferAttachment offer)
        {
            if (!string.IsNullOrEmpty(offer.OfferGroupId))
            {
                offerService.ActivateGroup(offer.OfferGroupId);
                return;
            }

            if (!string.IsNullOrEmpty(offer.OfferId))
            {
                offerService.ActivateOffer(offer.OfferId);
            }
        }
    }
}