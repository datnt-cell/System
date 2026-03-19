using System;

namespace Game.Domain.Tasks
{
    /// <summary>
    /// Exception domain của Task
    /// </summary>
    public class TaskDomainException : Exception
    {
        public TaskDomainException(string message) : base(message) { }
    }
}