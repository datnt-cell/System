public enum Errors
{
    Unknown = 1,

    // =========================
    // AUTH
    // =========================
    NoAccessToken = 1000,

    // =========================
    // STORAGE
    // =========================
    StorageRequestsMadeTooOften = 1001,
    NoSuchProduct = 1002,
    StorageError = 1003,

    // =========================
    // VERSION
    // =========================
    GameVersion = 1004,
    DataVersion = 1005,

    // =========================
    // USER
    // =========================
    NoUserInfo = 1006,

    // =========================
    // UNITY IAP
    // =========================
    UnityPurchasing_PurchasingUnavailable = 1010,
    UnityPurchasing_NoProductsAvailable = 1011,
    UnityPurchasing_AppNotKnown = 1012,
    UnityPurchasing_ProductIsNotAvailable = 1013,
    UnityPurchasing_PurchaseFailed = 1014,

    // =========================
    // PAYMENT
    // =========================
    PaymentFailed = 1200,
    PaymentProcessing = 1201,
    PaymentCancelled = 1202,

    // =========================
    // RESOURCE
    // =========================
    NotEnoughResources = 1300,
    NotAvailable = 1301
}