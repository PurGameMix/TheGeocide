using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
	public static InputHandler instance;

	[SerializeField]
	private PlayerInput playerInput;

	[Header("Debug")]
	[SerializeField]
	private bool resetInstance;

	public Action<InputArgs> OnDeplacement;
	public Action<InputArgs> OnJumpPressed;

	internal static string GetBindingDisplayString(string actionName)
    {
		return instance.playerInput.actions[actionName].GetBindingDisplayString();
	}

    //public Action<InputArgs> OnJumpReleased;
    public Action<InputArgs> OnDash;
	public Action<InputArgs> OnLandingAttack;
	public Action<InputArgs> OnUppercutAttack;
	public Action<InputArgs> OnUltimateAttack;


	public Action<InputArgs> OnMeleeAttackTap;
	private bool _isHoldingMeleeAttack;
	public Action<InputArgs> OnMeleeAttackHoldStart;
	public Action<InputArgs> OnMeleeAttackHoldCancel;

	public Action<InputArgs> OnRangedAttackTap;
	private bool _isHoldingRangedAttack;
	public Action<InputArgs> OnRangedAttackHoldStart;
	public Action<InputArgs> OnRangedAttackHoldCancel;

	//GUI
	public Action<InputArgs> OnInteraction;
	public Action<InputArgs> OnEscape;
	public Action<InputArgs> OnInventory;
	public Vector2 MoveInput { get; private set; }

    private void Awake()
	{
		#region Singleton
		if (instance == null || resetInstance)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		#endregion

		//controls = new Controls();

		#region Assign Inputs
		playerInput.actions["Movement"].performed += HandleDeplacementBehavior;
		playerInput.actions["Movement"].canceled += ctx => MoveInput = Vector2.zero;
		playerInput.actions["Jump"].performed += HandleJumpBehavior;
		playerInput.actions["Dash"].performed += ctx => OnDash(new InputArgs { context = ctx });
		playerInput.actions["LandingAttack"].performed += ctx => OnLandingAttack(new InputArgs { context = ctx });
		playerInput.actions["UppercutAttack"].performed += ctx => OnUppercutAttack(new InputArgs { context = ctx });
		playerInput.actions["UltimateAttack"].performed += ctx => OnUltimateAttack(new InputArgs { context = ctx });
		playerInput.actions["MeleeAttack"].performed += HandleMeleeAttackBehavior;
		playerInput.actions["MeleeAttack"].canceled += HandleMeleeAttackCancelBehavior;
		playerInput.actions["RangedAttack"].performed += HandleRangedAttackBehavior;
		playerInput.actions["RangedAttack"].canceled += HandleRangedAttackCancelBehavior;

		playerInput.actions["Interact"].performed += ctx => OnInteraction(new InputArgs { context = ctx });
		playerInput.actions["Escape"].performed += ctx => OnEscape(new InputArgs { context = ctx });
		playerInput.actions["Inventory"].performed += ctx => OnInventory(new InputArgs { context = ctx });		
		#endregion
	}

    private void OnDestroy()
	{
		playerInput.actions["Movement"].performed -= ctx => MoveInput = ctx.ReadValue<Vector2>();
		playerInput.actions["Movement"].canceled -= ctx => MoveInput = Vector2.zero;

		playerInput.actions["Jump"].performed -= HandleJumpBehavior;
		//controls.Player.JumpUp.performed -= ctx => OnJumpReleased(new InputArgs { context = ctx });
		playerInput.actions["Dash"].performed -= ctx => OnDash(new InputArgs { context = ctx });
		playerInput.actions["LandingAttack"].performed -= ctx => OnLandingAttack(new InputArgs { context = ctx });
		playerInput.actions["UppercutAttack"].performed -= ctx => OnUppercutAttack(new InputArgs { context = ctx });
		playerInput.actions["UltimateAttack"].performed -= ctx => OnUltimateAttack(new InputArgs { context = ctx });

		playerInput.actions["MeleeAttack"].performed -= HandleMeleeAttackBehavior;
		playerInput.actions["MeleeAttack"].canceled -= HandleMeleeAttackCancelBehavior;

		playerInput.actions["RangedAttack"].performed -= HandleRangedAttackBehavior;
		playerInput.actions["RangedAttack"].canceled -= HandleRangedAttackCancelBehavior;
	}

	#region Events
	public class InputArgs
	{
		public InputAction.CallbackContext context;
	}
	#endregion

	#region Special input behavior
	private void HandleMeleeAttackBehavior(InputAction.CallbackContext ctx)
	{
		if (ctx.interaction is TapInteraction)
		{
			OnMeleeAttackTap(new InputArgs { context = ctx });
			return;
		}
		if (ctx.interaction is HoldInteraction)
		{
			_isHoldingMeleeAttack = true;
			OnMeleeAttackHoldStart(new InputArgs { context = ctx });
			return;
		}
	}

	private void HandleMeleeAttackCancelBehavior(InputAction.CallbackContext ctx)
	{
		if (ctx.interaction is HoldInteraction && _isHoldingMeleeAttack)
		{
			OnMeleeAttackHoldCancel(new InputArgs { context = ctx });
			_isHoldingMeleeAttack = false;
		}
	}

	private void HandleRangedAttackBehavior(InputAction.CallbackContext ctx)
	{
		{
			if (ctx.interaction is TapInteraction)
			{
				OnRangedAttackTap(new InputArgs { context = ctx });
				return;
			}
			if (ctx.interaction is HoldInteraction)
			{
				_isHoldingRangedAttack = true;
				OnRangedAttackHoldStart(new InputArgs { context = ctx });
				return;
			}
		};
	}

	private void HandleRangedAttackCancelBehavior(InputAction.CallbackContext ctx)
	{
		if (ctx.interaction is HoldInteraction && _isHoldingRangedAttack)
		{
			OnRangedAttackHoldCancel(new InputArgs { context = ctx });
			_isHoldingRangedAttack = false;
		}
	}

	private void HandleDeplacementBehavior(InputAction.CallbackContext ctx)
    {
		MoveInput = ctx.ReadValue<Vector2>();
		OnDeplacement(new InputArgs { context = ctx });
	}
	private void HandleJumpBehavior(InputAction.CallbackContext ctx)
	{
		//if (ctx.interaction is TapInteraction)
		//{
			OnJumpPressed(new InputArgs { context = ctx });
		//	return;
		//}
		//if (ctx.interaction is HoldInteraction)
		//{
		//	OnJumpReleased(new InputArgs { context = ctx });
		//	return;
		//}
	}
	#endregion //Special input behavior

	#region OnEnable/OnDisable
	private void OnEnable()
	{
		//controls.Enable();
	}

	private void OnDisable()
	{
		//controls.Disable();
	}
	#endregion
}

