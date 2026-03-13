using System.Collections.Generic;
using GameOfferSystem.Domain;
using GameOfferSystem.Infrastructure;

public class EasySaveGameOfferGroupRepository : IGameOfferGroupRepository
{
    private const string KEY = "GAME_OFFER_GROUP_RUNTIME";

    public void Save(List<GameOfferGroupRuntimeData> groups)
    {
        ES3.Save(KEY, groups);
    }

    public List<GameOfferGroupRuntimeData> Load()
    {
        if (!ES3.KeyExists(KEY))
            return new List<GameOfferGroupRuntimeData>();

        return ES3.Load<List<GameOfferGroupRuntimeData>>(KEY);
    }
}