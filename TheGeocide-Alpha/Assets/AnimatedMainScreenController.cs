using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMainScreenController : MonoBehaviour
{
    [SerializeField]
    private Animator _playerAnimator;

    private void Start()
    {
        _playerAnimator.SetLayerWeight(0, 0);
        _playerAnimator.SetLayerWeight(1, 0);
        _playerAnimator.SetLayerWeight(2, 1);
        _playerAnimator.Play("IdleMainScreen");
    }
}
