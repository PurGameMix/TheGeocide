using UnityEngine;

public class FrozenEffect : MonoBehaviour
{
    [SerializeField]
    internal Animator _animator;

    [SerializeField]
    internal AudioController _audioController;

    private float _frostCoef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    internal void FrostBegin()
    {
        _audioController.Play(GetSound());
    }

    private string GetSound()
    {
        if(_frostCoef <= 0.3f)
        {
            return "Light";
        }

        if (_frostCoef <= 0.6f)
        {
            return "Medium";
        }

        return "Strong";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Destroy()
    {
        Destroy(gameObject);
    }

    internal void Init(float frozenCoef)
    {
        _frostCoef = frozenCoef;
        _animator.Play("IceFrozen");
    }

    internal void SetFrostRatio(float frozenCoef)
    {
        _frostCoef = frozenCoef;
    }
}
