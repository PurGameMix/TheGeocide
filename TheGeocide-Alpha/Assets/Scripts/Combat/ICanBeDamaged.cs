using UnityEngine;

public interface ICanBeDamaged
{
    void TakeDamage(int damage, HealthEffectorType type);
}