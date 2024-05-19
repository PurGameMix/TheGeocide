using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileAnimatorEventTransmitter : MonoBehaviour
{

    [SerializeField]
    private Crocodile _entity;


    public void DoDammage()
    {
        _entity.DoDammage();
    }

    public void SwimLight()
    {
        _entity.SwimLight();
    }

    public void SwimHeavy()
    {
        _entity.SwimHeavy();
    }

    public void AttackCompleted()
    {
        _entity.AttackCompleted();
    }

    public void DespawnCompleted()
    {
        _entity.DespawnCompleted();
    }
}
