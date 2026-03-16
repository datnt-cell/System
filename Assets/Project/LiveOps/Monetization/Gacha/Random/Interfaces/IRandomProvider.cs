namespace GameSystems.Random.Interfaces
{
    public interface IRandomProvider
    {
        int Range(int min, int max);
        float Value();
    }
}