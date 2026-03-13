using System.Collections.Generic;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;

public class EasySaveGameOfferGroupRepository : IGameOfferGroupRepository
{
    private const string KEY = "GAME_OFFER_GROUP_RUNTIME";
    private const string FILE = "GameOfferGroupRuntime.es3";

    /// <summary>
    /// Lưu runtime data của các Offer Groups
    /// </summary>
    public void Save(List<GameOfferGroupRuntimeData> groups)
    {
        ES3.Save(KEY, groups, FILE);
    }

    /// <summary>
    /// Load runtime data từ file
    /// </summary>
    public List<GameOfferGroupRuntimeData> Load()
    {
        if (!ES3.KeyExists(KEY, FILE))
            return new List<GameOfferGroupRuntimeData>();

        return ES3.Load<List<GameOfferGroupRuntimeData>>(KEY, FILE);
    }
}