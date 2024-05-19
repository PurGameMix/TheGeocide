using Assets.Scripts.Localization;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    [SerializeField]
    private FlowChannel _flowChannel;

    [SerializeField]
    UnityEvent m_OnInteraction;

    public string TextKey;

    public TradTable TextTable;

    public void DoInteraction()
    {
        m_OnInteraction.Invoke();
    }

    public void SetInteraction(UnityEvent onInteraction, string tradKey, TradTable tradTable = TradTable.GUI)
    {
        if (_flowChannel == null)
        {
            Debug.LogWarning($"{GetType().FullName} : Impossible de mettre à jour l'interface graphique");
            return;
        }

        m_OnInteraction = onInteraction;
        TextKey = tradKey;
        TextTable = tradTable;

        _flowChannel.RaiseInteractionChanged();
    }
}