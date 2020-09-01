using UnityEngine;

/// <summary>
/// Inspired by ThridPersonController from StandardAssets.
/// 
/// The scripts moves the GameObject using the assigned rigidBody.
/// How much it moves is determined by the root motion of the locomotion animations
/// 
/// </summary>
public class EnemyCharacter : MonoBehaviour
{
	[SerializeField] float movingTurnSpeed = 360;
	[SerializeField] float stationaryTurnSpeed = 180;
	[Range(0f, 2f)] [SerializeField] float masterTurnSpeed = 1f;
	[SerializeField] float jumpPower = 12f;
	[Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
	[SerializeField] float moveSpeedMultiplier = 1f;
	[SerializeField] float animSpeedMultiplier = 1f;
	[SerializeField] float groundCheckDistance = 0.1f;

	float turnAmount;
	float forwardAmount;

	public Rigidbody rb;
	public Animator animator;

	bool isGrounded;
	float origGroundCheckDistance;

	const float half = 0.5f;
	Vector3 groundNormal;

	//float capsuleHeight;		       // For crouching
	//Vector3 capsuleCenter;		   // For crouching
	//public CapsuleCollider capsule;  // For crouching
	//bool crouching;                  // For crouching


	void Start()
	{
		Validate();
		Calibrate();

		// animator.applyRootMotion = true; // This is by default

		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		origGroundCheckDistance = groundCheckDistance;
	}

	void Validate()
    {
		if (!animator) animator = GetComponent<Animator>();
		if (!animator) throw new MissingComponentException("Missing Animator component");
		if (!rb) rb = GetComponent<Rigidbody>();
		if (!rb) throw new MissingComponentException("Missing Rigidbody component");
	}


	/// <summary>
	/// Fix the Enemy properties, and make sure some of the values are set
	/// To make animation root motions work as we want it
	/// </summary>
	void Calibrate()
    {
		var navMeshAgent = rb.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		
		// Make sure the navMesh does not do slow stopping, or applying wrong stopping behaviour
		navMeshAgent.autoBraking = false;

		// Make sure the navmesh speed is set to 1. We are going to control the speed using the enemy character.
		navMeshAgent.speed = 1f;

		// Make agent not rotating the object
		navMeshAgent.updateRotation = false;

		rb.useGravity = true;
		rb.isKinematic = false;

		var collider = rb.gameObject.GetComponent<Collider>();
		collider.isTrigger = false;
	}

    private void Update()
    {
		CheckGroundStatus();
    }

    public void Move(Vector3 move, bool crouch, bool jump)
	{
		// Convert the world relative moveInput vector into a local-relative
		// Turn amount and forward amount required to head in the desired
		// direction.
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		turnAmount = Mathf.Atan2(move.x, move.z);
		forwardAmount = move.z;

		ApplyTurnRotation();

		// Control and velocity handling is different when grounded and airborne:
		if (isGrounded)
			HandleGroundedMovement(crouch, jump);
		//else
		//	HandleAirborneMovement();

		// Send input and other state parameters to the animator
		UpdateAnimator(move);
	}

	void UpdateAnimator(Vector3 move)
	{
		// Update the animator parameters
		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
		animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
		//animator.SetBool("OnGround", isGrounded);

		if (forwardAmount >= 0.001f)
			animator.SetBool("IsRunning", true);
		else
			animator.SetBool("IsRunning", false);

		if (!isGrounded)
		{
			animator.SetFloat("Jump", rb.velocity.y);
		}

		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (isGrounded && move.magnitude > 0)
		{
			animator.speed = animSpeedMultiplier;
		}
		else
		{
			// don't use that while airborne
			animator.speed = 1;
		}
	}

	void HandleAirborneMovement()
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
		rb.AddForce(extraGravityForce);

		groundCheckDistance = rb.velocity.y < 0 ? origGroundCheckDistance : 0.01f;
	}


	void HandleGroundedMovement(bool crouch, bool jump)
	{
		// check whether conditions are right to allow a jump:
		if (jump && !crouch && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
		{
			// jump!
			rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
			isGrounded = false;
			animator.applyRootMotion = false;
			groundCheckDistance = 0.1f;
		}
	}

	/// <summary>
	/// Turns the character, not by root motion.
	/// Master turn speed [0,2]:
	///   0: no turning,
	///   1: 180 degrees in 1 second,
	///   2: rotates directly to the goal,
	/// 
	/// Help the character turn faster (this is in addition to root rotation in the animation)
	/// </summary>
	void ApplyTurnRotation()
	{
		float turnSpeedProportionalToForwardAmount = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);

		if (masterTurnSpeed != 2f)
        {
			var turnSpeedMultiplier = (masterTurnSpeed * turnSpeedProportionalToForwardAmount) / ((2 - masterTurnSpeed));
			var finalTurnSpeed = Mathf.Min(turnSpeedMultiplier * Time.deltaTime, Mathf.Rad2Deg);
			transform.Rotate(0, turnAmount * finalTurnSpeed, 0);
		}
		else
			transform.Rotate(0, Mathf.Rad2Deg * turnAmount, 0);

	}

	/// <summary>
	/// Implementing this function will override the default root motion.
	/// Which allows us to modify the positional speed before it's applied.
	/// </summary>
	public void OnAnimatorMove()
	{
		if (isGrounded && Time.deltaTime > 0)
		{
			Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
			// We preserve the existing y part of the current velocity.
			v.y = rb.velocity.y;
			rb.velocity = v;
		}
		// If we want the animator to apply rotation too, uncomment the code below
		// transform.rotation = animator.deltaRotation * transform.rotation;
	}


	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
#if UNITY_EDITOR

		Vector3 raycastStartPoint = transform.position + (Vector3.up * 0.05f);
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(raycastStartPoint, raycastStartPoint + (Vector3.down * groundCheckDistance));
#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(raycastStartPoint, Vector3.down, out hitInfo, groundCheckDistance))
		{
			groundNormal = hitInfo.normal;
			isGrounded = true;
			animator.applyRootMotion = true;
		}
		else
		{
			isGrounded = false;
			groundNormal = Vector3.up;
			animator.applyRootMotion = false;
		}
	}

	public void DoRagdoll(AttackInfo attackInfo)
    {
		var ragdollEffect = GetComponent<RagdollEffect>();
		if (ragdollEffect)
		{
			// Disable stuff before ragdoll
			rb.isKinematic = true;
			rb.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
			rb.gameObject.GetComponent<Collider>().enabled = false;

			ragdollEffect.Activate();

			// Send force
			ragdollEffect.ApplyForce(attackInfo);
        }
		else
        {
			Debug.LogWarning("No Ragdoll effect for this character");
        }
    }

	public void DeactivateRagdoll()
	{
		var ragdollEffect = GetComponent<RagdollEffect>();
		if (ragdollEffect)
		{
			// Enable stuff
			rb.isKinematic = false;
			rb.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
			rb.gameObject.GetComponent<Collider>().enabled = true;

			ragdollEffect.Deactivate();
		}
		else
		{
			Debug.LogWarning("No Ragdoll effect for this character");
		}
	}

}
