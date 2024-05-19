using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnSummonFX : InitionFX
{
    internal abstract void SetParameters(bool isBoosted = false, bool isGrounded = false);
}
