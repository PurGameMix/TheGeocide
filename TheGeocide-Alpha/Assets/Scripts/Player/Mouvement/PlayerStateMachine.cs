using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Data.Items.Definition;
using Assets.Scripts.Player.Mouvement;
using Assets.Data.Common.Definition;
using Assets.Scripts.Player.Animations.Rigging;
using Assets.Data.PlayerMouvement.Definition;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private PlayerMouvementSO data;
    [SerializeField]
    private SmokeEffect _playerFallSmoke;
    [SerializeField]
    private PlayerFacingController _facingController;
    [SerializeField]
    internal PlayerRopePhysicsController _ropeController;
    public float SlopeFactor;

    internal int _currentHealth;
    private bool _isLoadingComplete;
    [Header("Channels")]
    [SerializeField]
    internal PlayerStateChannel _playerStateChannel;
    [SerializeField]
    private AudioChannel _guiAudioChannel;
    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;

    [SerializeField]
    private GameStateChannel _gameStateChannel;


    #region STATE MACHINE
    public MouvementStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }

    public PlayerSlopeSlideState SlopeSlideState { get; private set; }

    public PlayerIdleCrouchState IdleCrouchState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    public PlayerRunState RunState { get; private set; }

    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerIdleSwimState IdleSwimState { get; private set; }
    public PlayerSwimState SwimState { get; private set; }

    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerLandingState LandingState { get; private set; }
    public PlayerLandedState LandedState { get; private set; }

    public PlayerUppercutState UppercutState { get; private set; }
    public PlayerUltimateState UltimateState { get; private set; }

    public PlayerClimbingState ClimbState { get; private set; }
    public PlayerIdleClimbState IdleClimbState { get; private set; }

    public PlayerEdgeClimbState EdgeClimbState { get; private set; }

    public PlayerRopeState RopeState { get; private set; }
    public PlayerIdleRopeState IdleRopeState { get; private set; }

    public PlayerRopeSwingState RopeSwingState { get; private set; }
    public PlayerZiplineState ZipState { get; private set; }

    public PlayerDeadState DeadState { get; private set; }
    public PlayerHurtedState HurtedState { get; private set; }

    public PlayerStunnedState StunnedState { get; private set; }

    public PlayerFallDeadState FallDeadState { get; private set; }
    public PlayerFallHurtedState FallHurtedState { get; private set; }

    public PlayerAttackMeleeState AttackMeleeState { get; private set; }
    public PlayerAttackRangedState AttackRangedState { get; private set; }

    public PlayerPushingState PushingState { get; private set; }


    //[ReadOnly]
    public string CurrentState;
    #endregion

    #region COMPONENTS
    [SerializeField]
    internal Rigidbody2D RB;
    [SerializeField]
    private CapsuleCollider2D _collider;
    [SerializeField]
    private CapsuleCollider2D _detectioncollider;
    #endregion

    #region STATE PARAMETERS

    private bool isGround;
    public float LastFallPower { get; private set; }

    public float LastOnGroundTime { get; set; }
    public float LastInWaterTime { get; private set; }
    public float LastOnSlopeTime { get; private set; }
    public float LastOnWallTime { get; set; }
    internal WallJumpHitState LastWallJumpHit { get; set; }

    public float LastOnClimbTime { get; set; }


    internal Vector2 GetZipCheckPoint()
    {
        return _zipCheckPoint.position;

    }

    public float LastOnRopeTime { get; private set; }
    public float LastOnZipTime { get; private set; }

    internal bool isOnZip { get; set; }
    internal Zipline currentZipline;

    public float LastOnPushTime { get; private set; }
    public float LastWoundedTime { get; private set; }
    public float LastFallImpactTime { get; private set; }

    internal Vector2 _stunDirection;

    public float LastStunTime { get; private set; }
    #endregion

    #region Items parameters

    internal Dictionary<ItemType, PlayerItemSO> _itemMap = new Dictionary<ItemType, PlayerItemSO>();
    private int _ultimateStacks = 10;
    internal float _landingRecoverTime = 1;
    internal float _ultimateRecoverTime = 1.15f;
    #endregion

    #region INPUT PARAMETERS
    public float LastPressedJumpTime { get; private set; }
    public float LastPressedDashTime { get; private set; }
    public float LastLandingAttackTime { get; private set; }
    public float LastUppercutAttackTime { get; private set; }
    public float LastUltimateAttackTime { get; private set; }

    #endregion

    #region CHECK PARAMETERS
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _wallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize;
    [Space(5)]
    [SerializeField] private Transform _climbCheckPoint;
    [SerializeField] private Vector2 _climbCheckSize;

    [Space(5)]
    [SerializeField] private Transform _zipCheckPoint;
    [SerializeField] private Vector2 _zipCheckSize;

    [SerializeField] internal Transform _slopeCheckPoint;
    [SerializeField] private Vector2 _slopeCheckSize;
    [Space(5)]

    [SerializeField] internal Transform _waterCheckPoint;
    [SerializeField] private Vector2 _waterCheckSize;
    [Space(5)]
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
    [SerializeField] internal LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _movingLayers;
    [SerializeField] private LayerMask _climbLayer;
    [SerializeField] private LayerMask _ropeLayer;
    [SerializeField] private LayerMask _zipLayer;
    [SerializeField] private LayerMask _pushableLayer;
    [SerializeField] internal LayerMask _slopLayer;
    [SerializeField] internal LayerMask _waterLayer;

    private static Dictionary<HealthEffectorType, float> _dmgStunDurationMap = new Dictionary<HealthEffectorType, float>()
    {
        {HealthEffectorType.fall, 1.5f },
        {HealthEffectorType.enemy, 0.3f },
        {HealthEffectorType.trap, 0.3f },
    };
    #endregion

    #region Debug
    //private Vector2 _origin;
    //private Vector2 _targetPoint;
    //private Vector2 _hitPoint;
    #endregion //Debug
    private static int _pushRatio = 9;

    private Vector2 LastMovingObjectVelocity;
    private CollisionBlockerSide _hasHitBlocage = CollisionBlockerSide.None;
    internal bool isOnEdge = false;
    internal Vector2 EdgePosition;
    private void Awake()
    {

        #region STATE MACHINE
        StateMachine = new MouvementStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, data);
        SlopeSlideState = new PlayerSlopeSlideState(this, StateMachine, data);
        CrouchState = new PlayerCrouchWalkState(this, StateMachine, data);
        IdleCrouchState = new PlayerIdleCrouchState(this, StateMachine, data);
        RunState = new PlayerRunState(this, StateMachine, data);
        JumpState = new PlayerJumpState(this, StateMachine, data);
        InAirState = new PlayerInAirState(this, StateMachine, data);
        IdleSwimState = new PlayerIdleSwimState(this, StateMachine, data);
        SwimState = new PlayerSwimState(this, StateMachine, data);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, data);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, data);
        DashState = new PlayerDashState(this, StateMachine, data);
        LandingState = new PlayerLandingState(this, StateMachine, data);
        LandedState = new PlayerLandedState(this, StateMachine, data);
        UppercutState = new PlayerUppercutState(this, StateMachine, data);
        UltimateState = new PlayerUltimateState(this, StateMachine, data);
        ClimbState = new PlayerClimbingState(this, StateMachine, data);
        IdleClimbState = new PlayerIdleClimbState(this, StateMachine, data);
        EdgeClimbState = new PlayerEdgeClimbState(this, StateMachine, data);
        RopeState = new PlayerRopeState(this, StateMachine, data);
        IdleRopeState = new PlayerIdleRopeState(this, StateMachine, data);
        RopeSwingState = new PlayerRopeSwingState(this, StateMachine, data);
        ZipState = new PlayerZiplineState(this, StateMachine, data);

        DeadState = new PlayerDeadState(this, StateMachine, data);
        HurtedState = new PlayerHurtedState(this, StateMachine, data);
        FallDeadState = new PlayerFallDeadState(this, StateMachine, data);
        FallHurtedState = new PlayerFallHurtedState(this, StateMachine, data);

        AttackMeleeState = new PlayerAttackMeleeState(this, StateMachine, data);
        AttackRangedState = new PlayerAttackRangedState(this, StateMachine, data);

        PushingState = new PlayerPushingState(this, StateMachine, data);
        StunnedState = new PlayerStunnedState(this, StateMachine, data);
        #endregion

        #region Events registration
        _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
        _gameStateChannel.OnHealthPointAnswered += OnHealthPointAnswered;
        _gameStateChannel.OnHealthChanged += OnHealthChanged;
        _playerStateChannel.OnStateEffectorTriggered += OnStateEffectorTriggered;
        _playerStateChannel.OnEdgeDetected += OnEdgeDetected;
        #endregion
    }

    private void OnDisable()
    {
        StateMachine.ChangeState(IdleState);
    }

    private void Start()
    {
        InputHandler.instance.OnDeplacement += OnDeplacement;
        InputHandler.instance.OnJumpPressed += OnJump;
        //InputHandler.instance.OnJumpReleased += OnJumpUp;
        InputHandler.instance.OnDash += OnDash;
        InputHandler.instance.OnLandingAttack += OnLandingAttack;
        InputHandler.instance.OnUppercutAttack += OnUppercutAttack;
        InputHandler.instance.OnUltimateAttack += OnUltimateAttack;
        StateMachine.Initialize(this, IdleState);

        SetGravityScale(data.gravityScale);
        isGround = false;
        _gameStateChannel.RaisedHealthPointRequest();
    }

    private void OnDestroy()
    {
        InputHandler.instance.OnDeplacement -= OnDeplacement;
        InputHandler.instance.OnJumpPressed -= OnJump;
        //InputHandler.instance.OnJumpReleased -= OnJumpUp;
        InputHandler.instance.OnDash -= OnDash;

        _gameStateChannel.OnHealthChanged -= OnHealthChanged;
        _gameStateChannel.OnHealthPointAnswered -= OnHealthPointAnswered;
        _playerStateChannel.OnStateEffectorTriggered -= OnStateEffectorTriggered;
        _playerStateChannel.OnEdgeDetected -= OnEdgeDetected;

        _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
    }

    private void Update()
    {
        if (!_isLoadingComplete)
        {
            return;
        }

        StateMachine.CurrentState.LogicUpdate();

        #region CHECKS
        LastOnGroundTime -= Time.deltaTime;
        LastOnSlopeTime -= Time.deltaTime;
        LastInWaterTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;

        LastOnClimbTime -= Time.deltaTime;

        LastOnRopeTime -= Time.deltaTime;
        LastOnZipTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;

        LastLandingAttackTime -= Time.deltaTime;
        LastUppercutAttackTime -= Time.deltaTime;
        LastUltimateAttackTime -= Time.deltaTime;

        LastWoundedTime -= Time.deltaTime;
        LastFallImpactTime -= Time.deltaTime;
        LastStunTime -= Time.deltaTime;
        LastOnPushTime -= Time.deltaTime;
        //Ground Check

        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))//checks if set box overlaps with ground
        {
            if (!isGround)
            {
                isGround = true;



                var fallDmg = GetFallDamage();
                if (fallDmg > 0)
                {
                    _playerStateChannel.RaiseTakeDamageRequest(new PlayerTakeDamageEvent() { Damage = fallDmg, Origin = HealthEffectorType.fall });
                    LastFallImpactTime = _dmgStunDurationMap[HealthEffectorType.fall] + data.coyoteTime;
                }
            }
            else
            {
                //Debug.Log("ImpactReset" + LastFallPower);
                LastFallPower = 0;
            }
            LastOnGroundTime = data.coyoteTime; //if so sets the lastGrounded to coyoteTime


            //Check moving ground
            var hitMovingObject = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _movingLayers);
            if (hitMovingObject)
            {
                var rb = hitMovingObject.GetComponent<Rigidbody2D>();
                LastMovingObjectVelocity = rb?.velocity ?? Vector2.zero;
            }
            else
            {
                LastMovingObjectVelocity = Vector2.zero;
            }
        }
        else
        {

            if (RB.velocity.y < 0)
            {
                LastFallPower = -RB.velocity.y;
                //Debug.Log("fallpower:" + LastFallPower);
            }
            else
            {
                LastFallPower = 0;
            }


            //Debug.Log(DateTime.Now + ": GroundLost");
            isGround = false;
            LastMovingObjectVelocity = Vector2.zero;
        }

        //Wall Check
        var overlapInfo = Physics2D.OverlapBox(_wallCheckPoint.position, _wallCheckSize, 0, _wallLayer);
        if (overlapInfo)
        {
            //Debug.Log(DateTime.Now + ": LeftWallHit");
            LastOnWallTime = data.coyoteTime;
        }

        if (Physics2D.OverlapBox(_climbCheckPoint.position, _climbCheckSize, 0, _climbLayer))
        {
            LastOnClimbTime = data.coyoteTime;
        }

        //RopeClimb Check
        var ropeHit = Physics2D.OverlapBox(_climbCheckPoint.position, _climbCheckSize, 0, _ropeLayer);
        if (ropeHit && !_ropeController.IsOnRope())
        {
            //Debug.Log(DateTime.Now + ": ropeHit");
            LastOnRopeTime = data.coyoteTime;

            var hitLink = ropeHit.GetComponent<Rigidbody2D>();
            _ropeController.RegisterValidRopeLink(hitLink);
        }

        var zipHit = Physics2D.OverlapBox(_zipCheckPoint.position, _zipCheckSize, 0, _zipLayer);
        //ZipLine Check
        if (!isOnZip && zipHit)
        {
            Debug.Log(DateTime.Now + ": zipHit");
            LastOnZipTime = data.coyoteTime;
            currentZipline = zipHit.GetComponentInParent<Zipline>();
            currentZipline.SetAnchor(_zipCheckPoint.position);
        }

        if (!isOnZip && LastOnZipTime < 0)
        {
            CleanCurrentZipline();
        }


        //Pushable Check
        if (Physics2D.OverlapBox(_wallCheckPoint.position, _wallCheckSize, 0, _pushableLayer))
        {
            //Debug.Log(DateTime.Now + ": LeftWallHit");
            LastOnPushTime = 0;
        }

        //Check sloppy ground
        if (Physics2D.OverlapBox(_slopeCheckPoint.position, _slopeCheckSize, 0, _slopLayer))
        {
            LastOnSlopeTime = data.coyoteTime;
        }

        //Check in water
        if (Physics2D.OverlapBox(_waterCheckPoint.position, _waterCheckSize, 0, _waterLayer))
        {
            LastInWaterTime = data.coyoteTime;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #region INPUT CALLBACKS
    //These functions are called when an even is triggered in my InputHandler. You could call these methods through a if(Input.GetKeyDown) in Update

    public void OnDeplacement(InputHandler.InputArgs args)
    {
        if (InputHandler.instance.MoveInput.x != 0
            && !StateMachine.CurrentState.GetMouvementType().IsTurnForbidden())
        {
            _facingController.CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
        }

        //if (Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _pushableLayer))
        //{
        //    //Debug.Log(DateTime.Now + ": LeftWallHit");
        //    LastOnPushTime = 0;
        //}
        //else
        //{
        //    LastOnPushTime = -data.coyoteTime;
        //}
    }

    public void OnJump(InputHandler.InputArgs args)
    {
        LastPressedJumpTime = data.jumpBufferTime;
    }

    public void OnJumpUp(InputHandler.InputArgs args)
    {
        if (JumpState.CanJumpCut() || WallJumpState.CanJumpCut())
            JumpCut();
    }

    public void OnDash(InputHandler.InputArgs args)
    {
        LastPressedDashTime = data.dashBufferTime;
    }


    private bool IsAbilityOnCD(ItemType type, float currentAbilityTime)
    {
        if (!_itemMap.ContainsKey(type))
        {
            return true;
        }

        return currentAbilityTime > -_itemMap[type].CountDown;
    }

    public void OnLandingAttack(InputHandler.InputArgs args)
    {
        if (IsAbilityOnCD(ItemType.Landing, LastLandingAttackTime))
        {

            _guiAudioChannel.RaiseAudioRequest(new AudioEvent("OnCD"));
            return;
        }

        LastLandingAttackTime = data.dashBufferTime;
    }

    public void OnUppercutAttack(InputHandler.InputArgs args)
    {
        if (IsAbilityOnCD(ItemType.Uppercut, LastUppercutAttackTime))
        {
            _guiAudioChannel.RaiseAudioRequest(new AudioEvent("OnCD"));
            return;
        }

        LastUppercutAttackTime = data.dashBufferTime;
    }

    public void OnUltimateAttack(InputHandler.InputArgs args)
    {
        if (IsAbilityOnCD(ItemType.Ultimate, LastUltimateAttackTime) || _ultimateStacks <= 0)
        {
            _guiAudioChannel.RaiseAudioRequest(new AudioEvent("OnCD"));
            return;
        }

        _ultimateStacks--;
        LastUltimateAttackTime = data.dashBufferTime;
    }
    #endregion

    #region Facing methods

    public void CheckDirectionToFace(bool wantToFaceRight)
    {
        _facingController.CheckDirectionToFace(wantToFaceRight);
    }

    internal bool IsFacingRight()
    {
        return _facingController.IsFacingRight();
    }

    internal bool IsAiming()
    {
        return _facingController.IsAiming();
    }
    #endregion //Facing methods

    #region MOVEMENT METHODS

    internal void SetCollider(bool isCrouchWanted)
    {
        if (isCrouchWanted)
        {
            _collider.offset = data.CrouchOffset;
            _collider.size = data.CrouchSize;

            _detectioncollider.offset = data.CrouchOffset;
            _detectioncollider.size = data.CrouchSize;        
            return;
        }
        _collider.offset = data.StandOffset;
        _collider.size = data.StandSize;

        _detectioncollider.offset = data.CrouchOffset;
        _detectioncollider.size = data.CrouchSize;

    }

    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    public void Drag(float amount)
    {
        Vector2 force = amount * RB.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(RB.velocity.x), Mathf.Abs(force.x)); //ensures we only slow the player down, if the player is going really slowly we just apply a force stopping them
        force.y = Mathf.Min(Mathf.Abs(RB.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(RB.velocity.x); //finds direction to apply force
        force.y *= Mathf.Sign(RB.velocity.y);

        //Debug.Log("Drag");
        RB.AddForce(-force, ForceMode2D.Impulse); //applies force against movement direction
    }


    internal void Swim(int lerpAmount)
    {
        float targetSpeed = InputHandler.instance.MoveInput.x * data.swimMaxSpeed;
        Deplacement(lerpAmount, targetSpeed, true, PlayerMovementType.Swim);
    }

    private float GetMovementSpeed()
    {
        if (!_facingController.IsAiming())
        {
            return data.runMaxSpeed;
        }

        var aimOppositeToMovementDirection = Mathf.Sign(InputHandler.instance.MoveInput.x) != Mathf.Sign(_facingController.GetAimingDirection().x);
        if (aimOppositeToMovementDirection)
        {
            return data.backwardCastSpeed;
        }

        return data.forwardCastSpeed;
    }
    public void Run(float lerpAmount)
    {

        float targetSpeed = InputHandler.instance.MoveInput.x * GetMovementSpeed();

        if (_hasHitBlocage == CollisionBlockerSide.Left && InputHandler.instance.MoveInput.x < 0
            || _hasHitBlocage == CollisionBlockerSide.Right && InputHandler.instance.MoveInput.x > 0
        )
        {
            targetSpeed = 0;
        }

        Deplacement(lerpAmount, targetSpeed, true, LastOnGroundTime > 0 ? PlayerMovementType.Run : PlayerMovementType.InAir);
    }
    public void Crouch(float lerpAmount)
    {
        float targetSpeed = InputHandler.instance.MoveInput.x * data.crouchMaxSpeed;

        if (_hasHitBlocage == CollisionBlockerSide.Left && InputHandler.instance.MoveInput.x < 0
            || _hasHitBlocage == CollisionBlockerSide.Right && InputHandler.instance.MoveInput.x > 0
        )
        {
            targetSpeed = 0;
        }

        Deplacement(lerpAmount, targetSpeed, true, PlayerMovementType.Crouch);
    }


    public void Push(float lerpAmount)
    {
        float targetSpeed = InputHandler.instance.MoveInput.x * data.runMaxSpeed;

        //Ensuring force is not apply when player change direction while pushing
        if (LastOnPushTime == 0)
        {
            targetSpeed = targetSpeed * _pushRatio; //apply a ratio to help pushing heavy objects
        }

        Deplacement(lerpAmount, targetSpeed, true, PlayerMovementType.Run);
    }

    public void Deplacement(float lerpAmount, float targetSpeed, bool checkDirection, PlayerMovementType moveType = PlayerMovementType.Run)
    {

        if (LastMovingObjectVelocity != Vector2.zero)
        {
            RB.velocity = new Vector2(LastMovingObjectVelocity.x, RB.velocity.y);
            targetSpeed += RB.velocity.x;
        }

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = GetMovement(lerpAmount, targetSpeed, moveType);

        //Debug.Log("Run");

        RB.AddForce(movement * Vector2.right); //applies force to rigidbody, multiplying by Vector2.right so that it only affects X axis 

        //if (InputHandler.instance.MoveInput.x != 0 && checkDirection)
        //    CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
    }

    public void SetBlockage(CollisionBlockerSide side2D)
    {
        _hasHitBlocage = side2D;
    }

    private float GetMovement(float lerpAmount, float targetSpeed, PlayerMovementType moveType)
    {
        float accelRate;
        //gets an acceleration value based on if we are accelerating (includes turning) or trying to stop (decelerating). As well as applying a multiplier if we're air borne
        if (moveType == PlayerMovementType.InAir)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.GetAccel(PlayerMovementType.Run) * data.accelInAir : data.GetAccel(PlayerMovementType.Run, true) * data.deccelInAir;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.GetAccel(moveType) : data.GetAccel(moveType, true);

        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = data.stopPower;
        }
        else if (Mathf.Abs(RB.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(RB.velocity.x)))
        {
            velPower = data.turnPower;
        }
        else
        {
            velPower = data.accelPower;
        }

        float speedDif = targetSpeed - RB.velocity.x; //calculate difference between current velocity and desired velocity
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        return Mathf.Lerp(RB.velocity.x, movement, lerpAmount);
    }

    public void Stun()
    {
        Deplacement(0.2f, 0, false);
    }

    public void Jump()
    {
        //ensures we can't call a jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnSlopeTime = 0;
        #region Perform Jump
        float adjustedJumpForce = data.jumpForce;
        if (RB.velocity.y < 0)
            adjustedJumpForce -= RB.velocity.y;

        //Debug.Log("Jump");
        RB.AddForce(Vector2.up * adjustedJumpForce, ForceMode2D.Impulse);
        #endregion
    }

    public void WallJump(int dir)
    {
        //ensures we can't call a jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnSlopeTime = 0;
        LastOnWallTime = 0;


        #region Perform Wall Jump
        Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= RB.velocity.y;

        //Debug.Log("WallJump");
        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }

    internal bool CanWallJump()
    {
        //Only 1 wall jump by side
        if (LastWallJumpHit == WallJumpHitState.None ||
            _facingController.IsFacingRight() && LastWallJumpHit == WallJumpHitState.Left ||
            !_facingController.IsFacingRight() && LastWallJumpHit == WallJumpHitState.Right
            )
        {
            return true;
        }
        return false;
    }

    private void JumpCut()
    {
        //Debug.Log("JumpCut");
        //applies force downward when the jump button is released. Allowing the player to control jump height
        RB.AddForce(Vector2.down * RB.velocity.y * (1 - data.jumpCutMultiplier), ForceMode2D.Impulse);
    }

    public void Slide()
    {
        //works the same as the Run but only in the y-axis
        float targetSpeed = 0;
        float speedDif = targetSpeed - RB.velocity.y;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * data.slideAccel, data.slidePower) * Mathf.Sign(speedDif);
        //Debug.Log("Slide");
        RB.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }

    private Vector2 AdjustVelocityToSlope(Vector2 velocity, float direction)
    {
        //Todo? Implem zipline behavior?
        var normal = new Vector2(direction, -1);
        Vector2 origin = direction > 0 ? Vector2.right : Vector2.left;
        var slopeRotation = Quaternion.FromToRotation(origin, normal);
        var adjustedVelocity = slopeRotation * velocity;

        Debug.Log($"x:{adjustedVelocity.x},y:{adjustedVelocity.y}");
        return adjustedVelocity;
    }

    internal void SlopeSlide(float lerpAmount, float direction)
    {
        float targetSpeed = direction * data.slopeMaxSpeed;
        float movement = GetMovement(lerpAmount, targetSpeed, PlayerMovementType.Run);

        //Debug.Log("Run");
        Vector2 velocity = new Vector2(movement, 0);
        velocity = AdjustVelocityToSlope(velocity, direction);
        RB.AddForce(velocity);

        _facingController.CheckDirectionToFace(direction > 0);
    }

    public void Dash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;
        LastOnSlopeTime = 0;

        RB.velocity = dir.normalized * data.dashSpeed;

        SetGravityScale(0);
    }

    public void Uppercut(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastUppercutAttackTime = 0;
        LastOnSlopeTime = 0;

        RB.velocity = dir.normalized * data.dashSpeed;
        SetGravityScale(0);
    }



    #region RopeClimb
    internal void Climb(int lerpAmount)
    {
        float targetSpeed = InputHandler.instance.MoveInput.y * data.climbMaxSpeed; //calculate the direction we want to move in and our desired velocity

        float speedDif = targetSpeed - RB.velocity.y; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;

        //gets an acceleration value based on if we are accelerating (includes turning) or trying to decelerate (stop). As well as applying a multiplier if we're air borne
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.climbAccel : data.climbDeccel;

        //if we want to climb but are already going faster than max climb speed
        if (((RB.velocity.y > targetSpeed && targetSpeed > 0.01f) || (RB.velocity.y < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = data.stopPower;
        }
        else if (Mathf.Abs(RB.velocity.y) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(RB.velocity.y)))
        {
            velPower = data.turnPower;
        }
        else
        {
            velPower = data.accelPower;
        }
        #endregion

        // applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        //movement = Mathf.Lerp(RB.velocity.y, movement, lerpAmount); // lerp so that we can prevent the Run from immediately slowing the player down, in some situations eg wall jump, dash 

        //Debug.Log($"Velocity: {RB.velocity.y} || movement: {movement}");
        //if(movement < 20)
        //      {
        //	movement = 0;
        //      }
        RB.AddForce(movement * Vector2.up); // applies force force to rigidbody, multiplying by Vector2.right so that it only affects Y axis 

        //on the climb we are not moving
        //SetGravityScale(InputHandler.instance.MoveInput.y == 0 ? -1 : data.gravityScale);
    }

    public bool IsOnRope()
    {
        return _ropeController.IsOnRope();
    }

    public bool CanCatchRope()
    {
        return _ropeController.CanCatchRope();
    }

    public void RopeSwing(float lerpAmount)
    {
        float targetSpeed = InputHandler.instance.MoveInput.x * data.climbMaxSpeed; //calculate the direction we want to move in and our desired velocity
        float speedDif = targetSpeed - RB.velocity.x; //calculate difference between current velocity and desired velocity

        #region Acceleration Rate
        float accelRate;

        //gets an acceleration value based on if we are accelerating (includes turning) or trying to stop (decelerating). As well as applying a multiplier if we're air borne
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.deccelInAir;

        //if we want to run but are already going faster than max run speed
        if (((RB.velocity.x > targetSpeed && targetSpeed > 0.01f) || (RB.velocity.x < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
        {
            accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = data.stopPower;
        }
        else if (Mathf.Abs(RB.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(RB.velocity.x)))
        {
            velPower = data.turnPower;
        }
        else
        {
            velPower = data.accelPower;
        }
        #endregion

        //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(RB.velocity.x, movement, lerpAmount);

        //Debug.Log("Run");
        RB.AddForce(movement * Vector2.right); //applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 

        if (InputHandler.instance.MoveInput.x != 0)
        {
            //Inverse to looking Side for this animation thats why it is inverted
            var isLookingLeft = InputHandler.instance.MoveInput.x <= 0;
            _facingController.CheckDirectionToFace(isLookingLeft);
        }

    }
    #endregion //RopeClimb
    public void CleanCurrentZipline()
    {
        isOnZip = false;
        currentZipline = null;
    }
    internal void ResetWallJump()
    {
        LastWallJumpHit = WallJumpHitState.None;
    }


    internal bool IsDeadlyFall()
    {
        return _currentHealth - GetFallDamage() <= 0;
        //var fallDamage = 10 + 
    }
    internal int GetFallDamage()
    {
        if (LastFallPower < data.fallDamageLimit)
        {
            return 0;
        }

        if (LastFallPower > data.fallDyingLimit)
        {
            return 300;
        }

        var hurtDiff = (LastFallPower - data.fallDamageLimit) / 10;
        var curve = data.fallDamageCurve.Evaluate(hurtDiff);
        return (int)(curve * 100);
    }

    internal void PlaySmoke()
    {
        _playerFallSmoke.Play();
    }
    #endregion

    #region Events
    private void OnHealthPointAnswered(int maxHp, int currenthp)
    {
        _currentHealth = currenthp;
        _isLoadingComplete = true;
    }

    public void OnHealthChanged(HealthEvent hpEvt)
    {
        _currentHealth = _currentHealth + hpEvt.Difference;

        if (hpEvt.IsHpGain())
        {
            return;
        }

        //Pas de reset sur les blessures
        if (_currentHealth > 0 && LastWoundedTime < 0)
        {
            if (_dmgStunDurationMap.ContainsKey(hpEvt.Origin))
            {
                LastWoundedTime = _dmgStunDurationMap[hpEvt.Origin];
            }
            else
            {
                LastWoundedTime = 0.3f; //default value
            }
        }
    }

    public void OnStateEffectorTriggered(StateEffectorEvent seEvt)
    {
        switch (seEvt.Type)
        {
            case StateEffectorType.stun:
                _stunDirection = seEvt.Direction;
                LastStunTime = seEvt.Duration;
                break;
        }
    }

    private void OnEdgeDetected(PlayerEdgeEvent newValue)
    {
        isOnEdge = newValue.IsEdgeDetected;
        if (isOnEdge)
        {
            EdgePosition = newValue.EdgePosition;
        }
    }

    private void OnInventoryChanged(PlayerItemSO so)
    {
        if (!so.Type.IsHandleByStateMachine())
        {
            return;
        }

        if (_itemMap.ContainsKey(so.Type))
        {
            _itemMap[so.Type] = so;
            return;
        }

        _itemMap.Add(so.Type, so);
    }

    #endregion //Events

    void OnDrawGizmos()
    {

#if UNITY_EDITOR
        Gizmos.color = Color.white;
        Gizmos.DrawCube(_slopeCheckPoint.transform.position, _slopeCheckSize);

        Gizmos.color = Color.grey;
        Gizmos.DrawCube(_groundCheckPoint.transform.position, _groundCheckSize);
        Gizmos.DrawCube(_waterCheckPoint.transform.position, _waterCheckSize);
        Gizmos.DrawCube(_wallCheckPoint.transform.position, _wallCheckSize);
        Gizmos.DrawCube(_climbCheckPoint.transform.position, _climbCheckSize);
        Gizmos.DrawCube(_zipCheckPoint.transform.position, _zipCheckSize);
#endif


        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_detectioncollider.bounds.center, _detectioncollider.bounds.size);
        //if (_origin == null || _targetPoint == null)
        //      {
        //          return;
        //      }

        //Gizmos.color = Color.black;
        //Gizmos.DrawSphere(_origin, 0.05f);

        //if (_hitPoint != null)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawSphere(_hitPoint, 0.02f);
        //}

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(_origin, _targetPoint);
        //Gizmos.DrawRay(new Ray(_origin, Vector2.left));

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(_origin, _targetPoint);
        //Gizmos.DrawRay(new Ray(_origin, Vector2.right));

    }
}