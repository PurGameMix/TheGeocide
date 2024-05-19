using Assets.Data.Enemy.Definition;
using Assets.Data.PlayerMouvement.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmojiEnemyAgent : MonoBehaviour
{
    [SerializeField]
    private PlayerStateChannel _playerStateChannel;
    [SerializeField]
    private EnemyStateChannel _enemyStateChannel;
    [SerializeField]
    private EntityInfo _parentInfos;
    private string _enemyId;

    [SerializeField]
    private List<AIEmojiSO> _emojiList;

    [SerializeField]
    private Transform _primaryDisplayPoint;

    [SerializeField]
    private Transform _secondaryDisplayPoint;

    private Emoji _primaryEmoji = null;
    private Emoji _secondaryEmoji = null;
    private bool _isDead;
    private List<WaitingEmoji> _waitingQueue = new List<WaitingEmoji>();
    private List<EnemyStateType> _EnemyStatesInvolvingEmoji = new List<EnemyStateType>()
    {
        EnemyStateType.ActionSuprised,
        EnemyStateType.ActionAlarm,
        EnemyStateType.ActionCheck,
        EnemyStateType.Flee,
        EnemyStateType.Hurt,
        EnemyStateType.HurtStun,
        EnemyStateType.MoveToPlayer,
    };
    private List<EmojiType> _effectState = new List<EmojiType>()
    {
        EmojiType.Frozen,
        EmojiType.Burn,
        EmojiType.Electrocute
    };

    private List<PlayerStateType> _playerStateInvolvingEmoji = new List<PlayerStateType>()
    {
        PlayerStateType.Dead,
    };

    private void Awake()
    {
        _enemyStateChannel.OnStateEnter += OnStateEnter;
        _enemyStateChannel.OnStateExit += OnStateExit;
        _enemyStateChannel.OnFrozenTickChanged += OnFrozenTickChanged;
        _playerStateChannel.OnMouvementStateEnter += OnMouvementStateEnter;
        _playerStateChannel.OnMouvementStateExit += OnMouvementStateExit;
        _enemyStateChannel.OnElectrocuteTickChanged += OnElectrocuteTickChanged;
    }

    private void Start()
    {
        _enemyId = _parentInfos.EntityName;
    }

    private void OnDestroy()
    {
        _enemyStateChannel.OnStateEnter -= OnStateEnter;
        _enemyStateChannel.OnStateExit -= OnStateExit;
        _playerStateChannel.OnMouvementStateEnter -= OnMouvementStateEnter;
        _playerStateChannel.OnMouvementStateExit -= OnMouvementStateExit;
        _enemyStateChannel.OnFrozenTickChanged -= OnFrozenTickChanged;
    }

    private void OnFrozenTickChanged(FrozenStateEvent frozenEvt)
    {
        if (_enemyId != frozenEvt.EnemyId)
        {
            return;
        }

        if (frozenEvt.FrozenTick == 0 || _isDead)
        {
            CleanDisplay(EmojiType.Frozen);
            return;
        }

        ReorganiseEmoji();
        var text = $"{frozenEvt.FrozenTick} / {frozenEvt.FrozenMax}";

        if(_primaryEmoji?.EmojiType == EmojiType.Frozen)
        {
            _primaryEmoji.SetText(text);
            return;
        }

        if (_secondaryEmoji?.EmojiType == EmojiType.Frozen)
        {
            _secondaryEmoji.SetText(text);
            return;
        }

        DisplayEmoji(EmojiType.Frozen, text);
    }

    private void OnElectrocuteTickChanged(ElectrocuteStateEvent electEvt)
    {
        if (_enemyId != electEvt.EnemyId)
        {
            return;
        }

        if (electEvt.ElecTick == 0 || _isDead)
        {
            CleanDisplay(EmojiType.Electrocute);
            return;
        }

        ReorganiseEmoji();
        var text = $"{electEvt.ElecTick} / {electEvt.ElecMax}";

        if (_primaryEmoji?.EmojiType == EmojiType.Electrocute)
        {
            _primaryEmoji.SetText(text);
            return;
        }

        if (_secondaryEmoji?.EmojiType == EmojiType.Electrocute)
        {
            _secondaryEmoji.SetText(text);
            return;
        }

        DisplayEmoji(EmojiType.Electrocute, text);
    }

    private void OnStateEnter(EnemyStateEvent stateEvt)
    {

        if(_enemyId != stateEvt.EnemyId)
        {
            return;
        }

        if (!_EnemyStatesInvolvingEmoji.Contains(stateEvt.Type))
        {
            _isDead = stateEvt.Type == EnemyStateType.Die;
            return;
        }

        var emojyType = EmojiTypeExtensions.GetFromEnemyStateType(stateEvt.Type);
        DisplayEmoji(emojyType);
    }

    private void OnStateExit(EnemyStateEvent stateEvt)
    {
        if (_enemyId != stateEvt.EnemyId)
        {
            return;
        }

        if (!_EnemyStatesInvolvingEmoji.Contains(stateEvt.Type))
        {
            return;
        }

        CleanDisplay(EmojiTypeExtensions.GetFromEnemyStateType(stateEvt.Type));
    }

    private void OnMouvementStateEnter(PlayerStateEvent mvtEvt)
    {
        if (!_playerStateInvolvingEmoji.Contains(mvtEvt.Type))
        {
            return;
        }

        var emojyType = EmojiTypeExtensions.GetFromPlayerStateType(mvtEvt.Type);
        DisplayEmoji(emojyType);
    }

    private void OnMouvementStateExit(PlayerStateEvent mvtEvt)
    {
        if (!_playerStateInvolvingEmoji.Contains(mvtEvt.Type))
        {
            return;
        }

        CleanDisplay(EmojiTypeExtensions.GetFromPlayerStateType(mvtEvt.Type));
    }


    private void DisplayEmoji(EmojiType emojyType, string text = "")
    {
        var emojiPrefab = GetEmojiPrefab(emojyType);
        if (emojiPrefab == null)
        {
            Debug.LogWarning($"{GetType().FullName}: Emoji not found for type {emojyType}");
            return;
        }

        if (_primaryEmoji != null)
        {
            if(_primaryEmoji.EmojiType == emojyType)
            {
                _primaryEmoji.SetText(text);
                return;
            }

            if (_secondaryEmoji != null)
            {

                if (_secondaryEmoji.EmojiType == emojyType)
                {
                    _secondaryEmoji.SetText(text);
                    return;
                }

                RegisterInQueue(emojiPrefab, emojyType, text);
            }
            else
            {
                _secondaryEmoji = InstantiateEmoji(emojiPrefab, emojyType, _secondaryDisplayPoint, text);
            }
        }
        else
        {
            _primaryEmoji = InstantiateEmoji(emojiPrefab, emojyType, _primaryDisplayPoint, text);
        }

        ReorganiseEmoji();
    }

    private Emoji InstantiateEmoji(GameObject prefab, EmojiType type, Transform _displayPoint, string text = "")
    {
        Emoji instance = Instantiate(prefab, _displayPoint).GetComponent<Emoji>();
        instance.EmojiType = type;
        instance.SetText(text);
        return instance;
    }

    private void CleanDisplay(EmojiType emojyType)
    {
        if (_primaryEmoji != null && _primaryEmoji.EmojiType == emojyType )
        {
            Destroy(_primaryEmoji.gameObject);
            _primaryEmoji = null;
        }

        if (_secondaryEmoji != null && _secondaryEmoji.EmojiType == emojyType)
        {
            Destroy(_secondaryEmoji.gameObject);
            _secondaryEmoji = null;
        }

        UnstackQueue();
    }

    private void ReorganiseEmoji()
    {
        if(_primaryEmoji != null && _secondaryEmoji != null)
        {
            if(_effectState.Contains(_primaryEmoji.EmojiType) && !_effectState.Contains(_secondaryEmoji.EmojiType))
            {
                var tmp = _primaryEmoji;
                _primaryEmoji = _secondaryEmoji;
                _secondaryEmoji = tmp;
                _primaryEmoji.transform.position = _primaryDisplayPoint.position;
                _secondaryEmoji.transform.position = _secondaryDisplayPoint.position;
            }
            return;
        }
        if (_primaryEmoji == null && _secondaryEmoji != null)
        {
            _primaryEmoji = _secondaryEmoji;
            _secondaryEmoji = null;
            _primaryEmoji.transform.position = _primaryDisplayPoint.position;
        }
    }

    private void UnstackQueue()
    {
        //full
        if (_primaryEmoji != null && _secondaryEmoji != null)
        {
            return;
        }

        //swap
        ReorganiseEmoji();

        if (_primaryEmoji == null && _waitingQueue.Count > 0)
        {
            var item = _waitingQueue[0];
            _waitingQueue.Remove(item);
            _primaryEmoji = InstantiateEmoji(item.prefab, item.EmojiType, _primaryDisplayPoint ,item.text);
        }

        if (_secondaryEmoji == null && _waitingQueue.Count > 0)
        {
            var item = _waitingQueue[0];
            _waitingQueue.Remove(item);
            _secondaryEmoji = InstantiateEmoji(item.prefab, item.EmojiType, _secondaryDisplayPoint, item.text); ;
        }
    }

    private void RegisterInQueue(GameObject emojiPrefab, EmojiType emojyType, string text)
    {
        if (_waitingQueue.Any(item => item.EmojiType == emojyType))
        {
            return;
        }

        _waitingQueue.Add(new WaitingEmoji()
        {
            EmojiType = emojyType,
            prefab = emojiPrefab,
            text = text
        });
    }

    private GameObject GetEmojiPrefab(EmojiType emojiType)
    {
        var element = _emojiList.FirstOrDefault(item => item.Type == emojiType);

        if (element == null)
        {
            return null;
        }

        var nbEmoji = element.prefabList.Count();

        if (nbEmoji == 0)
        {
            return null;
        }

        if (nbEmoji == 1)
        {
            return element.prefabList.First().Prefab;
        }

        //todo handle feeling

        //then get by weight
        var rng = UnityEngine.Random.Range(0, 101) / 100;
        foreach (var emoji in element.prefabList.OrderBy(item => item.Weight))
        {
            if (rng <= emoji.Weight)
            {
                return emoji.Prefab;
            }
        }

        return null;
    }

}
