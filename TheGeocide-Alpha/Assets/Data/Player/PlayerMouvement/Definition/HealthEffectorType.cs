using System.Collections.Generic;

public enum HealthEffectorType
{
    dot,
    enemy,
    fall,
    trap,
    player,
    playerIgnite, //The player start ignition
    ignite, //ignite over time damages
    playerFroze,
    playerFire,
    playerFrozeFist,
    playerElec
}

public static class HealthEffectorTypeExtensions
{

    private static List<HealthEffectorType> _isFireList = new List<HealthEffectorType>{
        HealthEffectorType.playerIgnite,
        HealthEffectorType.playerFire,
        HealthEffectorType.ignite,
    };

    public static bool IsFire(this HealthEffectorType het)
    {
        return _isFireList.Contains(het);
    }
}