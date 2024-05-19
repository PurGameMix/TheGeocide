using Assets.Data.Dialogs.Definition;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/DialogManagerChannel")]
/*
 * Overlay of DialogueChannel allowing  the communication between DialogManager and all DialogAgents
 * See DialogManager.cs && DialogAgent.cs
 */
public class DialogManagerChannel : ScriptableObject
{
    public delegate void SpeakerRequest(string recipientName);
    public delegate void DialogueCallback(DialogueEvent dialog);
    public delegate void DialogueIdCallback(string dialogId, string recipientName);
    public delegate void DialogueListCallback(List<DialogueSO> dialogList, string recipientName);
    public delegate void DialogueStateCallback(DialogueState dialog);

    public SpeakerRequest OnCurrentDialogueRequested;
    public SpeakerRequest OnCurrentDialogueSkipRequested;
    public DialogueCallback OnCurrentDialogueFound;
    public DialogueCallback OnDialogUnlocked;
    public DialogueCallback OnDialogueRequested;
    public DialogueCallback OnDialogueEnd;
    public DialogueListCallback OnRegisterDialogRequested;
    public DialogueCallback OnRegisterDialogCompleted;
    public DialogueIdCallback OnRequestDialogState;
    public DialogueStateCallback OnRequestDialogStateCompleted;

    internal void RaiseRequestDialogState(string dialogId, string recipientName)
    {
        OnRequestDialogState?.Invoke(dialogId, recipientName);
    }

    public void RaiseRequestDialogStateCompleted(DialogueState dialogState)
    {
        OnRequestDialogStateCompleted?.Invoke(dialogState);
    }


    internal void RaiseRegisterDialogueList(List<DialogueSO> availableDialogList, string recipientName)
    {
        OnRegisterDialogRequested?.Invoke(availableDialogList, recipientName);
    }

    public void RaiseRegisterDialogCompleted(DialogueEvent dialogEvt)
    {
        OnRegisterDialogCompleted?.Invoke(dialogEvt);
    }

    internal void RaiseRequestCurrentDialogSkip(string recipientName)
    {
        OnCurrentDialogueSkipRequested?.Invoke(recipientName);
    }

    public void RaiseRequestCurrentDialogue(string recipientName)
    {
        OnCurrentDialogueRequested?.Invoke(recipientName);
    }

    public void RaiseCurrentDialogueFound(DialogueEvent dialogEvt)
    {
        OnCurrentDialogueFound?.Invoke(dialogEvt);
    }

    public void RaiseDialogueUnlocked(DialogueEvent dialogEvt)
    {
        OnDialogUnlocked?.Invoke(dialogEvt);
    }

    public void RaiseRequestDialogue(DialogueEvent dialogEvt)
    {
        OnDialogueRequested?.Invoke(dialogEvt);
    }

    public void RaiseDialogueEnd(DialogueEvent dialogEvt)
    {
        OnDialogueEnd?.Invoke(dialogEvt);
    }
}