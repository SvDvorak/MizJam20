using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
	public static GameObject Player;
	public float MovementSpeed;

	private Rigidbody2D rb;
	private Vector2 move;

	void Awake()
	{
		Player = gameObject;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
    {
	    rb.position += move * MovementSpeed * Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
		var value = context.ReadValue<Vector2>();
		move = new Vector2(value.x, value.y);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
			Debug.Log("Interact!");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
			Debug.Log("Jump!");
    }
}
