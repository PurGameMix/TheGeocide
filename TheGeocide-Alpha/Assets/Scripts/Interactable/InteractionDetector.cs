using Assets.Scripts.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    [Header("GameFlow State")]
    [SerializeField]
    private FlowChannel m_GameFlowChannel;
    [SerializeField]
    private FlowState m_InGameFlowState;

    private Dictionary<string, Interactable> m_NearbyInteractables = new Dictionary<string, Interactable>();

    public bool HasNearbyInteractables()
    {
        return m_NearbyInteractables.Count != 0;
    }

    private void Start()
    {
        InputHandler.instance.OnInteraction += OnInteraction;
    }

    private void OnDestroy()
    {
        InputHandler.instance.OnInteraction -= OnInteraction;
    }

    private void OnInteraction(InputHandler.InputArgs obj)
    {   
        if (!HasNearbyInteractables() || FlowStateMachine.Instance.CurrentState != m_InGameFlowState)
        {
            return;
        }

        //Ideally, we'd want to find the best possible interaction (ex: by distance & orientation).
        foreach (var kvp in m_NearbyInteractables)
        {
            kvp.Value.DoInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && !m_NearbyInteractables.ContainsKey(other.name))
        {
            m_NearbyInteractables.Add(other.name, interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && m_NearbyInteractables.ContainsKey(other.name))
        {
            m_NearbyInteractables.Remove(other.name);
        }
    }

    internal TradTable GetTextTable()
    {
        if (m_NearbyInteractables.Count == 0)
        {
            return TradTable.GUI;
        }

        return m_NearbyInteractables.Last().Value.TextTable;
    }

    internal string GetTextKey()
    {
        if(m_NearbyInteractables.Count == 0)
        {
            //Debug.LogWarning($"{GetType().FullName}: No Interacble found");
            return string.Empty;
        }

        return m_NearbyInteractables.Last().Value.TextKey;
    }
}