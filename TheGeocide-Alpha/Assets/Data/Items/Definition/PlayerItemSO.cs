using Assets.Data.Items.Definition;
using Assets.Scripts.Localization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerItem", menuName ="ScriptableObjects/PlayerItem")]
public class PlayerItemSO : ScriptableObject
{
    public string NameKey;
    public string DescriptionKey;
    public bool IsDefault;
    public float CountDown;
    public float MaxCastTime;
    public float MinCastTime;
    public int Damage;
    public HealthEffectorType EffectType;
    public float EffectProbability = 100;

    public int Level;

    [SerializeField]
    private ItemSpriteLibrarySO _spriteLibrary;
    public ItemType Type;
    public ItemSetType SetType;
    public SpellType SpellType;
    public SpellInitOrigin SpellOrigin;
    public bool IsWorldInstance; // !IsWorldInstance mean "Player child Instance"
    public bool IsDashingSpell;

    public List<PassiveCapacitySO> Capacities;
    public List<string> PlayerBonesChanges;

    public Sprite InventorySprite;
    public Sprite InGameSprite;

    public List<PlayerItemAnimation> OverrideAnimationList;
    public List<PlayerItemSound> OverrideSoundList;


    public GameObject PrimaryEffectsPrefab;
    public GameObject ProjectilePrefab;
    public GameObject CastingUIPrefab;
    public CastingUIType CastingUIType;

    private TradTable _guiTradTable = TradTable.GUI;

    public PlayerItemSO()
    {

    }

    public List<string> GetPlayerBonesChanges()
    {
        return PlayerBonesChanges;
    }


    internal string GetDetailHeader()
    {
        return LocalizationService.GetLocalizedString(NameKey, TradTable.Item);
    }

    internal string GetDetailContent()
    {
        var test =
            $"<b><size=120%>{LocalizationService.GetLocalizedString("SO_DETAIL_DESC", _guiTradTable)}</size></b> : {LocalizationService.GetLocalizedString(DescriptionKey, TradTable.Item)} <nobr> \n" +
            $"<b><size=120%>{LocalizationService.GetLocalizedString("SO_DETAIL_TYPE", _guiTradTable)}</size></b> : <color={GetTypeColor()}>{GetTypeText()}</color> \n" +
            $"<b><size=120%>{LocalizationService.GetLocalizedString("SO_DETAIL_DAMAGE", _guiTradTable)}</size></b> : <color={GetDamageIntensity().GetColor()}>{LocalizationService.GetLocalizedString(GetDamageIntensity().GetTextKey(), _guiTradTable)}</color> \n" +
            $"<b><size=120%>{LocalizationService.GetLocalizedString("SO_DETAIL_SPEED", _guiTradTable)} </size></b> : <color={GetSpeedIntensity().GetColor()}>{LocalizationService.GetLocalizedString(GetSpeedIntensity().GetTextKey(), _guiTradTable)}</color> \n";
        //Debug.Log("ICICICI1: " + test);
        return test;
    }

    internal string GetName()
    {
        return name;
    }

    internal string GetLabel()
    {
        return LocalizationService.GetLocalizedString(NameKey, TradTable.Item);
    }

    #region Text formating
    #endregion //Text formating
    private StatIntensity GetSpeedIntensity()
    {
        if (CountDown > 12)
        {
            return StatIntensity.Low;
        }

        if (CountDown > 2)
        {
            return StatIntensity.Medium;
        }

        return StatIntensity.High;
    }

    private StatIntensity GetDamageIntensity()
    {
        if (Damage > 50)
        {
            return StatIntensity.High;
        }

        if (Damage > 30)
        {
            return StatIntensity.Medium;
        }

        return StatIntensity.Low;
    }

    private string GetTypeText()
    {
        return LocalizationService.GetLocalizedString("SO_TYPE_"+ Type.ToString().ToUpperInvariant(), _guiTradTable);
    }


    private string GetTypeColor()
    {
        return Type.GetColor();
    }

    public bool IsDashSpell()
    {
        return IsDashingSpell;
    }

    internal bool HasLandingEffect()
    {
        return IsDashingSpell && ProjectilePrefab != null && Type == ItemType.Landing;
    }

    internal GameObject GetProjectile()
    {
        return ProjectilePrefab;
    }

    internal bool HasProjectile()
    {
        return ProjectilePrefab != null;
    }

    internal bool HasCooldown()
    {
        return SpellType == SpellType.Instant  || SpellType == SpellType.Summon;
    }

    public Sprite GetSetSprite()
    {
        if(SetType == ItemSetType.None)
        {
            return InventorySprite;
        }
        return _spriteLibrary.GetItemSetIcon(SetType);
    }
}
