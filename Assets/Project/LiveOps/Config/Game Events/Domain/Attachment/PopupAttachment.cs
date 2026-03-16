using GameEventModule.Application;

namespace GameEventModule.Domain
{
    public class PopupAttachment : IGameEventAttachment
    {
        public string PopupId;

        public void Execute(IGameEventAttachmentExecutor executor)
        {
            executor.Execute(this);
        }
    }
}