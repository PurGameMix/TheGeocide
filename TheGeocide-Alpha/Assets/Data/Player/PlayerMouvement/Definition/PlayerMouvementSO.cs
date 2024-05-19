using Assets.Scripts.Player.Mouvement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/Mouvement")]
public class PlayerMouvementSO : ScriptableObject
{
	//PHYSICS
	[Header("Gravity")]
	public float gravityScale; //overrides rb.gravityScale
	public float waterGravityScale; //overrides rb.gravityScale
	public float fallGravityMult;
	public float quickFallGravityMult;
	public float fallSmokeLimit;
	public float fallDamageLimit;
	public float fallDyingLimit;
	public AnimationCurve fallDamageCurve;

	[Header("Drag")]
	public float dragAmount; //drag in air
	public float frictionAmount; //drag on ground

	[Header("Other Physics")]
	[Range(0, 0.5f)] public float coyoteTime; //grace time to Jump after the player has fallen off a platformer


	//GROUND
	[Header("Run")]
	public float runMaxSpeed;
	public float runAccel;
	public float runDeccel;
	public float backwardCastSpeed;
	public float forwardCastSpeed;

	[Header("Crouch")]
	public float crouchMaxSpeed;
	public float crouchAccel;
	public float crouchDeccel;

	[Header("Swim")]
	public float swimMaxSpeed;
	public float swimAccel;
	public float swimDeccel;

	[Range(0, 1)] public float accelInAir;
	[Range(0, 1)] public float deccelInAir;
	[Space(5)]
	[Range(.5f, 2f)] public float accelPower;   
	[Range(.5f, 2f)] public float stopPower;
	[Range(.5f, 2f)] public float turnPower;


	//JUMP
	[Header("Jump")]
	public float jumpForce;
	[Range(0, 1)] public float jumpCutMultiplier;
	[Space(10)]
	[Range(0, 0.5f)] public float jumpBufferTime; //time after pressing the jump button where if the requirements are met a jump will be automatically performed

	[Header("Wall Jump")]
	public Vector2 wallJumpForce;
	[Space(5)]
	[Range(0f, 1f)] public float wallJumpRunLerp; //slows the affect of player movement while wall jumping
	[Range(0f, 1.5f)] public float wallJumpTime;

	[Header("Climb")]
	public float climbMaxSpeed;
	public float climbAccel;
	public float climbDeccel;
	public Vector2 edgeBeginOffset;
	public Vector2 edgeEndOffset;

	[Header("Slop Slide")]
	public float slopeMaxSpeed;

	//WALL
	[Header("Wall Slide")]
	public float slideAccel;
	[Range(.0f, 2f)] public float slidePower;


	//ABILITIES
	[Header("Dash")]
	public int dashAmount;
	public float dashSpeed;
	[Space(5)]
	public float dashAttackTime;
	public float landingAttackTime;
	public float dashAttackDragAmount;
	[Space(5)]
	public float dashEndTime; //time after you finish the inital drag phase, smoothing the transition back to idle (or any standard state)
	public float landingEndTime;
	[Range(0f, 1f)] public float dashUpEndMult; //slows down player when moving up, makes dash feel more responsive (used in Celeste)
	[Range(0f, 1f)] public float dashEndRunLerp; //slows the affect of player movement while dashing
	[Space(5)]
	[Range(0, 0.5f)] public float dashBufferTime;

	[Header("Collider")]
	public Vector2 StandOffset;
	public Vector2 StandSize;
	public Vector2 CrouchOffset;
	public Vector2 CrouchSize;
	//OTHER
	[Header("Other Settings")]
	public bool doKeepRunMomentum; //player movement will not decrease speed if above maxSpeed, letting only drag do so. Allows for conservation of momentum
	public bool doTurnOnWallJump; //player will rotate to face wall jumping direction


    internal float GetAccel(PlayerMovementType moveType, bool isInverse = false)
    {
        switch (moveType)
        {
			case PlayerMovementType.Run: return isInverse? runDeccel : runAccel;
			case PlayerMovementType.InAir: return isInverse? deccelInAir : accelInAir;
			case PlayerMovementType.Crouch: return isInverse ? crouchDeccel : crouchAccel;
			case PlayerMovementType.Swim: return isInverse ? swimDeccel : swimAccel;
			default: return isInverse ? runDeccel : runAccel;

		}
    }
}

//Think a setting should be renamed or added? Reach out to me @DawnosaurDev
