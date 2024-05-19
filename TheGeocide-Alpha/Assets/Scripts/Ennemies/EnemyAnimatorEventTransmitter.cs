using UnityEngine;

public class EnemyAnimatorEventTransmitter : MonoBehaviour
{
    [SerializeField]
    private EnemyAI _enemyAI;


    //Called by animator
    public void RangeAttack()
    {
        _enemyAI.SpecialAttackBegin();
    }

    //Begin of the animation
    public void BasicAttackBegin()
    {
        _enemyAI.BasicAttackBegin();
    }

    //The weapon is armed
    public void BasicAttackSlash()
    {
        _enemyAI.BasicAttackSlash();
    }

    //Dammage point
    public void BasicAttack()
    {
        _enemyAI.BasicAttack();
    }

    //End of the attack animation
    public void BasicAttackCompleted()
    {
        _enemyAI.BasicAttackCompleted();
    }

    //Begin of the animation
    public void RepulseAttackBegin()
    {
        _enemyAI.RepulseAttackBegin();
    }

    //The weapon is armed
    public void RepulseAttackSlash()
    {
        _enemyAI.RepulseAttackSlash();
    }

    //Dammage point
    public void RepulseAttack()
    {
        _enemyAI.RepulseAttack();
    }

    //End of the attack animation
    public void RepulseAttackCompleted()
    {
        _enemyAI.RepulseAttackCompleted();
    }

    public  void SpecialAttackAnimationBegin()
    {
        _enemyAI.SpecialAttackAnimationBegin();
    }
    public  void SpecialAttackSlash()
    {
        _enemyAI.SpecialAttackSlash();
    }
    public  void SpecialAttackBegin()
    {
        _enemyAI.SpecialAttackBegin();
    }

    public void SpecialAttackEnd()
    {
        _enemyAI.SpecialAttackEnd();
    }

    public  void SpecialAttackAnimationCompleted()
    {
        _enemyAI.SpecialAttackAnimationCompleted();
    }

    //Hurt begin
    //public void HurtBegin()
    //{
    //    _enemyAI.HurtBegin();
    //}

    public void HurtCompleted()
    {
        _enemyAI.HurtCompleted();
    }

    public void SurpriseCompleted()
    {
        _enemyAI.SurpriseCompleted();
    }

    public void CheckCompleted()
    {
        _enemyAI.CheckCompleted();
    }

    public void ElecStunCompleted()
    {
        _enemyAI.ElecStunCompleted();
    }
    //Hurt End

    public void DeathCompleted()
    {
        _enemyAI.DeathCompleted();
    }

    public void LeftStepCompleted()
    {
        _enemyAI.LeftStepCompleted();
    }

    public void RightStepCompleted()
    {
        _enemyAI.RightStepCompleted();
    }

    public void AlarmCompleted()
    {
        _enemyAI.AlarmCompleted();
    }

    public void AimingLockBegin()
    {
        _enemyAI.AimingLockBegin();
    }

    public void AimingLockEnd()
    {
        _enemyAI.AimingLockEnd();
    }
}
