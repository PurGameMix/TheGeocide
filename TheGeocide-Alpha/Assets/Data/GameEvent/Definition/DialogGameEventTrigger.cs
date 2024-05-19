using Assets.Data.GameEvent.Definition;
using System;


[Serializable]
public class DialogGameEventTrigger
{
    public DialogGameTriggerType Type;
    public DialogueSO Dialogue;
}
