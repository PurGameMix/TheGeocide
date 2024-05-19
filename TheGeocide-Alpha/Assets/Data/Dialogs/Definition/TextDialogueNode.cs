using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialog/Node_Text")]
public class TextDialogueNode : DialogueNode
{
    public DialogueNode NextNode;


    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return NextNode == node;
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}