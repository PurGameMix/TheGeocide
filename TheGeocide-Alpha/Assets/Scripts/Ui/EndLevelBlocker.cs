using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelBlocker : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _blockPlayerFrontier;
    private int _deadEnemy = 0;
    private int _unlockCondition = 5;
    // Start is called before the first frame update
    void Start()
    {
        _blockPlayerFrontier.enabled = true;
        _deadEnemy = 0;
    }

    public void CountDeadEnemy()
    {
        _deadEnemy++;

        if(_deadEnemy >= _unlockCondition)
        {
            _blockPlayerFrontier.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_blockPlayerFrontier.bounds.center, _blockPlayerFrontier.bounds.size);
    }
}
