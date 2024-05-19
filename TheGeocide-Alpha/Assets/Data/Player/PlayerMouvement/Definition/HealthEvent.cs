public class HealthEvent
{
    public int Difference;
    public HealthEffectorType Origin;
    public bool IsHpLoss()
    {
        return Difference < 0;
    }

    public bool IsHpGain()
    {
        return Difference > 0;
    }
}