using Assets.Scripts.Utils;
using System;
using UnityEngine;

public class SummonSpell : MonoBehaviour
{
    [SerializeField]
    private InitionFX _summoningFx;

    [SerializeField]
    private InvocationFX _invocationFx;

    [SerializeField]
    private UnSummonFX _destroyFx;

    [SerializeField] private Transform _sumCheckPoint;
    [SerializeField] private Vector2 _sumCheckSize;
    [SerializeField] private LayerMask _sumLayer;

    public bool IsSummoningPossible()
    {
        var colliders = Physics2D.OverlapBoxAll(_sumCheckPoint.position, _sumCheckSize, 0, _sumLayer);
        for(var i =0; i < colliders.Length; i++)
        {
            var col = colliders[i];
            if (!col.isTrigger && col.enabled)
            {
                return false;
            }
        }
        return true;
    }

    public void DestroySpell()
    {
        Destroy(gameObject);
    }

    public void Init(PlayerItemSO item)
    {
        if (!IsSummoningPossible())
        {
            Debug.LogWarning("Impossible to summon spell");
            return;
        }

        if(_summoningFx != null)
        {
            _summoningFx.Init(OnEmergeCompleted);
        }
        else
        {
            _invocationFx.Init(null);
        }      
    }

    public void OnEmergeCompleted()
    {
        _invocationFx.Init(null);
    }

    public void UnSummon()
    {
        _invocationFx.Destroy();
        _summoningFx.Destroy(OnUnSummonCompleted);
    }

    public void Break( bool isBoosted, bool isGrounded)
    {
        _invocationFx.Destroy();
        _destroyFx.SetParameters(isBoosted, isGrounded);
        _destroyFx.Init(OnDestructionCompleted);

    }

    public void OnDestructionCompleted()
    {
        Destroy(gameObject);
    }

    public void OnUnSummonCompleted()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {

        #if UNITY_EDITOR
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(_sumCheckPoint.transform.position, _sumCheckSize);
        #endif
    }
}
