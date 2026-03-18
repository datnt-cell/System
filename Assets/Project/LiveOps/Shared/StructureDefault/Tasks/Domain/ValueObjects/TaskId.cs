using System;

namespace Game.Domain.Tasks
{
    /// <summary>
    /// ID duy nhất của mỗi Task
    /// </summary>
    public readonly struct TaskId
    {
        public readonly string Value;

        public TaskId(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("TaskId không được rỗng");
            Value = value;
        }

        public override string ToString() => Value;
    }
}