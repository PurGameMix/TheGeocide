using Assets.Data.Upgrades.Starters.Definition;
using Assets.Scripts.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    public string NameKey;
    public string DescriptionKey;
    public Sprite Icon;
    public UpgradeCurves Curves;
    public int UnlockCost;

    public List<AbilityAttribute> OptionnalAttributes;
    public PlayerItemSO TargetedItem;
    public string GetId()
    {
        return name;
    }

    internal int GetSelectionCost(int currentLevel)
    {
        return (int)(UnlockCost * 0.1f + Curves.GetSelectionCost(currentLevel));
    }

    internal int GetUpgradeCost(int currentLevel)
    {
        if(currentLevel == 0)
        {
            return UnlockCost;
        }

        return (int) (UnlockCost * 0.25f + Curves.GetUpgradeCost(currentLevel));
    }

    internal string GetLocalizedTitle()
    {
        return TargetedItem == null? LocalizationService.GetLocalizedString(NameKey, TradTable.GUI) : TargetedItem.GetLabel(); 
    }

    internal Sprite GetIcon()
    {
        return TargetedItem == null ? Icon : TargetedItem.GetSetSprite();
    }

    internal string GetLocalizedDescription(int currentLevel)
    {
        var builtDescription = LocalizationService.GetLocalizedString(DescriptionKey, TradTable.GUI);

        if(OptionnalAttributes == null)
        {
            return builtDescription;         
        }

        foreach(var attribute in OptionnalAttributes)
        {
            builtDescription = builtDescription.Replace(attribute.Type.GetReplaceKey(), GetAttributeValue(currentLevel, attribute));
        };

        return builtDescription;
    }

    private string GetAttributeValue(int currentLevel, AbilityAttribute attribute)
    {
        var curveValue = GetAttributeCurveByLevel(currentLevel, attribute);

        return "" + curveValue;
    }

    internal int GetAttributeCurveByLevel(int currentLevel, AbilityAttribute attribute)
    {
        switch (attribute.Type)
        {
            case AttributeType.stackValue:
                
                return attribute.baseValue + (int) (attribute.baseValue * 0.25f * Curves.GetStackByLevel(currentLevel));
            case AttributeType.dmgValue:
               
                return attribute.baseValue + (int)(attribute.baseValue * 0.25f * Curves.GetDammageByLevel(currentLevel));
            case AttributeType.timeValue:

                return attribute.baseValue + (int)(attribute.baseValue * 0.25f * Curves.GetTimeByLevel(currentLevel));
            case AttributeType.percentValue:

                return attribute.baseValue + (int)(attribute.baseValue * 0.25f * Curves.GetPercentByLevel(currentLevel));
            default: throw new Exception($"StarterSo: AttributeType {attribute.Type} not found");
        }
    }
}
