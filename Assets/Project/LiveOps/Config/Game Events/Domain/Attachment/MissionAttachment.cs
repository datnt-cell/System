using GameEventModule.Application;

namespace GameEventModule.Domain
{
    public class MissionAttachment : IGameEventAttachment
    {
        public string MissionId;

        public void Execute(IGameEventAttachmentExecutor executor)
        {
            executor.Execute(this);
        }
    }
}