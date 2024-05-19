using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioController _audioController;

    public float BreakTime;

    private float _destroyTimer = 3;
    private bool _isNotTrigger = true;

    public void SetBreakTime(float breakTime)
    {
        BreakTime = breakTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInChildren<Player>())
        {
            OnShakingTriggered();
        }
    }

    private IEnumerator Shaking()
    {
        if (!_isNotTrigger)
        {
            yield break;
        }

        _isNotTrigger = false;
        _animator.Play("FallPlat_Shake");
        _audioController.Play("Shake");
        yield return new WaitForSeconds(BreakTime);
        _audioController.Stop("Shake");
        _audioController.Play("Break");
        _animator.Play("FallPlat_Idle");
        _rb.isKinematic = false;
        Destroy(gameObject, _destroyTimer);
    }

    public virtual void OnShakingTriggered()
    {
        StartCoroutine(Shaking());
    }
}
