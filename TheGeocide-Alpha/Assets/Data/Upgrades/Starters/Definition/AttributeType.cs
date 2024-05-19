using System.Collections.Generic;

public enum AttributeType
{
    stackValue,
    dmgValue,
    timeValue,
    percentValue
}

public static class AttributeTypeExtensions
{
    private static Dictionary<AttributeType, string> _replaceKeyDico = new Dictionary<AttributeType, string>(){
    { AttributeType.stackValue,"%%stackValue%%" },
    {AttributeType.dmgValue,"%%dmgValue%%" },
     {AttributeType.timeValue,"%%timeValue%%" },
     {AttributeType.percentValue,"%%pcValue%%" }
    };

    public static string GetReplaceKey(this AttributeType at)
    {
        if (!_replaceKeyDico.ContainsKey(at))
        {
            throw new KeyNotFoundException($"AttributeTypeExtensions: No replace key founded for {at}");
        }

        return _replaceKeyDico[at];
    }
}