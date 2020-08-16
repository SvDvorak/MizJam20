using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
	public float MovementSpeed;

	private Vector3 move;

	void Start()
    {
        
    }

    void FixedUpdate()
    {
	    transform.position += move * MovementSpeed * Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
		var value = context.ReadValue<Vector2>();
		move = new Vector3(value.x, value.y);
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
