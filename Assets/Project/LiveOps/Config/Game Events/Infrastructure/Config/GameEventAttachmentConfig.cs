using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using GameEventModule.Domain;

namespace GameEventModule.Infrastructure.Config
{
    [Serializable]
    [InlineProperty]
    public class GameEventAttachmentConfig
    {
        [HorizontalGroup("Row", 0.3f)]
        [LabelText("Type")]
        public EventAttachmentType Type;

        // =========================
        // OFFER
        // =========================

        [HorizontalGroup("Row")]
        [ShowIf("@Type == EventAttachmentType.Offer")]
        [LabelText("Offer Group")]
        [ValueDropdown(nameof(GetOfferGroupDropdown), IsUniqueList = true)]
        public string OfferGroupId;

        [HorizontalGroup("Row")]
        [ShowIf("@Type == EventAttachmentType.Offer")]
        [LabelText("Offer")]
        [ValueDropdown(nameof(GetOfferDropdown), IsUniqueList = true)]
        public string OfferId;

        // =========================
        // CURRENCY
        // =========================

        [HorizontalGroup("Row")]
        [ShowIf("@Type == EventAttachmentType.Currency")]
        [LabelText("Currency")]
        [ValueDropdown(nameof(GetCurrencyIds))]
        public string CurrencyId;

        [HorizontalGroup("Row")]
        [ShowIf("@Type == EventAttachmentType.Currency")]
        [LabelText("Amount")]
        public int Amount;

        // =========================
        // DROPDOWNS
        // =========================

        private static IEnumerable<ValueDropdownItem<string>> GetOfferGroupDropdown()
        {
            var list = new List<ValueDropdownItem<string>>();

            list.Add(new ValueDropdownItem<string>("None", ""));

            if (GameOfferGroupGlobalConfig.Instance == null)
                return list;

            list.AddRange(
                GameOfferGroupGlobalConfig.Instance.Groups
                    .Select(g => new ValueDropdownItem<string>(
                        $"{g.Id} | {g.DisplayName}", g.Id))
            );

            return list;
        }

        private static IEnumerable<ValueDropdownItem<string>> GetOfferDropdown()
        {
            var list = new List<ValueDropdownItem<string>>();

            list.Add(new ValueDropdownItem<string>("None", ""));

            if (GameOfferGlobalConfig.Instance == null)
                return list;

            list.AddRange(
                GameOfferGlobalConfig.Instance.Offers
                    .Select(o => new ValueDropdownItem<string>(
                        $"{o.DisplayName}", o.Id))
            );

            return list;
        }

        private static IEnumerable<ValueDropdownItem<string>> GetCurrencyIds()
        {
            if (CurrencyGlobalConfig.Instance == null)
                return Enumerable.Empty<ValueDropdownItem<string>>();

            return CurrencyGlobalConfig.Instance.Currencies
                .Select(c =>
                {
                    string label = string.IsNullOrEmpty(c.DisplayName)
                        ? c.Id
                        : c.DisplayName;

                    return new ValueDropdownItem<string>(label, c.Id);
                });
        }

        // =========================
        // BUILD DOMAIN
        // =========================

        public IGameEventAttachment Build()
        {
            switch (Type)
            {
                case EventAttachmentType.Offer:
                    return new GameOfferAttachment(
                        OfferGroupId,
                        OfferId);

                case EventAttachmentType.Currency:
                    return new CurrencyAttachment
                    {
                        CurrencyId = CurrencyId,
                        Amount = Amount
                    };
            }

            return null;
        }
    }
}