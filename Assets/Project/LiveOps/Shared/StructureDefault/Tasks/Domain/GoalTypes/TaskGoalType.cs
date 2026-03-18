namespace Game.Domain.Tasks
{
    /// <summary>
    /// Loại mục tiêu của Task
    /// </summary>
    public enum TaskGoalType
    {
        CollectItem,    // Thu thập vật phẩm
        CompleteLevel,  // Hoàn thành level
        LevelStreak,    // Chuỗi level liên tiếp
        // TODO: thêm loại goal mới ở đây
    }
}