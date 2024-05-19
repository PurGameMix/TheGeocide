using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTankSpecialAttackSpell : MonoBehaviour
{

    [SerializeField]
    private ChannelSpell _specialAttackFX;
    [SerializeField]
    private RepulseFX _specialAttackAreaFX;
    [SerializeField]
    private AreaEffector2D _areaEffector2D;
    public void Init(int dmg, float tickPerSecond, HealthEffectorType et, float ep = 100)
    {
        _specialAttackFX.Init(dmg, tickPerSecond, et, ep);
    }

    internal void StartChannel()
    {
        _specialAttackFX.StartChannel();
        _specialAttackAreaFX.StartFX();
        _areaEffector2D.enabled = true;
    }

    internal void StopChannel()
    {
        _specialAttackFX.StopChannel();
        _specialAttackAreaFX.StopFX();
        _areaEffector2D.enabled = false;
    }

}
