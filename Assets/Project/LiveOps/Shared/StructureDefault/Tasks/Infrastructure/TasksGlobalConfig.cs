using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using Game.Domain.Tasks;

[CreateAssetMenu(fileName = "TasksGlobalConfig", menuName = "GlobalConfigs/TasksGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/Tasks/")]
public class TasksGlobalConfig : GlobalConfig<TasksGlobalConfig>
{
    [Title("📋 TASK LIST", bold: true)]
    [TableList(AlwaysExpanded = false)]
    [Searchable]
    [OnCollectionChanged(nameof(OnTaskListChanged))]
    [ValidateInput(nameof(ValidateTaskIds), "Task Id bị trùng!")]
    public List<TaskConfigData> Tasks = new();

    // ===== AUTO ID =====
    private void OnTaskListChanged(CollectionChangeInfo info)
    {
        if (info.ChangeType == CollectionChangeType.Add)
        {
            var newItem = info.Value as TaskConfigData;
            if (newItem != null && string.IsNullOrEmpty(newItem.Id))
            {
                newItem.Id = GenerateNextTaskId();
            }
        }
    }

    private string GenerateNextTaskId()
    {
        int max = Tasks
            .Where(x => !string.IsNullOrEmpty(x.Id))
            .Select(x => ExtractNumber(x.Id))
            .DefaultIfEmpty(0)
            .Max();

        return $"TASK_{(max + 1):000}";
    }

    private int ExtractNumber(string id)
    {
        var digits = new string(id.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int number) ? number : 0;
    }

    private bool ValidateTaskIds(List<TaskConfigData> list)
        => list.Select(x => x.Id).Distinct().Count() == list.Count;

    public IEnumerable<string> GetAllTaskIds()
    {
        return Tasks.Select(x => x.Id);
    }
}

[System.Serializable]
public class TaskConfigData
{
    // =========================
    // INFO
    // =========================

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Fields")]
    [ReadOnly]
    public string Id;

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Fields")]
    public string DisplayName;

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Fields")]
    public string Description;

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Fields")]
    public TaskGoalType GoalType; // enum: CollectItem, CompleteLevel, LevelStreak,...

    [HorizontalGroup("Content")]
    [BoxGroup("Content/INFO")]
    [VerticalGroup("Content/INFO/Fields")]
    public int GoalAmount; // số lượng cần hoàn thành

    // =========================
    // REWARD
    // =========================

    [HorizontalGroup("Content", Width = 300)]
    [BoxGroup("Content/REWARD")]
    [LabelWidth(70)]
    public List<CurrencyRewardData> Rewards = new();
}

[System.Serializable]
public class CurrencyRewardData
{
    public string CurrencyId;
    [MinValue(1)] public int Amount;
}