using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{

    public int BeginTime = 0;

    public int FireRate = 1;

    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _firePoint;
    [SerializeField]
    private Transform _targetPoint;


    private bool _isShooting;
    private bool _PeriodicLock;
    // Start is called before the first frame update
    void Start()
    {
        if(BeginTime > 0)
        {
            StartCoroutine(IsAwakePLaying());
        }
    }

    private IEnumerator IsAwakePLaying()
    {
        yield return new WaitForSeconds(BeginTime);
        ShootArrawPeriodic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootArrawPeriodic()
    {
        if (_isShooting) { 
            return;
        }
        _isShooting = true;

        if (!_PeriodicLock)
        {
            StartCoroutine(PeriodicArrow());
        }
        else
        {
            StartCoroutine(WaitForLock());
        } 
    }

    private IEnumerator WaitForLock()
    {
        while (_PeriodicLock)
        {
            yield return new WaitForSeconds(FireRate/2);
        }

        StartCoroutine(PeriodicArrow());
    }

    private IEnumerator PeriodicArrow()
    {
        _PeriodicLock = true;
        while (_isShooting)
        {
            ShootArrow();
            yield return new WaitForSeconds(FireRate);       
        }

        _PeriodicLock = false;
    }

    public void StopArrawPeriodic()
    {
        if (!_isShooting)
        {
            return;
        }

        _isShooting = false;
    }

    public void ShootArrow()
    {
        var prefabInstance = Instantiate(_projectile, _firePoint.position, _firePoint.rotation);
        Projectile arrow = prefabInstance.GetComponent<Projectile>();
        arrow.Fire(_targetPoint, 0);
    }
}
