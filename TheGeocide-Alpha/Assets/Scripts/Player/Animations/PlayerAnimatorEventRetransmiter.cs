using Assets.Scripts.Player.Animations.Rigging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEventRetransmiter : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private PlayerArmsCombat _playerCombat;

    [SerializeField]
    private PlayerSpells _playerEffects;

    [SerializeField]
    private PlayerStateChannel _psc;

    [SerializeField]
    private PlayerRopePhysicsController _ropePhysics;
    // Update is called once per frame
    void Update()
    {
    }

    public void RopeClimbBegin()
    {
        _ropePhysics.RopeClimbBegin();
    }

    public void RopeClimbLink()
    {
        _ropePhysics.RopeClimbLink();
    }

    //Called by animator
    public void MeleeAttack()
    {
        _playerCombat.MeleeAttack();
    }
    public void MeleeAttackCompleted()
    {
        _playerCombat.MeleeAttackCompleted();
    }

    public void RangedAttackBegin()
    {
        _playerCombat.RangedAttackBegin();
    }

    public void RangedAttackCompleted()
    {
        _playerCombat.RangedAttackCompleted();
    }

    public void DeathCompleted()
    {
        _player.DeathCompleted();
    }

    public void LeftStepCompleted()
    {
        _player.LeftStepCompleted();
    }

    public void RightStepCompleted()
    {
        _player.RightStepCompleted();
    }

    public void SwimLightCompleted()
    {
        _player.SwimLightCompleted();
    }

    public void SwimDiveCompleted()
    {
        _player.SwimDiveCompleted();
    }

    public void SwimHeavyCompleted()
    {
        _player.SwimHeavyCompleted();
    }

    public void EdgeClimbCompleted()
    {
        _psc.RaiseEdgeDetection(new PlayerEdgeEvent(false));
    }

    public void LandingAttackBegin()
    {
        _playerEffects.LandingAttackBegin();
    }

    public void LandingAttackCompleted()
    {
        _playerEffects.LandingAttackCompleted();
    }

    public void LandedAttackBegin()
    {
        _playerEffects.LandedAttackBegin();
    }

    public void LandedAttackCompleted()
    {
        _playerEffects.LandedAttackCompleted();
    }

    public void UppercutAttackBegin()
    {
        _playerEffects.UppercutAttackBegin();
    }

    public void UppercutAttackCompleted()
    {
        _playerEffects.UppercutAttackCompleted();
    }

    public void UltimateAttackBegin()
    {
        _playerEffects.UltimateAttackBegin();
    }

    public void UltimateAttackCompleted()
    {
        _playerEffects.UltimateAttackCompleted();
    }
}
