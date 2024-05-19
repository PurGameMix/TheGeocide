using Assets.Data.GameEvent.Definition;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameEventListenerEntry
{
    public GameEventType Type;
    public UnityEngine.Object Origin;
    public UnityEvent m_Event;
}

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEventChannel m_Channel;
    [SerializeField]
    private GameEventListenerEntry[] m_Entries;

    private void Awake()
    {
        m_Channel.OnGameEvent += OnGameEvent;
    }

    private void OnDestroy()
    {
        m_Channel.OnGameEvent -= OnGameEvent;
    }

    private void OnGameEvent(GameEvent gEvt)
    {
        GameEventListenerEntry foundEntry = Array.Find(m_Entries, x => x.Type == gEvt.Type && x.Origin == gEvt.Origin);
        if (foundEntry != null)
        {
            foundEntry.m_Event.Invoke();
        }
    }
}