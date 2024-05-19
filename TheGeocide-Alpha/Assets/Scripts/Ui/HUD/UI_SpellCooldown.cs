using Assets.Data.Items.Definition;
using Assets.Data.Player.PlayerSpells.Definition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpellCooldown : MonoBehaviour
{

    public ItemType ItemSlot;

    [SerializeField]
    private Sprite _emptySlotSprite;

    [SerializeField]
    private Image _spellImage;

    [SerializeField]
    private Image _cooldownImage;

    [SerializeField]
    private Image _castingImage;

    [SerializeField]
    private TMP_Text _textCooldown;

    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;
    [SerializeField]
    private PlayerSpellStateChannel _playerSpellStateChannel;


    private bool isCoolingdown = false;
    private float _cooldownTime = 1;
    private float _currentcoolDownTime = 0;

    private float _channelingTime = 0.5f;
    private float _castingTime = 3;
    private float _currentcastingTime = 3;
    private bool _isChanneling;
    private bool _isCasting;



    private void Awake()
    {
        _spellImage.sprite = _emptySlotSprite;
        _textCooldown.gameObject.SetActive(false);
        _cooldownImage.fillAmount = 0;
        _castingImage.fillAmount = 0;
        _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
        _playerSpellStateChannel.OnSpellStateChanged += OnSpellStateChanged;
    }


    private void OnDestroy()
    {
        _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
        _playerSpellStateChannel.OnSpellStateChanged -= OnSpellStateChanged;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleCooldown();
        HandleCastingGauge();
        HandleChanneling();
    }

    private void HandleCastingGauge()
    {
        if (!_isCasting)
        {
            return;
        }

        _currentcastingTime += Time.deltaTime;

        if (_currentcastingTime <= _castingTime)
        {
            _castingImage.fillAmount = _currentcastingTime / _castingTime;
        }
    }

    private void HandleChanneling()
    {
        if (!_isChanneling)
        {
            return;
        }

        _currentcastingTime += Time.deltaTime;

        if (_currentcastingTime >= _channelingTime)
        {
            _castingImage.fillAmount = 0;
            _currentcastingTime = 0;
        }
        else
        {
            _castingImage.fillAmount = _currentcastingTime / _channelingTime;
        }
    }

    private void HandleCooldown()
    {
        if (!isCoolingdown)
        {
            return;
        }

        _currentcoolDownTime -= Time.deltaTime;

        if (_currentcoolDownTime <= 0)
        {
            isCoolingdown = false;
            _textCooldown.gameObject.SetActive(false);
            _cooldownImage.fillAmount = 0.0f;
        }
        else
        {
            _textCooldown.text = Mathf.RoundToInt(_currentcoolDownTime).ToString();
            _cooldownImage.fillAmount = _currentcoolDownTime / _cooldownTime;
        }
    }

    #region Events
    private bool IsOtherItemSlot(ItemType itemType)
    {
        return ItemSlot != itemType;
    }

    private void OnInventoryChanged(PlayerItemSO item)
    {
        if (IsOtherItemSlot(item.Type))
        {
            return;
        }

        _castingTime = item.MaxCastTime;
        _cooldownTime = item.CountDown;
        _spellImage.sprite = item.GetSetSprite();

        _cooldownImage.fillAmount = 0;
        _castingImage.fillAmount = 0;

        if(_cooldownTime < 10)
        {
            _textCooldown.enabled = false;
        }

        StartCoolDown();
    }

    private void OnSpellStateChanged(PlayerSpellStateEvent psse)
    {

        if (IsOtherItemSlot(psse.Type))
        {
            return;
        }

        if (psse.Action == SpellKeyActionType.Release)
        {
            //Debug.Log("Release");
            StartCoolDown();
        }

        if (psse.Action == SpellKeyActionType.Hold)
        {
            //Debug.Log("Hold");
            ToggleCastingGauge(psse.SpellType, true);
        }

        if (psse.Action == SpellKeyActionType.Cancel)
        {
            //Debug.Log("Cancel");
            ToggleCastingGauge(psse.SpellType, false);
        }
    }
    #endregion

    private void ResetCasting()
    {
        _castingImage.fillAmount = 0;
        _isCasting = false;
        _currentcastingTime = 0;
    }
    private bool StartCoolDown()
    {
        if (isCoolingdown)
        {
            return false;
        }

        //Reset casting
        ResetCasting();

        isCoolingdown = true;
        _textCooldown.gameObject.SetActive(true);
        _currentcoolDownTime = _cooldownTime;
        return true;
    }
    private void ToggleCastingGauge(SpellType sp, bool toogle)
    {

        _castingImage.fillAmount = 0;
        _currentcastingTime = 0;


        if (sp == SpellType.Channel)
        {

            _isChanneling = toogle;
            return;
        }

        if (sp == SpellType.Casting)
        {
            _isCasting = toogle;
            return;
        }
    }

}
