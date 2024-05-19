using Assets.Scripts.Utils;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 5f;
    public int dommage = 34;
    [SerializeField]
    private LayerMask _hitLayers;

    [SerializeField]
    private LayerMask _fleshLayers;

    public Rigidbody2D rb;
    public GameObject ImpactEffect;
    public GameObject FleshImpactEffect;
    public bool isPlayerShooting;

    private float _maxLifeTime = 10f;
    private float _currentTime = 0;
    [SerializeField]
    private Transform _backPosition;

    [SerializeField]
    private Transform _frontPosition;

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        if (!_hitLayers.Contains(hitInfo.gameObject.layer))
        {
            return;
        }

        HandleShot(hitInfo);
    }

    private void FixedUpdate()
    {
        //Detroy projectile if he fly for too long
        _currentTime += Time.deltaTime;
        if (_currentTime >= _maxLifeTime)
        {
            Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void DestroyProjectile(Collider2D hitInfo)
    {
        if (_fleshLayers.Contains(hitInfo.gameObject.layer) && FleshImpactEffect !=null)
        {
            Instantiate(FleshImpactEffect, transform.position, transform.rotation);
        }
        else if (ImpactEffect != null)
        {
            Instantiate(ImpactEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void HandleShot(Collider2D hitInfo)
    {
        DestroyProjectile(hitInfo);

        var canBeDamageObj = hitInfo.GetComponent<ICanBeDamaged>();
        if (canBeDamageObj != null)
        {
            canBeDamageObj.TakeDamage(dommage, isPlayerShooting? HealthEffectorType.player : HealthEffectorType.enemy);
        }
    }

    internal void Fire(Transform targetPoint, float launcherCurrentSpeed)
    {
        Vector2 origin = transform.position;
        Vector2 target = targetPoint.position;
        Vector2 direction = target - origin;
        Vector2 force = direction.normalized * (speed + launcherCurrentSpeed);

        //var angle = GetFireAngle(origin, target, direction);

        //HandleGfxDisplay(targetPoint);
        transform.right = direction;
        rb.velocity = force;
    }

    internal void Fire(float launcherCurrentSpeed)
    {

        Vector3 direction = (_frontPosition.position - _backPosition.position).normalized;

        Vector2 force = direction.normalized * (speed + launcherCurrentSpeed);

        //var angle = GetFireAngle(origin, target, direction);

        //HandleGfxDisplay(targetPoint);
        transform.right = direction;
        //Debug.Log("test:" + force);
        rb.velocity = force;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.localPosition, new Vector2(transform.localPosition.x + 1, transform.localPosition.y));
       
    }
}
