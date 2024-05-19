using Assets.Data.Enemy.Definition;
using Assets.Data.PlayerMouvement.Definition;
using System;
using System.Collections.Generic;

public enum EmojiType
{
    Attack,
    Burn,
    Check,
    Dead,
    Flee,
    Frozen,
    Hit,
    Hurt,
    Stun,
    Surprised,
    Electrocute,
    Alarm
}

public static class EmojiTypeExtensions
{

    private static List<EmojiType> _loopingEmoji = new List<EmojiType>()
    {
        EmojiType.Stun,
        EmojiType.Frozen,
        EmojiType.Burn,
        EmojiType.Check,
        EmojiType.Electrocute,
        EmojiType.Alarm
    };

    public static bool IsLoop(this EmojiType et)
    {
        return _loopingEmoji.Contains(et);
    }

    private static Dictionary<EnemyStateType, EmojiType> _enemyStateMap = new Dictionary<EnemyStateType, EmojiType>(){
        {EnemyStateType.MoveToPlayer,EmojiType.Attack },  
        {EnemyStateType.ActionCheck,EmojiType.Check },   
        {EnemyStateType.Flee,EmojiType.Flee },    
        {EnemyStateType.Hurt,EmojiType.Hurt },    
        {EnemyStateType.HurtStun,EmojiType.Stun },    
        {EnemyStateType.ActionSuprised, EmojiType.Surprised },
        {EnemyStateType.ActionAlarm, EmojiType.Alarm }
    };

    public static EmojiType GetFromEnemyStateType(EnemyStateType est)
    {
        return _enemyStateMap[est];
    }

    private static Dictionary<PlayerStateType, EmojiType> _playerStateMap = new Dictionary<PlayerStateType, EmojiType>(){
        {PlayerStateType.Dead, EmojiType.Dead},
    };

    public static EmojiType GetFromPlayerStateType(PlayerStateType pst)
    {
        return _playerStateMap[pst];
    }
}

