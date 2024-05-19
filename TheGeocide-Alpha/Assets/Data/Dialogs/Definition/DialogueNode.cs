using UnityEngine;

public abstract class DialogueNode : ScriptableObject
{
    public SpeakerSO Speaker;
    public string Text;

    public abstract bool CanBeFollowedByNode(DialogueNode node);
    public abstract void Accept(DialogueNodeVisitor visitor);
}