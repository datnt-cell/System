namespace GachaSystem.Domain.Models
{
    public class GachaResult
    {
        public string PoolId;
        public GachaItem Item;
        public int Index;

        public GachaResult(string poolId, GachaItem item, int index)
        {
            PoolId = poolId;
            Item = item;
            Index = index;
        }
    }
}