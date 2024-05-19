using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Emoji", menuName = "ScriptableObjects/AIEmojiSO")]
public class AIEmojiSO : ScriptableObject
{
    public string Name;
    public EmojiType Type;
    public List<FeelingPrefab> prefabList;
}