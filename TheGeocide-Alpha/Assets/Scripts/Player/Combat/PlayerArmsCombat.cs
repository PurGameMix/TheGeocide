using Assets.Data.Items.Definition;
using Assets.Data.Player.PlayerSpells.Definition;
using Assets.Data.PlayerMouvement.Definition;
using Assets.Scripts.Player.Mouvement;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

/// <summary>
/// Handle melee (Back arm) and ranged (front arm) mechanics
/// </summary>
public class PlayerArmsCombat : MonoBehaviour
{
    [Header("Common")]
    //Common

    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;
    [SerializeField]
    private PlayerStateChannel _playerStateChannel;
    [SerializeField]
    private PlayerSpellStateChannel _playerSpellStateChannel;

    [SerializeField]
    private AudioController _playerAudioController;
    [SerializeField]
    private AudioChannel _guiAudioChannel;

    [SerializeField]
    private LayerMask _hitLayers;
    //Aiming
    [SerializeField]
    private Transform _cursorTransform;
    [SerializeField]
    private Transform _cursorCenter;
    [SerializeField]
    private float _cursorRadius = 0.8f;
    [SerializeField]
    private Rigidbody2D _rb;

    private PlayerInventory _inventory;
    private PlayerItemSO _meleeWeapon;
    private PlayerItemSO _rangedWeapon;
    private bool _isRangedAiming;
    private bool _isMeleeAiming;
    private bool _isCurrentlyAiming;


    private PlayerFacingController _facingController;
    private Vector3 mousePosition;
    private bool _IsAttackAvailable;
    [SerializeField]
    private Transform _spellPoint;
    [Header("Melee")]
    //Melee attack
    public float attackRange = 0.3f;
    [SerializeField]
    private Rig _meleeRigLayer;
    [SerializeField]
    private Rig _meleeAimingRigLayer;
    [SerializeField]
    private Transform _meleeAttackPoint;

    private float _lastMeleeAttackTimer = 0f;
    private int _meleeAttackDamage = 34;
    private float _meleeAttackCD = 1f;
    private float _meleeRecoveryDuration = 0.1f;
    [Header("Ranged")]

    [SerializeField]
    private Transform _rangedAttackPoint;
    [SerializeField]
    private Transform _rangedAimPoint;
    [SerializeField]
    private Rig _rangedAimingRigLayer;
    [SerializeField]
    private Rig _rangedRigLayer;

    //ranged attack
    private float _lastRangedAttackTimer = 0f;
    private int _rangedAttackDamage = 10;
    private float _rangedAttackCD = 1f;
    public float aimSpeed = 1f;
    private float _aimDuration = 0.3f;
    private ChannelSpell _currentChannelSpell;
    private float _currentCastTime = 0f;
    private AimAssist _castingUi;
    private bool _isCasting;
    // Start is called before the first frame update
    void Start()
    {
        //_hitLayers = LayerMask.GetMask("Enemy", "Obstacle", "Breakable");
        _lastMeleeAttackTimer = _meleeAttackCD;
        _inventory = GetComponent<PlayerInventory>();
        _meleeWeapon = _inventory.GetItem(ItemType.Melee);
        _rangedWeapon = _inventory.GetItem(ItemType.Ranged);
        _facingController = GetComponent<PlayerFacingController>();
        _playerAudioController = GetComponent<AudioController>();
        if (_meleeWeapon != null)
        {
            _meleeAttackDamage = _meleeWeapon.Damage;
            _meleeAttackCD = _meleeWeapon.CountDown;
        }

        if (_rangedWeapon != null)
        {
            _rangedAttackDamage = _rangedWeapon.Damage;
            _rangedAttackCD = _rangedWeapon.CountDown;
        }

        _meleeRigLayer.weight = 0;
        _rangedAimingRigLayer.weight = 0;


        InputHandler.instance.OnMeleeAttackTap += OnMeleeAttackTap;
        InputHandler.instance.OnMeleeAttackHoldStart += OnMeleeAttackHoldStart;
        InputHandler.instance.OnMeleeAttackHoldCancel += OnMeleeAttackHoldCancel;
        InputHandler.instance.OnRangedAttackTap += OnRangedAttackTap;
        InputHandler.instance.OnRangedAttackHoldStart += OnRangedAttackHoldStart;
        InputHandler.instance.OnRangedAttackHoldCancel += OnRangedAttackHoldCancel;
    }
    private void Awake()
    {
        _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
        _playerStateChannel.OnMouvementStateEnter += OnMouvementStateEnter;
    }

