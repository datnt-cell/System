using System;
using R3;

namespace IAPModule.Domain
{
    [Serializable]
    public class PaymentState
    {
        // =========================
        // BASIC METRICS
        // =========================

        public ReactiveProperty<float> TotalSpend = new(0);

        public ReactiveProperty<int> PaymentsCount = new(0);

        public ReactiveProperty<float> MaxPayment = new(0);

        public ReactiveProperty<long> LastPaymentTime = new(0);

        public ReactiveProperty<string> Currency = new("");

        // =========================
        // PLAYER ECONOMY PROFILE
        // =========================

        public ReactiveProperty<float> ComfortablePayment = new(0);

        public ReactiveProperty<float> ResourcesMultiplier = new(1);

        public ReactiveProperty<float> MeanDaysBetweenPurchase = new(0);

        // =========================
        // INTERNAL
        // =========================

        private long _firstPaymentTime;

        // =========================
        // ADD PURCHASE
        // =========================

        public void AddPurchase(float price, string currency)
        {
            if (price <= 0)
                return;

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Currency
            if (string.IsNullOrEmpty(Currency.Value))
                Currency.Value = currency;

            // Count
            PaymentsCount.Value++;

            // Spend
            TotalSpend.Value += price;

            // Max payment
            if (price > MaxPayment.Value)
                MaxPayment.Value = price;

            // First payment
            if (_firstPaymentTime == 0)
                _firstPaymentTime = now;

            // =========================
            // Purchase interval
            // =========================

            if (LastPaymentTime.Value != 0)
            {
                float days = (now - LastPaymentTime.Value) / 86400f;

                if (MeanDaysBetweenPurchase.Value == 0)
                {
                    MeanDaysBetweenPurchase.Value = days;
                }
                else
                {
                    // Smooth average
                    MeanDaysBetweenPurchase.Value =
                        (MeanDaysBetweenPurchase.Value * 0.7f) + (days * 0.3f);
                }
            }

            LastPaymentTime.Value = now;

            // =========================
            // Comfortable payment
            // =========================

            ComfortablePayment.Value =
                TotalSpend.Value / PaymentsCount.Value;

            // =========================
            // Economy multiplier
            // =========================

            ResourcesMultiplier.Value =
                1f + (ComfortablePayment.Value / 50f);
        }

        // =========================
        // HELPERS
        // =========================

        public bool HasPurchased()
        {
            return PaymentsCount.Value > 0;
        }

        public int DaysSinceLastPurchase()
        {
            if (LastPaymentTime.Value == 0)
                return int.MaxValue;

            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return (int)((now - LastPaymentTime.Value) / 86400);
        }

        public float GetAverageSpend()
        {
            if (PaymentsCount.Value == 0)
                return 0;

            return TotalSpend.Value / PaymentsCount.Value;
        }
    }
}