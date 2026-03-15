namespace GameEventModule.Domain
{
    /// <summary>
    /// Attachment được thực thi khi Game Event bắt đầu
    /// </summary>
    public interface IGameEventAttachment
    {
        void Execute();
    }
}