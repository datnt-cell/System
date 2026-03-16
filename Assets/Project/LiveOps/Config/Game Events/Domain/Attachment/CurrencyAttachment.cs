using GameEventModule.Application;

namespace GameEventModule.Domain
{
    public class CurrencyAttachment : IGameEventAttachment
    {
        public string CurrencyId;
        public int Amount;

        public void Execute(IGameEventAttachmentExecutor executor)
        {
            executor.Execute(this);
        }
    }
}