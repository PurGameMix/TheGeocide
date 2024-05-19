using UnityEngine;

public interface ICanBeRepulsed
{
    void TakeRepulse(Vector2 repulseMagnitude);
    void TakeKnockBack(Vector2 repusleMagnitude, float stunDuration = 1.5f);
}