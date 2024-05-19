using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/DialogChannel")]
public class DialogueChannel : ScriptableObject
{
    public delegate void DialogueCallback(DialogueSO dialogue);
    public DialogueCallback OnDialogueRequested;
    public DialogueCallback OnDialogueStart;
    public DialogueCallback OnDialogueEnd;
    public DialogueCallback OnDialogueSkipRequested;

    public delegate void DialogueNodeCallback(DialogueNode node);
    public DialogueNodeCallback OnDialogueNodeRequested;
    public DialogueNodeCallback OnDialogueNodeStart;
    public DialogueNodeCallback OnDialogueNodeEnd;

    public void RaiseRequestDialogue(DialogueSO dialogue)
    {
        OnDialogueRequested?.Invoke(dialogue);
    }

    public void RaiseDialogueStart(DialogueSO dialogue)
    {
        OnDialogueStart?.Invoke(dialogue);
    }

    public void RaiseDialogueEnd(DialogueSO dialogue)
    {
        OnDialogueEnd?.Invoke(dialogue);
    }

    internal void RaiseDialogSkipRequest(DialogueSO dialogue)
    {
        OnDialogueSkipRequested?.Invoke(dialogue);
    }

    public void RaiseRequestDialogueNode(DialogueNode node)
    {
        OnDialogueNodeRequested?.Invoke(node);
    }

    public void RaiseDialogueNodeStart(DialogueNode node)
    {
        OnDialogueNodeStart?.Invoke(node);
    }

    public void RaiseDialogueNodeEnd(DialogueNode node)
    {
        OnDialogueNodeEnd?.Invoke(node);
    }
}