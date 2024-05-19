using Assets.Data.Dialogs.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private List<DialogueEvent> AvailableDialogEvent;

    private List<string> Recipients;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;


    [SerializeField]
    private DialogManagerChannel m_DialogueManagerChannel;

    [SerializeField]
    private FlowState m_InDialogFlowState;

    private string _currentDiagRecipient = string.Empty;

    private void Awake()
    {
        AvailableDialogEvent = new List<DialogueEvent>();
        Recipients = new List<string>();
        m_DialogueChannel.OnDialogueEnd += OnDialogueEnd;
        m_DialogueManagerChannel.OnDialogueRequested += OnDialogueRequested;
        m_DialogueManagerChannel.OnCurrentDialogueRequested += OnCurrentDialogueRequested;
        m_DialogueManagerChannel.OnCurrentDialogueSkipRequested += OnCurrentDialogueSkipRequested;
        m_DialogueManagerChannel.OnRegisterDialogRequested += OnRegisterDialogRequested;
        m_DialogueManagerChannel.OnRequestDialogState += OnRequestDialogState;
    }


    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueEnd -= OnDialogueEnd;
        m_DialogueManagerChannel.OnDialogueRequested -= OnDialogueRequested;
        m_DialogueManagerChannel.OnCurrentDialogueRequested -= OnCurrentDialogueRequested;
        m_DialogueManagerChannel.OnCurrentDialogueSkipRequested -= OnCurrentDialogueSkipRequested;
        m_DialogueManagerChannel.OnRegisterDialogRequested -= OnRegisterDialogRequested;
        m_DialogueManagerChannel.OnRequestDialogState -= OnRequestDialogState;
    }

    private void OnRequestDialogState(string dialogId, string recipient)
    {

        var dialog = GetDialogEventByRecipient(dialogId, recipient);
        if(dialog == null)
        {
            return;
        }
        m_DialogueManagerChannel.RaiseRequestDialogStateCompleted(dialog.GetDialogueState());
    }

    private void OnRegisterDialogRequested(List<DialogueSO> dialogList, string recipientName)
    {

        if (dialogList == null || dialogList.Count == 0 || string.IsNullOrEmpty(recipientName))
        {
            Debug.LogError($"{GetType().FullName}: List or recipient is empty");
            return;
        }

        if (Recipients.Contains(recipientName))
        {
            Debug.LogError($"{GetType().FullName}: Recipient {recipientName} already exist for list {string.Join(",",dialogList.Select(item => item.GetID()))}");
            return;
        }
        else
        {
            Recipients.Add(recipientName);
        }

        GetDialogEventFromList(dialogList, recipientName);
        var dialog = GetCurrentDialogOf(recipientName);
        m_DialogueManagerChannel.RaiseRegisterDialogCompleted(dialog);
    }

    private void GetDialogEventFromList(List<DialogueSO> dialogList, string recipientName)
    {
       foreach (var item in dialogList)
        {

            if (DialogEventAlreadyRegister(item, recipientName))
            {
                Debug.Log($"{GetType().FullName}: Dialog {item.GetID()} already registered for {recipientName}");
                continue;
            }

            var diagEvt = new DialogueEvent(item, recipientName);

            HandleHistoricalState(diagEvt);

            AvailableDialogEvent.Add(diagEvt);
        }
    }

    private void HandleHistoricalState(DialogueEvent diagEvt)
    {
        //todo: handle saves dialog status
    }

    private bool DialogEventAlreadyRegister(DialogueSO item, string recipientName)
    {
        return AvailableDialogEvent.Any(evt => evt.GetID() == item.GetID() && evt.Recipient == recipientName);
    }

    private void OnDialogueRequested(DialogueEvent dialogEvt)
    {
        _currentDiagRecipient = dialogEvt.Recipient;
        m_DialogueChannel.RaiseRequestDialogue(dialogEvt.Dialogue);
    }

    private void OnDialogueEnd(DialogueSO dialog)
    {
        var dialogEvent = GetDialogEventByRecipient(dialog.GetID(), _currentDiagRecipient);
        _currentDiagRecipient = string.Empty;
        dialogEvent.IsRead = true;

        //Handle unlock new diag
        if (dialog.ReadUnlocks.Any())
        {
            foreach(var unlock in dialog.ReadUnlocks)
            {
                var foundDiag = GetDialogEventById(unlock.GetID());
                foundDiag.IsLocked = false;
                m_DialogueManagerChannel.RaiseDialogueUnlocked(foundDiag);
            }
        }

        m_DialogueManagerChannel.RaiseDialogueEnd(dialogEvent);
    }

    private DialogueEvent GetDialogEventByRecipient(string dialogId, string recipient)
    {
        var foundDiag = AvailableDialogEvent.FirstOrDefault(item => item.GetID() == dialogId && item.Recipient == recipient);
        if (foundDiag == null)
        {
            Debug.LogWarning($"{GetType().FullName}: dialogue matching key {dialogId} not found for recipient {_currentDiagRecipient}.");
        }
        return foundDiag;
    }
    private DialogueEvent GetDialogEventById(string dialogId)
    {
        var foundDiag = AvailableDialogEvent.FirstOrDefault(item => item.GetID() == dialogId);
        if (foundDiag == null)
        {
            Debug.LogWarning($"{GetType().FullName}: dialogue matching key {dialogId} not found for recipient {_currentDiagRecipient}.");
        }
        return foundDiag;
    }

    private void DebugList()
    {
        var debug = $"List : {AvailableDialogEvent.Count}\n";
        foreach(var item in AvailableDialogEvent)
        {
            debug += $"Id: {item.GetEventID()}, IsRead: {item.IsRead}, IsLocked: {item.IsLocked}\n";
        }

        Debug.Log(debug);
    }
    public DialogueEvent GetCurrentDialogOf(string recipientName)
    {
        var available = AvailableDialogEvent.FirstOrDefault(item => item.Recipient == recipientName && item.IsRead == false && item.IsLocked == false && !item.IsDefault());

        //Searching loop
        if (available == null)
        {
            //DebugList();
            available = AvailableDialogEvent.FirstOrDefault(item => item.Recipient == recipientName && item.IsLoop() && item.IsRead);
        }

        //Searchin default
        if (available == null) {
            //DebugList();
            return AvailableDialogEvent.FirstOrDefault(item => item.Recipient == recipientName && item.IsDefault());
        }

        //Debug.Log($"Speaker : {owner.name}, Dialog: {available.GetID()}");

        return available;
    }


    private void OnCurrentDialogueRequested(string recipientName)
    {
        m_DialogueManagerChannel.RaiseCurrentDialogueFound(GetCurrentDialogOf(recipientName));
    }

    private void OnCurrentDialogueSkipRequested(string recipientName)
    {
        m_DialogueChannel.RaiseDialogSkipRequest(GetCurrentDialogOf(recipientName).Dialogue);
    }
    
}