    private void OnDestroy()
    {
        _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
        _playerStateChannel.OnMouvementStateEnter -= OnMouvementStateEnter;
    }

    private bool PlayAudioImpact(string ImpactName)
    {
        _playerAudioController.Play(ImpactName);

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse
        mousePosition = Mouse.current.position.ReadValue();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        var distance = Vector2.Distance(_cursorCenter.position, mousePosition);
        if(distance < _cursorRadius)
        {
            Vector2 direction = mousePosition - _cursorCenter.position;
             Vector2 point = (Vector2)_cursorCenter.position + direction.normalized * _cursorRadius;
            _cursorTransform.position = Vector2.Lerp(transform.position, point, aimSpeed);
        }
        else
        {
            _cursorTransform.position = Vector2.Lerp(_cursorCenter.position, mousePosition, aimSpeed);
        }

        //Timers
        _lastMeleeAttackTimer += Time.deltaTime;
        _lastRangedAttackTimer += Time.deltaTime;

        if (_isCasting)
        {
            _currentCastTime += Time.deltaTime;
        }

        //Melee
        handleRigAimInteraction(_isMeleeAiming, _meleeAimingRigLayer);

        //Ranged
        handleRigAimInteraction(_isRangedAiming, _rangedAimingRigLayer);
    }

    #region Handle Attacks
    private void SetAiming()
    {
        if(!_isCurrentlyAiming && (_isMeleeAiming || _isRangedAiming))
        {
            var direction = _cursorTransform.position - transform.position;
            _isCurrentlyAiming = true;
            _playerStateChannel.RaiseAimingUpdate(new AimingStateEvent() {
                IsAiming = _isCurrentlyAiming,
                Direction = direction
            });
        }

        if (_isCurrentlyAiming && !_isMeleeAiming && !_isRangedAiming)
        {
            _isCurrentlyAiming = false;
            _playerStateChannel.RaiseAimingUpdate(new AimingStateEvent()
            {
                IsAiming = _isCurrentlyAiming
            });
        }

    }
    private bool IsMeleeAttackPossible()
    {
        return _meleeWeapon != null && _IsAttackAvailable && !_isRangedAiming;
    }
    private bool IsRangedAttackPossible()
    {
        return _rangedWeapon != null && _IsAttackAvailable && !_isMeleeAiming;
    }
    private void HandleTapAttack(PlayerItemSO weapon, PlayerStateType attackMelee_Release)
    {
        _playerStateChannel.RaiseOnStateEnter(new PlayerStateEvent(attackMelee_Release));
    }
    private void HandleHoldAttacks(PlayerItemSO weapon, PlayerStateType pst)
    {
        _playerStateChannel.RaiseOnStateEnter(new PlayerStateEvent(pst));
        _playerAudioController.Play(pst.GetAudioKey(), true);
        _playerSpellStateChannel.RaiseSpellChanged(new PlayerSpellStateEvent(weapon, SpellKeyActionType.Hold));


        if (weapon.SpellType.IsCastSpell())
        {
            _currentCastTime = 0;
            _isCasting = true;

            //Attack has special ingame GUI
            if (weapon.CastingUIPrefab != null)
            {
                _castingUi = Instantiate(weapon.CastingUIPrefab, GetUiInitPoint(weapon)).GetComponent<AimAssist>();
                _castingUi.SetCursor(_cursorTransform);
                _castingUi.SetCastTime(weapon.MaxCastTime);
            }
        }

        if (weapon.SpellType.IsChannelSpell())
        {
            HandleWeaponChannel();
        }
    }

