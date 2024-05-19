using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialog/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueNode FirstNode;

    public SpeakerSO Owner;

    public bool IsLocked;

    //Loop on this dialogue when none over are available
    public bool IsLoop;

    //Play this dialog if no loop or no standard dialog
    public bool IsDefault;

    public List<DialogueSO> ReadUnlocks;

    //Dialog Id must be unique
    internal string GetID()
    {
        if(Owner == null)
        {
            return $"COMMON_{name}";
        }

        return $"{Owner.name}_{name}";
    }
}