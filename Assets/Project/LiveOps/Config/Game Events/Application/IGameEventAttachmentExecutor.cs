using GameEventModule.Domain;

namespace GameEventModule.Application
{
    public interface IGameEventAttachmentExecutor
    {
        void Execute(IGameEventAttachment attachment);
    }
}