    private void HandleOnCdBehavior()
    {
        _guiAudioChannel.RaiseAudioRequest(new AudioEvent("OnCD"));
        if (_castingUi != null)
        {
            Destroy(_castingUi.gameObject);
        }
    }
    private bool HandleHoldAttackCancel(PlayerItemSO weapon, PlayerStateType oldState, PlayerStateType newState, float lastAttackTimer)
    {
        var isAiming = false;
        _playerStateChannel.RaiseOnStateExit(new PlayerStateEvent(oldState));
        _playerAudioController.Stop(oldState.GetAudioKey(), true);
        _playerSpellStateChannel.RaiseSpellChanged(new PlayerSpellStateEvent(weapon, SpellKeyActionType.Cancel));


        if(lastAttackTimer < weapon.CountDown)
        {
            HandleOnCdBehavior();
            return false;
        }
        if (weapon.SpellType.IsCastSpell())
        {
            if (_currentCastTime >= weapon.MinCastTime)
            {
                AttackFired(weapon.Type);
                _playerStateChannel.RaiseOnStateEnter(new PlayerStateEvent(newState));
                _playerSpellStateChannel.RaiseSpellChanged(new PlayerSpellStateEvent(weapon, SpellKeyActionType.Release));
                _playerAudioController.Play(newState.GetAudioKey(), true);
                isAiming = true;
            }
            else
            {
                HandleOnCdBehavior();
                return false;
            }       
        }

        if (weapon.SpellType.IsChannelSpell())
        {
            if (_currentChannelSpell != null)
            {
                _currentChannelSpell.StopChannel(true);
                _currentChannelSpell = null;
            }
        }

        return isAiming;
    }

    private void AttackFired(ItemType type)
    {
        if(type == ItemType.Melee)
        {
            _lastMeleeAttackTimer = 0f;
            return;
        }

        _lastRangedAttackTimer = 0;
    }

