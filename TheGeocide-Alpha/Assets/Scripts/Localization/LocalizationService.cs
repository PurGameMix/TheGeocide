using Assets.Scripts.Localization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class LocalizationService : MonoBehaviour
{
    public static LocalizedStringTable m_StringTable = new LocalizedStringTable { TableReference = "GUI" };

    private List<LocalizedStringTable> _availableTable = new List<LocalizedStringTable>()
    {
         new LocalizedStringTable{TableReference = "GUI"},
        new LocalizedStringTable{TableReference = "Dialog"},
        new LocalizedStringTable{TableReference = "Item" }
    };

    private static Dictionary<string, StringTable> _availableStringTables;

    private void Awake()
    {
        _availableStringTables = new Dictionary<string, StringTable>();

        foreach (var table in _availableTable)
        {
            _availableStringTables.Add(table.TableReference.TableCollectionName, table.GetTable());
        }
    }

    public static string GetLocalizedString(string entryName, TradTable tableName)
    {

        if (!_availableStringTables.ContainsKey(tableName.ToString()))
        {
            Debug.LogWarning("Tryng to access localization but not yet loaded");
            return entryName;
        }

        var entry = _availableStringTables[tableName.ToString()].GetEntry(entryName);

        if(entry == null)
        {
            Debug.LogWarning($"No entry found for table [{tableName} and Key {entryName}]");
            return entryName;
        }
        // We can also extract Metadata here
        //var comment = entry.GetMetadata<Comment>();
        //if (comment != null)
        //{
        //    Debug.Log($"Found metadata comment for {entryName} - {comment.CommentText}");
        //}

        return entry.GetLocalizedString(); // We can pass in optional arguments for Smart Format or String.Format here.
    }
}