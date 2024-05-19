using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AimAssist : MonoBehaviour
{
    public abstract void SetCursor(Transform cursorTransform);

    public abstract void SetCastTime(float castTime);

    public abstract void Stop();

    public abstract float GetAimWindow();

    public abstract Transform GetPosition();
}