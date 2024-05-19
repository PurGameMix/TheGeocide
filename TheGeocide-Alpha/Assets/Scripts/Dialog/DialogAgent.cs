using Assets.Data.Dialogs.Definition;
using Assets.Data.GameEvent.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple version of DialogAgent.cs Handling one piece of dialog
/// </summary>
public class DialogAgent : MonoBehaviour
{
    [SerializeField]
    private string _recipientName;
    [SerializeField]
    private DialogManagerChannel m_DialogueManagerChannel;
    [SerializeField]
    private SpriteRenderer _availableDialogSprite;
    [SerializeField]
    private List<DialogueSO> _dialogueList;

    private bool hasFirstInteract;
    private DialogueEvent _currentDialog;

    [Header("Event")]
    public UnityEvent OnDialogBoxClose;

    [Header("GameEvent")]
    [SerializeField]
    private GameEventChannel _geChannel;

    [SerializeField]
    private List<DialogGameEventTrigger> Triggers;

    private void Awake()
    {

        if (string.IsNullOrEmpty(_recipientName))
        {
            Debug.LogWarning(name+ ": RecipientName is mandatory to communicate with DialogueManager");
        }
        m_DialogueManagerChannel.OnCurrentDialogueFound += OnCurrentDialogueFound;
        m_DialogueManagerChannel.OnDialogUnlocked += OnDialogUnlocked;
        m_DialogueManagerChannel.OnRegisterDialogCompleted += OnRegisterDialogCompleted;
        m_DialogueManagerChannel.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDestroy()
    {
        m_DialogueManagerChannel.OnCurrentDialogueFound -= OnCurrentDialogueFound;
        m_DialogueManagerChannel.OnDialogUnlocked -= OnDialogUnlocked;
        m_DialogueManagerChannel.OnDialogueEnd -= OnDialogueEnd;
        m_DialogueManagerChannel.OnRegisterDialogCompleted -= OnRegisterDialogCompleted;
    }

    internal void SkipDialog()
    {
        m_DialogueManagerChannel.RaiseRequestCurrentDialogSkip(_recipientName);
    }

    private void Start()
    {
        //Debug.Log(_recipientName + ":Ask register");
        m_DialogueManagerChannel.RaiseRegisterDialogueList(_dialogueList, _recipientName);
    }

    public string GetRecipientName()
    {
        return _recipientName;
    }

    public void OnDialogInteraction()
    {
        m_DialogueManagerChannel.RaiseRequestDialogue(_currentDialog);
    }

    private void OnCurrentDialogueFound(DialogueEvent dialog)
    {
        if (IsMessageForOtherAgent(dialog))
        {
            return;
        }

        SetCurrentDialog(dialog);
    }

    private void OnDialogUnlocked(DialogueEvent dialog)
    {
        if (IsMessageForOtherAgent(dialog))
        {
            return;
        }

        SetCurrentDialog(dialog);
    }

    private void OnDialogueEnd(DialogueEvent dialog)
    {

        if (IsMessageForOtherAgent(dialog))
        {
            return;
        }

        OnDialogBoxClose?.Invoke();

        if (_geChannel != null)
        {
            var hit = Triggers.FirstOrDefault(evt => evt.Type == DialogGameTriggerType.DialogEnd && evt.Dialogue == dialog.Dialogue);

            if (hit != null)
            {
                _geChannel.RaiseEvent(new GameEvent()
                {
                    Origin = hit.Dialogue,
                    Type = hit.Type.GetGameEventType()
                });
            }
            
        }
        m_DialogueManagerChannel.RaiseRequestCurrentDialogue(_recipientName);
    }

    private void OnRegisterDialogCompleted(DialogueEvent dialog)
    {

        if (dialog.Recipient != _recipientName)
        {
            return;
        }

        SetCurrentDialog(dialog);
    }

    private bool IsMessageForOtherAgent(DialogueEvent dialog)
    {
        return dialog.Recipient != _recipientName;
    }
    private void SetCurrentDialog(DialogueEvent dialog)
    {
        _currentDialog = dialog;

        //Debug.Log("CurrentEventId :" + dialog.GetEventID());

        if(_availableDialogSprite != null)
        {
            _availableDialogSprite.enabled = !_currentDialog.IsRead && !_currentDialog.IsDefault();
        }
       
    }

}
