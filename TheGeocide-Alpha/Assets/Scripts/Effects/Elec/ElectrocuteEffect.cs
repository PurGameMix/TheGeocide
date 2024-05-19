using UnityEngine;

public class ElectrocuteEffect : EntityFX
{
    [SerializeField]
    internal Animator _animator;

    [SerializeField]
    internal AudioController _audioController;

    private float _elecRatio;

    private string GetSound()
    {
        if(_elecRatio <= 0.34)
        {
            return "Light";
        }

        if (_elecRatio <= 0.67f)
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

    internal void Init(float elecRatio)
    {
        _elecRatio = elecRatio;
        _animator.Play("Electrocuted");
    }

    internal void SetFrostRatio(float elecRatio)
    {
        _elecRatio = elecRatio;
    }

    internal override void EffectBegin(string effectName)
    {
        _audioController.Play(GetSound());
    }

    internal override void EffectCompleted(string effectName)
    {
        throw new System.NotImplementedException();
    }
}
