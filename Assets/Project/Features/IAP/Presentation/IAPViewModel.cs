using Cysharp.Threading.Tasks;
using R3;
using IAPModule.Application.UseCases;
using Gley.EasyIAP;

namespace IAPModule.Presentation.ViewModel
{
    /// <summary>
    /// ViewModel không kế thừa MonoBehaviour
    /// </summary>
    public class IAPViewModel
    {
        private readonly PurchaseUseCase _useCase;

        public ReactiveProperty<bool> IsProcessing = new(false);
        public Subject<ShopProductNames> OnPurchaseSuccess = new();

        public IAPViewModel(PurchaseUseCase useCase)
        {
            _useCase = useCase;
        }

        public async UniTask BuyAsync(ShopProductNames productId)
        {
            IsProcessing.Value = true;

            var result = await _useCase.ExecuteAsync(productId);

            IsProcessing.Value = false;

            if (result.IsSuccess)
            {
                OnPurchaseSuccess.OnNext(productId);
            }
        }
    }
}