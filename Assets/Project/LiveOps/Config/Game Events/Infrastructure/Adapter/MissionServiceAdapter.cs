using GameEventModule.Application;

public class MissionServiceAdapter : IMissionService
{
    // private readonly MissionService missionService;

    // public MissionServiceAdapter(MissionService missionService)
    // {
    //     this.missionService = missionService;
    // }

    public void ActivateMission(string missionId)
    {
        if (string.IsNullOrEmpty(missionId))
            return;

      //  missionService.Activate(missionId);
    }
}