    private void handleRigAimInteraction(bool isAiming, Rig aimingLayer)
    {
        if (isAiming)
        {
            var mouseOffset = _cursorTransform.position.x - transform.position.x;
            aimingLayer.weight += Time.deltaTime / _aimDuration;
            //Debug.Log(mouseOffset);
            _facingController.FlipAim(mouseOffset);
        }
        else
        {
            aimingLayer.weight -= Time.deltaTime / _aimDuration;
        }
    }
    void HandleWeaponChannel()
    {
        var go = Instantiate(_meleeWeapon.PrimaryEffectsPrefab, _meleeAttackPoint);
        _currentChannelSpell = go.GetComponent<ChannelSpell>();
        _currentChannelSpell.Init(_meleeWeapon);
        _currentChannelSpell.StartChannel();
    }
    private void HandleAttackAnimationCompleted(PlayerItemSO weapon, PlayerStateType pst)
    {
        _playerStateChannel.RaiseOnStateExit(new PlayerStateEvent(pst));

        if (!weapon.HasProjectile())
        {
            if (weapon.SpellType.IsCastSpell() && _castingUi != null)
            {
                _HandleFireSpell(_castingUi.GetPosition());
                _castingUi.Stop();
                Destroy(_castingUi.gameObject);
            }

            HandleAiming(weapon.Type, false);
            return;
        }

        if (_castingUi != null)
        {
            _castingUi.Stop();
            var angleWindow = _castingUi.GetAimWindow();

            //todo : handle nb projectiles!
            for (var i = 0; i < 3; i++)
            {
                StartCoroutine(FireProjectile(i, 0.3f, angleWindow));
            }

            StartCoroutine(DestroyAimUi(0.3f * 3 + 0.1f, weapon.Type)) ;
        }
        else
        {
            _HandleSingleProjectile();
            HandleAiming(weapon.Type, false);
        }
    }
    private IEnumerator DestroyAimUi(float delay, ItemType it)
    {
        yield return new WaitForSeconds(delay);
        Destroy(_castingUi.gameObject);

        HandleAiming(it, false);
    }
    private void HandleAiming(ItemType it, bool isAiming)
    {
        if (it == ItemType.Melee)
        {
            _isMeleeAiming = isAiming;
        }

        if (it == ItemType.Ranged)
        {
            _isRangedAiming = isAiming;
        }

        SetAiming();
    }
    private IEnumerator FireProjectile(int projectileIndex, float delay, float angleWindow)
    {
        yield return new WaitForSeconds(projectileIndex * delay);

        var rngAimingAngle = GetProjectileOrientation(projectileIndex, angleWindow);
        var prefab = Instantiate(_rangedWeapon.GetProjectile(), _rangedAttackPoint.position, _rangedAttackPoint.rotation);

        prefab.transform.Rotate(0.0f, 0.0f, rngAimingAngle, Space.Self);

        Projectile projectile = prefab.GetComponent<Projectile>();
        //Override projectile damage to ensure SO values are used
        projectile.dommage = _rangedAttackDamage;

        projectile.Fire(_rb.velocity.magnitude);
    }
    private float GetProjectileOrientation(int projectileIndex, float angleWindow)
    {
        if (angleWindow == 0)
        {
            return 0;
        }


        //Each project (3) has a segment on the angle
        //Rng is about previoussegment + rng (Exemple: For projectile2 & angleWindow = 90, angle = previousInterval + rng => angle = 30 + rng)
        //The angle is between angle/2 & -angle/2 (Exemple: for angleWindow = 90, max top angle = 45, max bot angle = -45, center always at 0)
        var interval = (int)angleWindow / 3;
        int rng = UnityEngine.Random.Range(0, interval);
        var pangle = rng + (interval * (projectileIndex)) - angleWindow / 2;


        //Debug.Log($"Angle{projectileIndex}: {pangle}, rng:{rng},  max: {angleWindow}");
        return pangle;
    }
    private void _HandleFireSpell(Transform spellPosition)
    {
        if (spellPosition == null)
        {
            _guiAudioChannel.RaiseAudioRequest(new AudioEvent("Error"));
            return;
        }

        var spell = Instantiate(_rangedWeapon.PrimaryEffectsPrefab, spellPosition.position, spellPosition.rotation).GetComponent<InstantSpell>();
        Debug.Log("Spelled" + spellPosition);
        spell.Init(_rangedWeapon);
    }
    private void _HandleSingleProjectile()
    {
        var prefab = Instantiate(_rangedWeapon.GetProjectile(), _rangedAttackPoint.position, _rangedAttackPoint.rotation);

        Projectile projectile = prefab.GetComponent<Projectile>();

        //Override projectile damage to ensure SO values are used
        projectile.dommage = _rangedAttackDamage;

        projectile.Fire(_rangedAimPoint, _rb.velocity.magnitude);

    }
    #endregion //Handle Attacks

    #region Melee

    private void OnMeleeAttackTap(InputHandler.InputArgs obj)
    {
        //Debug.Log(DateTime.UtcNow + ": Taped");
        if (!IsMeleeAttackPossible() || !_meleeWeapon.SpellType.IsInstantSpell())
        {
            return;
        }
        _lastMeleeAttackTimer = 0;
        HandleTapAttack(_meleeWeapon, PlayerStateType.AttackMelee_Release);        
    }

    private void OnMeleeAttackHoldStart(InputHandler.InputArgs obj)
    {
        //Debug.Log(DateTime.UtcNow + ": Holding");
        if (!IsMeleeAttackPossible() || _meleeWeapon.SpellType.IsInstantSpell())
        {
            return;
        }

        HandleHoldAttacks(_meleeWeapon, PlayerStateType.AttackMelee_Cast);
        _isMeleeAiming = true;
        SetAiming();
    }

    private void OnMeleeAttackHoldCancel(InputHandler.InputArgs obj)
    {
        if (!_isMeleeAiming)
        {
            return;
        }

        Debug.Log(DateTime.UtcNow + ": Holded");
        
        _isMeleeAiming = HandleHoldAttackCancel(_meleeWeapon, PlayerStateType.AttackMelee_Cast, PlayerStateType.AttackMelee_Release, _lastMeleeAttackTimer);
        SetAiming();
    }

    void LoadMeleeAttackData()
    {
        _meleeAttackDamage = _meleeWeapon.Damage;
        _meleeAttackCD = _meleeWeapon.CountDown;
    }
    #endregion //Melee

    #region Ranged

