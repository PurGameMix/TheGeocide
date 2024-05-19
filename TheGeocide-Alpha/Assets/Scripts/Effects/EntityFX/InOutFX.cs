using System;

public class InOutFX : EntityFX
{
    internal override void EffectBegin(string effectName)
    {
        throw new NotImplementedException();
    }

    internal override void EffectCompleted(string effectName)
    {
        Destroy(gameObject);
    }
}
