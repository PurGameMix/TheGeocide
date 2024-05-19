
public interface DialogueNodeVisitor
{
    void Visit(TextDialogueNode node);
    void Visit(ChoiceDialogueNode node);
}