    private void OnRangedAttackTap(InputHandler.InputArgs obj)
    {
        //Debug.Log(DateTime.UtcNow + ": Taped");
        if (!IsRangedAttackPossible() || !_rangedWeapon.SpellType.IsInstantSpell())
        {
            return;
        }
        _lastRangedAttackTimer = 0;
        HandleTapAttack(_rangedWeapon, PlayerStateType.AttackRanged_Release);
    }

    private void OnRangedAttackHoldStart(InputHandler.InputArgs obj)
    {
        //Debug.Log(DateTime.UtcNow + ": Holding");

        if (!IsRangedAttackPossible() || _rangedWeapon.SpellType.IsInstantSpell())
        {
            return;
        }
        _isRangedAiming = true;
        SetAiming();
        HandleHoldAttacks(_rangedWeapon, PlayerStateType.AttackRanged_Cast);
    }

    private void OnRangedAttackHoldCancel(InputHandler.InputArgs obj)
    {
        if (!_isRangedAiming)
        {
            return;
        }

        //Debug.Log(DateTime.UtcNow + ": Holded");       
        _isRangedAiming = HandleHoldAttackCancel(_rangedWeapon, PlayerStateType.AttackRanged_Cast, PlayerStateType.AttackRanged_Release, _lastRangedAttackTimer);
        SetAiming();
    }

    private Transform GetUiInitPoint(PlayerItemSO weapon)
    {
        if (weapon.CastingUIType == CastingUIType.Foot)
        {
            return _spellPoint;
        }

        return GetUiDefaultPoint(weapon.Type);
    }

    private Transform GetUiDefaultPoint(ItemType type)
    {
        if(type == ItemType.Melee)
        {
            return _meleeAttackPoint;
        }

        if (type == ItemType.Ranged)
        {
            return _rangedAttackPoint;
        }

        return transform;
    }

    void LoadRangedAttackData()
    {
        _rangedAttackDamage = _rangedWeapon.Damage;
        _rangedAttackCD = _rangedWeapon.CountDown;
    }
    #endregion //Ranged

    #region Events
    private void OnInventoryChanged(PlayerItemSO so)
    {
        if (so.Type == ItemType.Melee)
        {
            _meleeWeapon = so;
            LoadMeleeAttackData();
            return;
        }

        if (so.Type == ItemType.Ranged)
        {
            _rangedWeapon = so;
            LoadRangedAttackData();
            return;
        }

    }

    private void OnMouvementStateEnter(PlayerStateEvent mvtEvt)
    {
        if (!mvtEvt.Type.IsStandardState())
        {
            return;
        }
        _IsAttackAvailable = mvtEvt.Type.IsAttackPossible();
        //Debug.Log($"_IsAttackAvailable : {_IsAttackAvailable}, {mvtEvt.Type}");
    }

    //Event from Animator trigger
    public void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_meleeAttackPoint.position, attackRange, _hitLayers);

        foreach (Collider2D collider in hitEnemies)
        {
            var canBeDamageObj = collider.GetComponent<ICanBeDamaged>();
            if (canBeDamageObj != null)
            {
                canBeDamageObj.TakeDamage(_meleeAttackDamage, HealthEffectorType.player);
                continue;
            }
        }

        //punch in air
        if (hitEnemies.Length == 0 && _meleeWeapon == null)
        {
            PlayAudioImpact("PunchAir");
        }
    }

    //Event from Animator trigger
    public void MeleeAttackCompleted()
    {
        _playerStateChannel.RaiseOnStateExit(new PlayerStateEvent(PlayerStateType.AttackMelee_Release));
    }

    public void RangedAttackBegin()
    {
    }


    public void RangedAttackCompleted()
    {
        HandleAttackAnimationCompleted(_rangedWeapon, PlayerStateType.AttackRanged_Release);
    }
    #endregion //Events 


    private void OnDrawGizmosSelected()
    {
        if (_rangedAttackPoint != null)
        {
            Gizmos.DrawWireSphere(_rangedAttackPoint.position, attackRange);
        }


        if (_meleeAttackPoint != null)
        {
            Gizmos.DrawWireSphere(_meleeAttackPoint.position, attackRange);
        }

    }
}
