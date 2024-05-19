using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Data.Dialogs.Definition
{
    public class DialogueEvent
    {
        internal DialogueSO Dialogue;
        internal string Recipient;
        internal bool IsRead;
        internal bool IsLocked;

        public DialogueEvent(DialogueSO item)
        {
            Dialogue = item;

            if(item.Owner != null)
            {
                Recipient = item.Owner.Name;
            }

            IsLocked = item.IsLocked;
        }

        public DialogueEvent(DialogueSO item, string recipientName)
        {
            Dialogue = item;          
            Recipient = recipientName;

            IsLocked = item.IsLocked;
        }

        internal bool IsDefault()
        {
            return Dialogue.IsDefault;
        }

        internal bool IsLoop()
        {
            return Dialogue.IsLoop;
        }

        internal string GetID()
        {
            return Dialogue.GetID();
        }

        internal string GetEventID()
        {
            return $"{Recipient}_Diag_{GetID()}";
        }

        internal DialogueState GetDialogueState()
        {
            return new DialogueState()
            {
                DialogId = GetID(),
                IsLocked = IsLocked,
                IsRead = IsRead
            };
        }
    }
}