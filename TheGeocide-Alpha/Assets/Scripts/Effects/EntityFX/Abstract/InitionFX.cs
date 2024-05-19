using System;
public abstract class InitionFX : EntityFX
{
    internal abstract void Init(Action onCompleted = null);

    internal abstract void Destroy(Action onCompleted = null);
}
