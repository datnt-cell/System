using GameEventModule.Application;

public class PopupServiceAdapter : IPopupService
{
    // private readonly PopupService popupService;

    // public PopupServiceAdapter(PopupService popupService)
    // {
    //     this.popupService = popupService;
    // }

    public void ShowPopup(string popupId)
    {
        if (string.IsNullOrEmpty(popupId))
            return;

      //  popupService.Show(popupId);
    }
}