using Assets.Data.PlayerMouvement.Definition;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmojiPlayerAgent : MonoBehaviour
{

    [SerializeField]
    private PlayerStateChannel _playerStateChannel;
    [SerializeField]
    private List<PlayerEmojiSO> _emojiList;

    private GameObject _currentEmoji = null;

    private List<PlayerStateType> _StatesInvolvingEmoji = new List<PlayerStateType>()
    {
        PlayerStateType.Stun
    };

	private void Awake()
	{
        _playerStateChannel.OnMouvementStateEnter += OnMouvementStateEnter;
        _playerStateChannel.OnMouvementStateExit += OnMouvementStateExit;
    }

    private void OnDestroy()
	{
        _playerStateChannel.OnMouvementStateEnter -= OnMouvementStateEnter;
        _playerStateChannel.OnMouvementStateExit -= OnMouvementStateExit;
    }
    private void OnMouvementStateEnter(PlayerStateEvent mvtEvt)
    {
        if (!_StatesInvolvingEmoji.Contains(mvtEvt.Type))
        {
            return;
        }

        var emoji = _emojiList.FirstOrDefault(item => item.Name == mvtEvt.Type.ToString());
        if (emoji == null)
        {
            Debug.LogError($"{GetType().FullName}: Emoji prefab not found for {mvtEvt.Type}");
        }

        _currentEmoji = Instantiate(emoji.prefab, transform);
    }

    private void OnMouvementStateExit(PlayerStateEvent mvtEvt)
    {
        Destroy(_currentEmoji);
        _currentEmoji = null;
    }
}
