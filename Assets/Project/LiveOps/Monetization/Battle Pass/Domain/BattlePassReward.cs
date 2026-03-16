namespace BattlePassModule.Domain
{
    public enum BattlePassRewardType
    {
        Currency,
        CurrencyBundle
    }

    public class BattlePassReward
    {
        public BattlePassRewardType Type;

        public string RewardId;

        public int Amount;

        private BattlePassReward(
            BattlePassRewardType type,
            string rewardId,
            int amount)
        {
            Type = type;
            RewardId = rewardId;
            Amount = amount;
        }

        // =========================
        // FACTORIES
        // =========================

        public static BattlePassReward Currency(
            string currencyId,
            int amount)
        {
            return new BattlePassReward(
                BattlePassRewardType.Currency,
                currencyId,
                amount);
        }

        public static BattlePassReward Bundle(
            string bundleId)
        {
            return new BattlePassReward(
                BattlePassRewardType.CurrencyBundle,
                bundleId,
                1);
        }
    }
}