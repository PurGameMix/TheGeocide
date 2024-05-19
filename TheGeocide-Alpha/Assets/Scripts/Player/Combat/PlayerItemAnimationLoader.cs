using Assets.Data.Items.Definition;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Player.Combat
{
    public class PlayerItemAnimationLoader : MonoBehaviour
    {
        [SerializeField]
        private PlayerInventoryChannel _playerInventoryChannel;

        [SerializeField]
        private AudioMixer _mixer;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private AnimatorOverrideController _runtimeAnimator;
        [SerializeField]
        private AudioController _playerAudioController;

        private AudioMixerGroup _effectGrp;
        void Awake()
        {
            _animator.runtimeAnimatorController = _runtimeAnimator;
            _effectGrp = _mixer.FindMatchingGroups("Effects")[0];
            _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
        }

        void OnDestroy()
        {
            _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
        }



        private void OnInventoryChanged(PlayerItemSO so)
        {
            LoadItemSounds(so);
            LoadItemAnimation(so);
        }

        private void LoadItemSounds(PlayerItemSO pitem)
		{
			//var grp = _mixer.FindMatchingGroups("Effects")[0];

            foreach(var sound in pitem.OverrideSoundList)
            {
                _playerAudioController.RegisterClip(sound.OverridedClipName, sound.SoundClip, _effectGrp);
            }
		}

		void LoadItemAnimation(PlayerItemSO pitem)
		{
            // _animator.runtimeAnimatorController = _runtimeAnimator;

            foreach (var anim in pitem.OverrideAnimationList)
            {
                _runtimeAnimator[anim.OverridedAnimationName] = anim.AnimationClip;
            }
		}

        //private void LoadMeleeAttackSounds()
        //{
        //    var grp = _mixer.FindMatchingGroups("Effects")[0];
        //    _playerAudioController.RegisterClip("Player_Melee_Cast", _meleeWeapon.GetCastSound(), grp);
        //    _playerAudioController.RegisterClip("Player_Melee_Release", _meleeWeapon.GetReleaseSound(), grp);
        //}

        //void LoadMeleeAttackAnimation()
        //{
        //    _animator.runtimeAnimatorController = _runtimeAnimator;
        //    _runtimeAnimator["Player_Melee_Cast_Body"] = _meleeWeapon.GetCastAnimation();
        //    _runtimeAnimator["Player_Melee_Release_Body"] = _meleeWeapon.GetReleaseAnimation();
        //}


        //void LoadRangedAttackSounds()
        //{
        //    var grp = _mixer.FindMatchingGroups("Effects")[0];
        //    _playerAudioController.RegisterClip("Player_Ranged_Cast", _rangedWeapon.GetCastSound(), grp);
        //    _playerAudioController.RegisterClip("Player_Ranged_Release", _rangedWeapon.GetReleaseSound(), grp);
        //}

        //void LoadRangedAttackAnimation()
        //{
        //    _animator.runtimeAnimatorController = _runtimeAnimator;
        //    _runtimeAnimator["Player_Ranged_Cast_Body"] = _rangedWeapon.GetCastAnimation();
        //    _runtimeAnimator["Player_Ranged_Release_Body"] = _rangedWeapon.GetReleaseAnimation();
        //}
    }
}
