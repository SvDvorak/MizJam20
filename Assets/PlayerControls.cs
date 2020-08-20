using Assets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
	public static GameObject Player;
	public Rigidbody2D Shadow;
	public float MovementSpeed;
	public float RejumpDelay;
	public float Gravity;

	private Rigidbody2D rb;

	private Vector2 shadowOffset;
	private Vector2 shadowInitialScale;

	private Vector2 moveVelocity;
	private bool isMoveJumping;
	private float startY;
	private float upVelocity;
	private Vector2 moveInput;
	private float autoRejumpTime;
	private float jumpRotation;

	private bool TimeToJumpAgain => autoRejumpTime < Time.time;

	void Awake()
	{
		Player = gameObject;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		shadowOffset = Shadow.transform.localPosition;
		shadowInitialScale = Shadow.transform.localScale;
	}

	void FixedUpdate()
	{
		var distanceFromJumpStart = rb.position.y - startY;
		var moveActual = Vector2.zero;
		if (isMoveJumping)
		{
			moveActual = moveInput;
			if(distanceFromJumpStart < 0)
			{
				upVelocity = 0;
				moveVelocity = Vector2.zero;
				isMoveJumping = false;
				Shadow.position = rb.position + shadowOffset;
				autoRejumpTime = Time.time + RejumpDelay;
			}
			else
			{
				upVelocity += Gravity * Time.fixedDeltaTime;
			}
		}

		//Re-jump automatically
		if(!isMoveJumping && TimeToJumpAgain && moveInput.sqrMagnitude > 0.01f)
		{
			StartMoveJump();
		}


		moveVelocity = (moveVelocity + moveActual * 0.05f) * 0.97f;

		startY += moveVelocity.y * MovementSpeed * Time.fixedDeltaTime;
		rb.velocity = moveVelocity * MovementSpeed + new Vector2(0, upVelocity);
		Shadow.velocity = moveVelocity * MovementSpeed;
		Shadow.transform.localScale = shadowInitialScale * Mathf.Clamp(1 - distanceFromJumpStart * 0.5f, 0.5f, 1f);
		var requestedJumpRotation = -moveVelocity.x * 15;

		jumpRotation += (requestedJumpRotation - jumpRotation) * 0.3f;
		transform.rotation = Quaternion.Euler(0, 0, jumpRotation);

		var scaling = distanceFromJumpStart * Mathf.Abs(moveVelocity.y) * 0.15f;
		transform.localScale = new Vector2(1 - scaling, 1 + scaling);
	}

	public void Move(InputAction.CallbackContext context)
    {
		var value = context.ReadValue<Vector2>();
		moveInput = new Vector2(value.x, value.y);

	    if (!isMoveJumping && context.performed && TimeToJumpAgain)
	    {
		    StartMoveJump();
	    }
    }

	private void StartMoveJump()
    {
	    moveVelocity = moveInput;

	    isMoveJumping = true;
	    startY = rb.position.y;
	    upVelocity = 2f;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
			Debug.Log("Interact!");
    }

    public void Boost(InputAction.CallbackContext context)
    {
        if(context.performed)
			Debug.Log("Boost!");
    }
}
