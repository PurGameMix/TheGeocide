using System;
using UnityEngine;

[Serializable]
public class Effect
{
    public string Name;

    public EntityFX Prefab;


    [Header("Play point")]
    public Transform OriginPosition;

    public bool IsFollowingOrigin;
}
