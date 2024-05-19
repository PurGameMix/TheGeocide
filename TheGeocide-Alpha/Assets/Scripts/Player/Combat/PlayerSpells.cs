using Assets.Data.Items.Definition;
using Assets.Data.Player.PlayerSpells.Definition;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Cast spells effects from weapons
/// </summary>
public class PlayerSpells : MonoBehaviour
{

    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;
    [SerializeField]
    private AudioChannel _guiChannel;
    [SerializeField]
    private PlayerSpellStateChannel _playerSpellStateChannel;

    [Header("Cast points")]

    public List<PlayerSpellConfig> CastPointList;
    [SerializeField]
    private Transform _baseSpellPoint;
    [SerializeField]
    private Transform _centerSpellPoint;
    [SerializeField]
    private Transform _upperSummonPoint;
    [SerializeField]
    private Transform _lowerSummonPoint;

    private PlayerItemSO _landingWeapon;
    private PlayerItemSO _uppercutWeapon;
    private PlayerItemSO _ultimateWeapon;

    private void Awake()
    {
        _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnDestroy()
    {
        _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
    }

    private void HandleSpellInstantation(PlayerItemSO pi)
    {
        var go = GetSpellInstance(pi);
        if (pi.SpellType.IsInstantSpell())
        {
            //var go = Instantiate(pi.PrimaryEffectsPrefab, _baseSpellPoint);
            //Todo voir si on a besoin d'un parenting instance parameter
            //var go = Instantiate(pi.PrimaryEffectsPrefab, _baseSpellPoint.position, Quaternion.identity);
            var spell = go.GetComponent<InstantSpell>();
            spell.Init(pi);
        }

        if (pi.SpellType.IsBuffSpell())
        {
            //var go = Instantiate(pi.PrimaryEffectsPrefab, _centerSpellPoint);
            var spell = go.GetComponent<InstantSpell>();
            spell.Init(pi);        
        }

        if (pi.SpellType.IsSummonSpell())
        {
            //var go = Instantiate(pi.PrimaryEffectsPrefab, GetSummonPoint(pi), Quaternion.identity);
            var spell = go.GetComponent<SummonSpell>();

            if (!spell.IsSummoningPossible())
            {
                _guiChannel.RaiseAudioRequest(new AudioEvent("OnCD"));
                Destroy(go);
                return;
            }

            spell.Init(pi);
        }

        _playerSpellStateChannel.RaiseSpellChanged(new PlayerSpellStateEvent(pi, SpellKeyActionType.Release));
    }

    private GameObject GetSpellInstance(PlayerItemSO pi)
    {
        var castPoint = GetCastPoint(pi);
        if (pi.IsWorldInstance)
        {
            return Instantiate(pi.PrimaryEffectsPrefab, castPoint.position, Quaternion.identity);
        }

        return Instantiate(pi.PrimaryEffectsPrefab, castPoint);
    }

    private Transform GetCastPoint(PlayerItemSO pi)
    {
        var point = CastPointList.FirstOrDefault(item => item.Origin == pi.SpellOrigin);

        if(point == null)
        {
            Debug.LogError("Point not found");
            return CastPointList.First().Position;
        }

        return point.Position;
    }

    internal void LandingAttackBegin()
    {
        HandleSpellInstantation(_landingWeapon);
    }

    internal void LandedAttackBegin()
    {
        if (_landingWeapon.HasLandingEffect())
        {
            var go = Instantiate(_landingWeapon.ProjectilePrefab, GetCastPoint(_landingWeapon).position, Quaternion.identity);
            var spell = go.GetComponent<InstantSpell>();
            spell.Init(_landingWeapon);
        }
    }

    internal void LandedAttackCompleted()
    {
    }

    internal void LandingAttackCompleted()
    {
    }

    internal void UppercutAttackBegin()
    {
        HandleSpellInstantation(_uppercutWeapon);
    }

    internal void UppercutAttackCompleted()
    {
        //Destroy(_currentEffect.gameObject,0.3f);
    }

    internal void UltimateAttackBegin()
    {
        HandleSpellInstantation(_ultimateWeapon);
    }

    internal void UltimateAttackCompleted()
    {

    }

    private void OnInventoryChanged(PlayerItemSO so)
    {
        if (!so.Type.IsHandleByStateMachine())
        {
            return;
        }

        switch (so.Type)
        {
            case ItemType.Landing: _landingWeapon = so; break;
            case ItemType.Uppercut: _uppercutWeapon = so; break;
            case ItemType.Ultimate: _ultimateWeapon = so; break;
            default: Debug.LogWarning($" PlayerItem of type {so.Type} is not handle");break;
        }

    }
}
