using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedWeapon : MonoBehaviour
{
    public Transform firePoint;
    public Transform targetPoint;
    public GameObject projectilePrefab;

    private Vector2 _lastPosition;
    private Vector2 _currentSpeed;

    //Called by animator
    public void Shoot()
    {
        var prefab = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile bullet = prefab.GetComponent<Projectile>();

        bullet.Fire(targetPoint, _currentSpeed.magnitude);
    }

    private void Start()
    {
        _lastPosition = transform.position;
    }
    private void Update()
    {
        Vector2 currentPosition = transform.position;
        if (_lastPosition != currentPosition)
        {
            _currentSpeed = currentPosition - _lastPosition;
            _currentSpeed /= Time.deltaTime;
            _lastPosition = currentPosition;
        }
    }
}
