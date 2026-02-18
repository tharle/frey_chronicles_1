using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public enum Id
    {
        // LoadTargets,
        // EnterRoom,
        // DamageToPlayer,
        // RefreshInfoHUD,
        // RefreshHUDHP,
        // RefreshHUDTP,
        // RefreshHUDAP,
        // ComboInfoHUD,
        // ComboTimerToggle,
        // ComboDamageToEnemy,
        // EnemyDie,
        // SelectTarget,
        // CastMagic,
        // BattleMode,
        // NotifyRunes,
        // AddRunes,
        // CleanRunes,
        // ResultSpell
        RotateCamera
    }

    #region Singleton

    private static GameEvent m_Instance;

    public static GameEvent Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = new();
            return m_Instance;
        }
    }
    #endregion

    private Dictionary<Id, Action<GameEventParams>> m_Events;

    private GameEvent()
    {
        m_Events = new Dictionary<Id, Action<GameEventParams>>();
    }

    public void SubscribeTo(Id eventId, Action<GameEventParams> action)
    {
        if (!m_Events.ContainsKey(eventId)) m_Events.Add(eventId, action);
        else m_Events[eventId] += action;
    }

    public void UnsubscribeFrom(Id eventId, Action<GameEventParams> action)
    {
        if (!m_Events.ContainsKey(eventId)) return;

        m_Events[eventId] -= action;

        if (m_Events[eventId] == null) m_Events.Remove(eventId);
    }

    public void TriggerEvent(Id eventId)
    {
        TriggerEvent(eventId, new GameEventParams());
    }

    public void TriggerEvent(Id eventId, GameEventParams parameters)
    {
        if (!m_Events.ContainsKey(eventId))
        {
            Debug.Log($"Impossible trigger Event {eventId}.");
            return;
        }

        m_Events[eventId]?.Invoke(parameters);
    }

    public void ClearAll()
    {
        m_Events = new Dictionary<Id, Action<GameEventParams>>();
    }
}

#region Game Event Params
public class GameEventParams
{
    public enum Id
    {
        Targets,
        Room,
        DamageAttack,
        DamageElemental,
        Player,
        ComboValue,
        ComboTimerToggle,
        Enter,
        HudBarData,
        TargetController,
        Target,
        Spell,
        Rune,
        TensionCost,
        SpellName
    }

    private Dictionary<Id, object> m_Params;

    public GameEventParams()
    {
        m_Params = new Dictionary<Id, object>();
    }

    public GameEventParams(Id messageId, object value) : this()
    {
        Add(messageId, value);
    }


    public void Add(Id eventMessageId, object value)
    {
        if (m_Params.ContainsKey(eventMessageId))
        {
            m_Params[eventMessageId] = value;
        }
        else
        {
            m_Params.Add(eventMessageId, value);
        }
    }

    public bool Contains<T>(Id eventMessageId, out T value)
    {
        value = default;
        if (m_Params.ContainsKey(eventMessageId) && m_Params[eventMessageId] is T)
        {
            value = (T)m_Params[eventMessageId];
            return true;
        }

        return false;
    }
}
#endregion