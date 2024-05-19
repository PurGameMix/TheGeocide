using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeEffect : MonoBehaviour
{

    [SerializeField]
    private Animator _leftAnimator;
    [SerializeField]
    private Animator _rightAnimator;

    [SerializeField]
    private AudioController _playerAudioController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        _leftAnimator.Play("Smoke_Play");
        _rightAnimator.Play("Smoke_Play");
        _playerAudioController.Play("JumpImpact");
    }
}
