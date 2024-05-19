using Assets.Data.GameEvent.Definition;
using Assets.Data.Items.Definition;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItem : MonoBehaviour
{
    public PlayerItemSO data;
    public GameObject PickupHelperCanvas;
    //private Animator _animator;
    private PlayerInventory _playerInvNear;
    private UI_PickupHelper _pickupHelperUI;

    [SerializeField]
    private AudioChannel _audioChannel;

    [SerializeField]
    private GameEventChannel _geChannel;


    void Awake()
    {
        //Setup sprite
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = GetUISprite();
    }

    private void Start()
    {
        _pickupHelperUI = PickupHelperCanvas.GetComponent<UI_PickupHelper>();
        _pickupHelperUI.SetContent(data);
        InputHandler.instance.OnInteraction += OnInteraction;
    }

    private void OnInteraction(InputHandler.InputArgs obj)
    {
        if (_playerInvNear == null)
        {
            return;
        }

        _audioChannel.RaiseAudioRequest(new AudioEvent("Pickup"));
        _playerInvNear.PickUp(data, transform);
        _geChannel.RaiseEvent(new GameEvent()
        {
            Origin = gameObject,
            Type = GameEventType.ItemPickedUp
        });
        Destroy(gameObject);
        //
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var entity = other.GetComponent<PlayerInventory>();

        if (entity  != null)
        {
            _playerInvNear = entity;

            _pickupHelperUI.SetPlayerItemCompare(_playerInvNear);

            _pickupHelperUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerInventory>())
        {
            _playerInvNear = null;
            _pickupHelperUI.SetActive(false);
        }
    }

    void Update()
    {
    }

    #region Setter & Getter
    internal SpellType GetSpellType()
    {
        return data.SpellType;
    }

    internal Sprite GetUISprite()
    {
        return data.GetSetSprite();
    }

    internal void SetData(PlayerItemSO item)
    {
        data = item;
    }
    #endregion //Setter & Getter
}
