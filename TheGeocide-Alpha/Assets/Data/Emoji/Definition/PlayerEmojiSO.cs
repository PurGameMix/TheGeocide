using UnityEngine;

[CreateAssetMenu(fileName = "Emoji", menuName = "ScriptableObjects/PlayerEmoji")]
public class PlayerEmojiSO : ScriptableObject
{
    public string Name;
    public GameObject prefab;
}